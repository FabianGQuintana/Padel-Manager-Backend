using PadelManager.Application.DTOs.Zone;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepo;
        private readonly IRegistrationRepository _registrationRepo;
        private readonly IStageRepository _stageRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public ZoneService(
            IZoneRepository zoneRepo,
            IRegistrationRepository registrationRepo,
            IStageRepository stageRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _zoneRepo = zoneRepo;
            _unitOfWork = unitOfWork;
            _registrationRepo = registrationRepo;
            _stageRepo = stageRepo;
            _currentUser = currentUser;
        }

        public async Task<ZoneResponseDto> AddZoneAsync(CreateZoneDto dto)
        {
            var zone = dto.ToEntity();
            var user = _currentUser.UserName ?? "System";

            zone.CreatedBy = user;
            zone.LastModifiedBy = user;

            var result = await _zoneRepo.AddAsync(zone);
            await _unitOfWork.SaveChangesAsync();

            return result.ToResponseDto();
        }

        public async Task<bool> UpdateZoneAsync(Guid id, UpdateZoneDto dto)
        {
            var existingZone = await _zoneRepo.GetByIdAsync(id);
            if (existingZone == null) return false;

            existingZone.MapToEntity(dto);

            existingZone.LastModifiedBy = _currentUser.UserName ?? "System";
            existingZone.LastModifiedAt = DateTime.UtcNow;

            await _zoneRepo.UpdateAsync(existingZone);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteZoneAsync(Guid id)
        {
            var zone = await _zoneRepo.GetZoneWithDetailsByIdAsync(id);
            if (zone == null) return false;

            var user = _currentUser.UserName ?? "System";
            zone.LastModifiedBy = user;
            zone.LastModifiedAt = DateTime.UtcNow;

            // 2. Si la estamos intentando desactivar (Soft Delete)
            if (!zone.DeletedAt.HasValue)
            {
                // REGLA: No se borra si tiene partidos generados
                if (zone.Matches != null && zone.Matches.Any(m => m.DeletedAt == null))
                {
                    throw new InvalidOperationException("No se puede eliminar la Zona: ya tiene partidos asignados.");
                }

                
                if (zone.Statistics != null && zone.Statistics.Any(s => s.DeletedAt == null))
                {
                    throw new InvalidOperationException("No se puede eliminar la Zona: tiene parejas registradas en el grupo.");
                }

                zone.DeletedAt = DateTime.UtcNow;
                zone.Status = "Inactive";
            }
            else
            {
                
                zone.DeletedAt = null;
                zone.Status = "Active";
            }

            await _zoneRepo.UpdateAsync(zone);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public IEnumerable<int> GetZoneDistribution(int totalCouples)
        {
            if (totalCouples < 6)
                throw new InvalidOperationException("Mínimo 6 parejas para armar zonas.");

            var zoneSizes = new List<int>();
            int remainder = totalCouples % 3;

            if (remainder == 0)
            {
                int zones = totalCouples / 3;
                for (int i = 0; i < zones; i++) zoneSizes.Add(3);
            }
            else if (remainder == 1)
            {
                int zonesOfThree = (totalCouples - 4) / 3;
                for (int i = 0; i < zonesOfThree; i++) zoneSizes.Add(3);
                zoneSizes.Add(4);
            }
            else
            {
                int zonesOfThree = (totalCouples - 8) / 3;
                for (int i = 0; i < zonesOfThree; i++) zoneSizes.Add(3);
                zoneSizes.Add(4);
                zoneSizes.Add(4);
            }

            return zoneSizes;
        }

        public bool IsRegistrationCountIdeal(int count) => count % 3 == 0;

        public async Task<bool> GenerateZonesWithDrawAsync(Guid categoryId)
        {
            var groupStage = await _stageRepo.GetGroupStageByCategoryAsync(categoryId);
            if (groupStage == null) return false;

            var couples = await _registrationRepo.GetCouplesByCategoryAsync(categoryId);
            if (couples.Count < 6) throw new Exception("No hay suficientes parejas (mínimo 6).");

            var random = new Random();
            var shuffledCouples = couples.OrderBy(c => random.Next()).ToList();
            var distribution = GetZoneDistribution(shuffledCouples.Count);
            var user = _currentUser.UserName ?? "System";

            int currentCoupleIndex = 0;
            int zoneNumber = 1;

            foreach (var size in distribution)
            {
                var newZone = new Zone
                {
                    Id = Guid.NewGuid(),
                    Name = $"Zona {zoneNumber++}",
                    StageId = groupStage.Id,
                    Stage = null!,
                    Couples = new List<Couple>(),
                    CreatedBy = user,
                    LastModifiedBy = user,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow
                };

                for (int i = 0; i < size; i++)
                {
                    newZone.Couples.Add(shuffledCouples[currentCoupleIndex]);
                    currentCoupleIndex++;
                }

                await _zoneRepo.AddAsync(newZone);
            }

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<ZoneResponseDto?> GetZoneByIdAsync(Guid id)
        {
            var zone = await _zoneRepo.GetByIdAsync(id);
            return zone?.ToResponseDto();
        }

        public async Task<IEnumerable<ZoneResponseDto>> GetZonesByNameAsync(string name)
        {
            var zones = await _zoneRepo.GetZonesByNameAsync(name);
            return zones.ToResponseDto();
        }

        public async Task<IEnumerable<ZoneResponseDto>> GetAllZonesAsync()
        {
            var zones = await _zoneRepo.GetAllAsync();
            return zones.ToResponseDto();
        }

        public async Task<ZoneResponseDto?> GetZoneWithDetailsAsync(Guid id)
        {
            var zoneWithDetails = await _zoneRepo.GetZoneWithDetailsByIdAsync(id);
            return zoneWithDetails?.ToResponseDto();
        }
    }
}
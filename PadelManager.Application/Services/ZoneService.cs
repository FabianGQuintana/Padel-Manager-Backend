using PadelManager.Application.DTOs.Zone;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepo;
        private readonly IRegistrationRepository _registrationRepo;
        private readonly IStageRepository _stageRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ZoneService(IZoneRepository zoneRepo,
            IRegistrationRepository registrationRepo,
            IStageRepository stageRepo,
            IUnitOfWork unitOfWork)
        {
            _zoneRepo = zoneRepo;
            _unitOfWork = unitOfWork;
            _registrationRepo = registrationRepo;
            _stageRepo = stageRepo;
        }

        public async Task<ZoneResponseDto> AddZoneAsync(CreateZoneDto dto)
        {
            // 1. Transformamos DTO a Entidad
            var zone = dto.ToEntity();

            // 2. Guardamos (La auditoría ICurrentUser se dispara sola en el SaveChanges)
            var result = await _zoneRepo.AddAsync(zone);

            // 3. Devolvemos el DTO de respuesta
            return result.ToResponseDto();
        }

        public async Task<bool> UpdateZoneAsync(Guid id, UpdateZoneDto dto)
        {
            var existingZone = await _zoneRepo.GetByIdAsync(id);
            if (existingZone == null) return false;

          
            existingZone.MapToEntity(dto);

            await _zoneRepo.UpdateAsync(existingZone);
            return true;
        }

        public async Task<bool> SoftDeleteZoneAsync(Guid id)
        {
            // 1. Buscamos la zona incluyendo los hijos que queremos "apagar"
            var zone = await _zoneRepo.GetZoneWithDetailsByIdAsync(id);
            if (zone == null) return false;

            // 2. Usamos la lógica de Toggle pero extendida
            if (zone.DeletedAt.HasValue)
            {
                zone.DeletedAt = null;
                zone.Status = "Active";
                // Opcional: ¿Restaurar también los partidos? Depende de tu regla de negocio
            }
            else
            {
                zone.DeletedAt = DateTime.UtcNow;
                zone.Status = "Inactive";

                // 🚀 CASCADA MANUAL: Apagamos los partidos de esta zona
                foreach (var match in zone.Matches)
                {
                    match.DeletedAt = DateTime.UtcNow;
                    match.StatusType = MatchStatus.Canceled;
                }
            }

            // 3. Guardamos todo mediante el Unit of Work
            await _zoneRepo.UpdateAsync(zone);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public  IEnumerable<int> GetZoneDistribution(int totalCouples)
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
            else // remainder == 2
            {
                int zonesOfThree = (totalCouples - 8) / 3;
                for (int i = 0; i < zonesOfThree; i++) zoneSizes.Add(3);
                zoneSizes.Add(4);
                zoneSizes.Add(4);
            }

            return zoneSizes; // C# lo convierte automáticamente a IEnumerable
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

            // 🎯 Aquí ya no da error el foreach porque el método no es Task
            var distribution = GetZoneDistribution(shuffledCouples.Count);

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
                    Couples = new List<Couple>()
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
            var zoneId = await _zoneRepo.GetByIdAsync(id);
            return zoneId?.ToResponseDto();
        }

        public async Task<IEnumerable<ZoneResponseDto>> GetZonesByNameAsync(string name)
        {
           var zonesName = await _zoneRepo.GetZonesByNameAsync(name);
            return zonesName.ToResponseDto();
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
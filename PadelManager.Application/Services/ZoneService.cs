using PadelManager.Application.DTOs.Zone;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;

namespace PadelManager.Application.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepo;

        public ZoneService(IZoneRepository zoneRepo)
        {
            _zoneRepo = zoneRepo;
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
            
            var result = await _zoneRepo.SoftDeleteToggleAsync(id);
            return result != null;
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
using PadelManager.Application.DTOs.Zone;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IZoneService
    {
        Task<ZoneResponseDto> AddZoneAsync(CreateZoneDto dto);
        Task<bool> UpdateZoneAsync(Guid id, UpdateZoneDto dto);
        Task<bool> SoftDeleteZoneAsync(Guid id);

        // Consultas
        Task<ZoneResponseDto?> GetZoneByIdAsync(Guid id);
        Task<IEnumerable<ZoneResponseDto>> GetZonesByNameAsync(string name);
        Task<IEnumerable<ZoneResponseDto>> GetAllZonesAsync();

        // Trae la zona con sus Parejas y Partidos
        Task<ZoneResponseDto?> GetZoneWithDetailsAsync(Guid id);
    }

}

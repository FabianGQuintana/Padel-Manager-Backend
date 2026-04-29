using PadelManager.Application.DTOs.Tournament;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

public static class TournamentMapper
{
    public static TournamentResponseDto ToResponseDto(this Tournament entity, int registrationCount = 0)
    {
        return new TournamentResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            StartDate = entity.StartDate,
            Regulations = entity.Regulations,
            StatusType = entity.StatusType.ToString(),
            IsActive = entity.Status,
            TournamentType = entity.TournamentType,

            Managers = entity.Managers.Select(m => new TournamentManagerSummaryDto
            {
                Id = m.Id,
                // Accedemos a la navegación m.User
                FullName = m.User != null
                ? $"{m.User.Name} {m.User.LastName}"
                : "Usuario no vinculado"
            }).ToList()
        };
    }

    // Corregimos la lista: pasamos 0 o el conteo si lo tenemos
    public static IEnumerable<TournamentResponseDto> ToResponseDto(this IEnumerable<Tournament> tournaments)
    {
        return tournaments.Select(t => t.ToResponseDto(0));
    }

    public static void MapToEntity(this Tournament existingEntity, UpdateTournamentDto dto)
    {
        if (dto.Name != null) existingEntity.Name = dto.Name;
        if (dto.Regulations != null) existingEntity.Regulations = dto.Regulations;
        if (dto.TournamentType != null) existingEntity.TournamentType = dto.TournamentType;

        if (dto.StartDate.HasValue)
            existingEntity.StartDate = dto.StartDate.Value;

        
    }

    public static Tournament ToEntity(this CreateTournamentDto dto)
    {
        return new Tournament
        {
            Name = dto.Name,
            Regulations = dto.Regulations,
            StartDate = dto.StartDate,
            TournamentType = dto.TournamentType,
            StatusType = TournamentStatus.Draft
            // Managers se inicializa vacío y se llena en el SERVICE
        };
    }
}
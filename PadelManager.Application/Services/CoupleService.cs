using PadelManager.Application.DTOs.Couple;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Application.DTOs.CoupleAvailability;

namespace PadelManager.Application.Services
{
    public class CoupleService : ICoupleService
    {
        private readonly ICoupleRepository _coupleRepository;
        private readonly IPlayerRepository _playerRepository;
        
        public CoupleService(
            ICoupleRepository coupleRepository,
            IPlayerRepository playerRepository)
        {
            _coupleRepository = coupleRepository;
            _playerRepository = playerRepository;
        }

        // =========================
        // CRUD BÁSICO
        // =========================

        public async Task<CoupleResponseDto> AddNewCoupleAsync(CreateCoupleDto dto)
        {
            await ValidatePlayersAsync(dto.Player1Id, dto.Player2Id);
            ValidateAvailabilities(dto.Availabilities);

            var existingCouples = await _coupleRepository.GetAllAsync();

            bool coupleAlreadyExists = existingCouples.Any(c =>
                (c.Player1Id == dto.Player1Id && c.Player2Id == dto.Player2Id) ||
                (c.Player1Id == dto.Player2Id && c.Player2Id == dto.Player1Id));

            if (coupleAlreadyExists)
                throw new Exception("La pareja ya existe");

            var couple = dto.ToEntity();

            await _coupleRepository.AddAsync(couple);

            var createdCouple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(couple.Id)
                               ?? await _coupleRepository.GetByIdAsync(couple.Id);

            return createdCouple!.ToResponseDto();
        }

        public async Task<bool> UpdateCoupleAsync(Guid id, UpdateCoupleDto dto)
        {
            var existingCouple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(id);

            if (existingCouple == null)
                return false;

            await ValidatePlayersAsync(dto.Player1Id, dto.Player2Id);

            bool tournamentStarted = HasTournamentStarted(existingCouple);

            if (!tournamentStarted)
            {
                ValidateAvailabilities(dto.Availabilities);
            }

            // Si el torneo empezó, actualiza todo menos availabilities
            dto.MapToEntity(existingCouple, updateAvailabilities: !tournamentStarted);

            await _coupleRepository.UpdateAsync(existingCouple);

            return true;
        }

        public async Task<bool> SoftDeleteToggleCoupleAsync(Guid id)
        {
            var existingCouple = await _coupleRepository.GetByIdAsync(id);

            if (existingCouple == null)
                return false;

            if (existingCouple.DeletedAt == null)
                existingCouple.DeletedAt = DateTime.UtcNow;
            else
                existingCouple.DeletedAt = null;

            await _coupleRepository.UpdateAsync(existingCouple);

            return true;
        }

        // =========================
        // LÓGICA DE NEGOCIO
        // =========================

        public async Task<bool> ReplacePlayerInCoupleAsync(Guid coupleId, ReplaceCouplePlayerDto dto)
        {
            var couple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(coupleId);

            if (couple == null)
                return false;

            // Validar que el jugador viejo forme parte de la pareja
            if (dto.OldPlayerId != couple.Player1Id && dto.OldPlayerId != couple.Player2Id)
                throw new Exception("El jugador a reemplazar no pertenece a la pareja");

            // Validar que el nuevo jugador exista
            var newPlayer = await _playerRepository.GetByIdAsync(dto.NewPlayerId);
            if (newPlayer == null)
                throw new Exception("El nuevo jugador no existe");

            // Validar que no quede duplicado
            if (dto.NewPlayerId == couple.Player1Id || dto.NewPlayerId == couple.Player2Id)
                throw new Exception("Una pareja no puede tener el mismo jugador dos veces");

            // Reemplazo
            if (dto.OldPlayerId == couple.Player1Id)
                couple.Player1Id = dto.NewPlayerId;
            else
                couple.Player2Id = dto.NewPlayerId;

            // Regla:
            // si el torneo ya empezó, NO se tocan availabilities
            // si no empezó, tampoco las tocamos acá porque este método es solo para reemplazo
            // las availabilities se modifican desde UpdateCoupleAsync

            await _coupleRepository.UpdateAsync(couple);

            return true;
        }

        // =========================
        // LECTURA
        // =========================

        public async Task<CoupleResponseDto?> GetCoupleByIdAsync(Guid coupleId)
        {
            var couple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(coupleId)
                        ?? await _coupleRepository.GetByIdAsync(coupleId);

            return couple?.ToResponseDto();
        }

        public async Task<CoupleResponseDto?> GetCoupleByNicknameAsync(string nickname)
        {
            var couple = await _coupleRepository.GetCoupleByNicknameAsync(nickname);
            return couple?.ToResponseDto();
        }

        public async Task<IEnumerable<CoupleResponseDto>> GetAllCouplesAsync()
        {
            var couples = await _coupleRepository.GetAllAsync();
            return couples.ToResponseDto();
        }

        public async Task<IEnumerable<CoupleResponseDto>> GetCouplesByPlayerIdAsync(Guid playerId)
        {
            var couples = await _coupleRepository.GetCouplesByPlayerIdAsync(playerId);
            return couples.ToResponseDto();
        }

        public async Task<IEnumerable<CoupleResponseDto>> GetCouplesByZoneIdAsync(Guid zoneId)
        {
            var couples = await _coupleRepository.GetCouplesByZoneIdAsync(zoneId);
            return couples.ToResponseDto();
        }

        public async Task<IEnumerable<CoupleResponseDto>> GetCouplesWithoutZoneAsync()
        {
            var couples = await _coupleRepository.GetCouplesWithoutZoneAsync();
            return couples.ToResponseDto();
        }

        // =========================
        // VALIDACIONES PRIVADAS
        // =========================

        private async Task ValidatePlayersAsync(Guid player1Id, Guid player2Id)
        {
            if (player1Id == player2Id)
                throw new Exception("Una pareja no puede tener el mismo jugador dos veces");

            var player1 = await _playerRepository.GetByIdAsync(player1Id);
            var player2 = await _playerRepository.GetByIdAsync(player2Id);

            if (player1 == null || player2 == null)
                throw new Exception("uno o ambos jugadores no existen");
        }

        private static void ValidateAvailabilities(List<CreateCoupleAvailabilityDto> availabilities)
        {
            if (availabilities == null || !availabilities.Any())
                throw new Exception("La pareja debe tener al menos una disponibilidad");

            foreach (var availability in availabilities)
            {
                if (availability.From >= availability.To)
                    throw new Exception("La hora de inicio debe ser menor a la hora de fin");
            }

            ValidateAvailabilityOverlaps(
                availabilities.Select(a => new AvailabilityCheckDto
                {
                    Day = a.Day,
                    From = a.From,
                    To = a.To
                }).ToList());
        }

        private static void ValidateAvailabilities(List<UpdateCoupleAvailabilityDto> availabilities)
        {
            if (availabilities == null || !availabilities.Any())
                throw new Exception("La pareja debe tener al menos una disponibilidad.");

            foreach (var availability in availabilities)
            {
                //  Validamos que no sean nulos antes de comparar
                if (!availability.From.HasValue || !availability.To.HasValue)
                    throw new Exception("Las horas de inicio y fin son obligatorias.");

                if (availability.From >= availability.To)
                    throw new Exception("La hora de inicio debe ser menor que la hora de fin.");
            }

            ValidateAvailabilityOverlaps(
                availabilities.Select(a => new AvailabilityCheckDto
                {
                    Day = a.Day,
                    From = a.From.Value,
                    To = a.To.Value
                }).ToList());
        }

        private static void ValidateAvailabilityOverlaps(List<AvailabilityCheckDto> availabilities)
        {
            var groupedByDay = availabilities
                .GroupBy(a => a.Day);

            foreach (var dayGroup in groupedByDay)
            {
                var orderedSlots = dayGroup
                    .OrderBy(a => a.From)
                    .ToList();

                for (int i = 0; i < orderedSlots.Count - 1; i++)
                {
                    if (orderedSlots[i].To > orderedSlots[i + 1].From)
                        throw new Exception($"Existen horarios superpuestos el día {dayGroup.Key}.");
                }
            }
        }

        private static bool HasTournamentStarted(Couple couple)
        {
            var tournament = couple.Registrations?
                .FirstOrDefault()?.Tournament;

            if (tournament == null)
                return false;

            return tournament.StatusType == TournamentStatus.InProgress ||
                   DateTime.UtcNow >= tournament.StartDate;
        }

        // DTO interno auxiliar para validación
        private class AvailabilityCheckDto
        {
            public DayOfWeek? Day { get; set; }
            public TimeOnly From { get; set; }
            public TimeOnly To { get; set; }
        }
    }
}
using PadelManager.Application.DTOs.CoupleAvailability;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Services
{
    public class CoupleAvailabilityService : ICoupleAvailabilityService
    {
        private readonly ICoupleAvailabilityRepository _coupleAvailabilityRepository;
        private readonly ICoupleRepository _coupleRepository;

        public CoupleAvailabilityService(
            ICoupleAvailabilityRepository coupleAvailabilityRepository,
            ICoupleRepository coupleRepository)
        {
            _coupleAvailabilityRepository = coupleAvailabilityRepository;
            _coupleRepository = coupleRepository;
        }

        // =========================
        // CRUD BÁSICO
        // =========================

        public async Task<CoupleAvailabilityResponseDto> AddNewAvailabilityAsync(CreateCoupleAvailabilityDto dto)
        {
            var couple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(dto.CoupleId)
                        ?? await _coupleRepository.GetByIdAsync(dto.CoupleId);

            if (couple == null)
                throw new Exception("La pareja especificada no existe");

            if (HasTournamentStarted(couple))
                throw new Exception("No se pueden agregar disponibilidades una vez iniciado el torneo");

            ValidateAvailability(dto);

            var existingAvailabilities = await _coupleAvailabilityRepository.GetAvailabilitiesByCoupleIdAsync(dto.CoupleId);

            ValidateAvailabilityOverlaps(
                existingAvailabilities,
                dto.Day,
                dto.From,
                dto.To);

            var availability = dto.ToEntity();

            await _coupleAvailabilityRepository.AddAsync(availability);

            return availability.ToResponseDto();
        }

        public async Task<bool> UpdateAvailabilityAsync(Guid id, UpdateCoupleAvailabilityDto dto)
        {
            var existingAvailability = await _coupleAvailabilityRepository.GetByIdAsync(id);

            if (existingAvailability == null)
                return false;

            var couple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(existingAvailability.CoupleId)
                        ?? await _coupleRepository.GetByIdAsync(existingAvailability.CoupleId);

            if (couple == null)
                throw new Exception("La pareja asociada no existe");

            if (HasTournamentStarted(couple))
                throw new Exception("No se pueden modificar las disponibilidades una vez iniciado el torneo");

            var finalDay = dto.Day ?? existingAvailability.Day;
            var finalFrom = dto.From ?? existingAvailability.From;
            var finalTo = dto.To ?? existingAvailability.To;

            ValidateAvailability(finalDay, finalFrom, finalTo);

            var availabilities = await _coupleAvailabilityRepository.GetAvailabilitiesByCoupleIdAsync(existingAvailability.CoupleId);

            var otherAvailabilities = availabilities
                .Where(a => a.Id != existingAvailability.Id)
                .ToList();

            ValidateAvailabilityOverlaps(
                otherAvailabilities,
                finalDay,
                finalFrom,
                finalTo);

            existingAvailability.MapToEntity(dto);

            await _coupleAvailabilityRepository.UpdateAsync(existingAvailability);

            return true;
        }

        public async Task<bool> SoftDeleteToggleAvailabilityAsync(Guid id)
        {
            var existingAvailability = await _coupleAvailabilityRepository.GetByIdAsync(id);

            if (existingAvailability == null)
                return false;

            var couple = await _coupleRepository.GetCoupleWithRegistrationDetailsAsync(existingAvailability.CoupleId)
                        ?? await _coupleRepository.GetByIdAsync(existingAvailability.CoupleId);

            if (couple == null)
                throw new Exception("La pareja asociada no existe");

            if (HasTournamentStarted(couple))
                throw new Exception("No se pueden eliminar disponibilidades una vez iniciado el torneo");

            if (existingAvailability.DeletedAt == null)
                existingAvailability.DeletedAt = DateTime.UtcNow;
            else
                existingAvailability.DeletedAt = null;

            await _coupleAvailabilityRepository.UpdateAsync(existingAvailability);

            return true;
        }

        // =========================
        // LECTURA
        // =========================

        public async Task<CoupleAvailabilityResponseDto?> GetAvailabilityByIdAsync(Guid id)
        {
            var availability = await _coupleAvailabilityRepository.GetByIdAsync(id);
            return availability?.ToResponseDto();
        }

        public async Task<IEnumerable<CoupleAvailabilityResponseDto>> GetAllAvailabilitiesAsync()
        {
            var availabilities = await _coupleAvailabilityRepository.GetAllAsync();
            return availabilities.ToResponseDto();
        }

        public async Task<IEnumerable<CoupleAvailabilityResponseDto>> GetAvailabilitiesByCoupleIdAsync(Guid coupleId)
        {
            var availabilities = await _coupleAvailabilityRepository.GetAvailabilitiesByCoupleIdAsync(coupleId);
            return availabilities.ToResponseDto();
        }

        // =========================
        // VALIDACIONES PRIVADAS
        // =========================

        private static void ValidateAvailability(CreateCoupleAvailabilityDto availability)
        {
            if (availability.From >= availability.To)
                throw new Exception("La hora de inicio debe ser menor que la hora de fin");
        }

        private static void ValidateAvailability(DayOfWeek day, TimeOnly from, TimeOnly to)
        {
            if (from >= to)
                throw new Exception("La hora de inicio debe ser menor que la hora de fin");
        }

        private static void ValidateAvailabilityOverlaps(
            IEnumerable<CoupleAvailability> availabilities,
            DayOfWeek day,
            TimeOnly from,
            TimeOnly to)
        {
            bool overlapExists = availabilities.Any(a =>
                a.Day == day &&
                from < a.To &&
                to > a.From);

            if (overlapExists)
                throw new Exception($"Existen horarios superpuestos el día {day}.");
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
    }
}
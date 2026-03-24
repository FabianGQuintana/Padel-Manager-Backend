using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PadelManager.Application.DTOs.Registration;

namespace PadelManager.Application.Interfaces.Services
{
    public interface IRegistrationService
    {
        // CRUD BÁSICO
        Task<RegistrationResponseDto> AddNewRegistrationAsync(CreateRegistrationDto dto);

        Task<bool> UpdateRegistrationAsync(Guid id, UpdateRegistrationDto dto);

        Task<bool> SoftDeleteToggleRegistrationAsync(Guid id);


        // LÓGICA DE NEGOCIO
        //Evita que la misma pareja se anote dos veces al mismo torneo/categoría
        Task<bool> IsCoupleAlreadyRegisteredAsync(Guid coupleId, Guid categoryId);


        // LECTURA Y FILTROS
        Task<RegistrationResponseDto?> GetRegistrationByIdAsync(Guid registrationId);

        Task<IEnumerable<RegistrationResponseDto>> GetAllRegistrationsAsync();

        // Filtros por relaciones
        Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByTournamentIdAsync(Guid tournamentId);

        Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCategoryIdAsync(Guid categoryId);

        Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByCoupleIdAsync(Guid coupleId);

        // Filtros por fecha
        Task<IEnumerable<RegistrationResponseDto>> GetRegistrationsByDateAsync(DateOnly date);
    }
}
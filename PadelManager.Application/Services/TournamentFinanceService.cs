using PadelManager.Application.DTOs.TournamentFinance;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Application.Mappers.PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class TournamentFinanceService : ITournamentFinanceService
    {
        private readonly ITournamentFinanceRepository _financeRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public TournamentFinanceService(
            ITournamentFinanceRepository financeRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _financeRepo = financeRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        // ==========================================
        // COMANDOS
        // ==========================================

        public async Task<TournamentFinanceResponseDto> RegisterMovementAsync(CreateTournamentFinanceDto dto)
        {
            var finance = dto.ToEntity();

            // AUDITORÍA
            var user = _currentUser.UserName ?? "System";
            finance.CreatedBy = user;
            finance.LastModifiedBy = user;

            await _financeRepo.AddAsync(finance);
            await _unitOfWork.SaveChangesAsync();

            return finance.ToResponseDto();
        }

        public async Task<bool> UpdateMovementAsync(Guid id, UpdateTournamentFinanceDto dto)
        {
            var existingFinance = await _financeRepo.GetByIdAsync(id);
            if (existingFinance == null) return false;

            existingFinance.MapToEntity(dto);

            // AUDITORÍA
            existingFinance.LastModifiedBy = _currentUser.UserName ?? "System";
            existingFinance.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteToggleFinanceAsync(Guid id)
        {
            var finance = await _financeRepo.GetByIdAsync(id);
            if (finance == null) return false;

            // AUDITORÍA
            finance.LastModifiedBy = _currentUser.UserName ?? "System";
            finance.LastModifiedAt = DateTime.UtcNow;

            await _financeRepo.SoftDeleteToggleAsync(id);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // ==========================================
        // CONSULTAS
        // ==========================================

        public async Task<TournamentFinanceResponseDto?> GetFinanceByIdAsync(Guid id)
        {
            var finance = await _financeRepo.GetByIdAsync(id);
            return finance?.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentFinanceResponseDto>> GetAllFinancesAsync()
        {
            var finances = await _financeRepo.GetAllAsync();
            return finances.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentFinanceResponseDto>> GetFinancesByTournamentIdAsync(Guid tournamentId)
        {
            var finances = await _financeRepo.GetFinancesByTournamentIdAsync(tournamentId);
            return finances.ToResponseDto();
        }

        public async Task<IEnumerable<TournamentFinanceResponseDto>> GetFinancesByTypeAsync(TypeMovement type)
        {
            var finances = await _financeRepo.GetFinancesByTypeAsync(type);
            return finances.ToResponseDto();
        }

        public async Task<decimal> GetTotalBalanceByTournamentAsync(Guid tournamentId)
        {
            var finances = await _financeRepo.GetFinancesByTournamentIdAsync(tournamentId);

            // Calculamos el balance neto: Ingresos - Egresos
            // Asumiendo que TypeMovement tiene Income y Expense
            var total = finances
                .Where(f => f.DeletedAt == null)
                .Sum(f => f.MovementType == TypeMovement.Income ? f.Amount : -f.Amount);

            return total;
        }
    }
}
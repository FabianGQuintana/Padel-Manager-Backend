using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;


namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ITournamentFinanceRepository : IGenericRepository<TournamentFinance>
    {
        // Buscamos todos los movimientos de un torneo (Ingresos y Egresos)
        Task<IEnumerable<TournamentFinance>> GetFinancesByTournamentIdAsync(Guid tournamentId);

        //  Buscamos por tipo (ej. ver solo los gastos para rendir cuentas)
        Task<IEnumerable<TournamentFinance>> GetFinancesByTypeAsync(TypeMovement type);
    }
}
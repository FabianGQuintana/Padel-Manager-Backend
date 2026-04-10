using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface ICoupleRepository : IGenericRepository<Couple>
    {
        Task<Couple?> GetCoupleByNicknameAsync(string nickname);
        Task<IEnumerable<Couple>> GetCouplesByPlayerIdAsync(Guid playerId);
        

        Task <Couple?> GetCoupleWithRegistrationDetailsAsync(Guid coupleId);

    }
}

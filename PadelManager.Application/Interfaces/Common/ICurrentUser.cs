using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Interfaces.Common
{
    public interface ICurrentUser
    {
        string? UserId { get; }      // El ID del usuario (sub en el JWT)
        string? UserName { get; }    // El nombre/email del usuario
        bool IsAuthenticated { get; }
    }
}

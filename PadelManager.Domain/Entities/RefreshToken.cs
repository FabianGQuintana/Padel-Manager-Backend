using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }

        // Relación con el Usuario
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Propiedad calculada para saber si venció
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsRevoked && !IsUsed && !IsExpired;
    }
}

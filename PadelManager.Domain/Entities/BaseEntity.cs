using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public abstract class BaseEntity
    {
        // 1. Identificador único
        public required Guid Id { get; set; } = Guid.NewGuid();

       

        // 2. Auditoría (Fecha de Creación/Modificación)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";

        public DateTime? LastModifiedAt { get; set; } = DateTime.UtcNow;
        public string? LastModifiedBy { get; set; } 
    }
}

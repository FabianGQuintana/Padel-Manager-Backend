using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Sanction : BaseEntity
    {
        public required string Reason { get; set; }

        public required StatusSeverity Severity { get; set; }

        public  DateTime? ExpirationDate { get; set; }

        //Fk
        public Guid PlayerId { get; set; }

        public Player Player { get; set; } = null!;
    }
}

using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Sanction
{
    public class CreateSanctionDto
    {
        public string Reason { get; set; } = null!;
        public StatusSeverity Severity { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public Guid PlayerId { get; set; }
    }
}

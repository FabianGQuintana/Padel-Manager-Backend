using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Sanction
{
    public class UpdateSanctionDto
    {
        public string? Reason { get; set; }
        public StatusSeverity? Severity { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}

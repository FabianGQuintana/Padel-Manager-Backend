using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Sanction
{
    public class SanctionResponseDto
    {
        public Guid Id { get; set; }
        public string Reason { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public DateTime? ExpirationDate { get; set; }
        public Guid PlayerId { get; set; }
        public string IsActive { get; set; } = null!; 
    }
}

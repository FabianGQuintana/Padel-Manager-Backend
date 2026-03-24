using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Couple
{
    public class CoupleListDto
    {
        public Guid Id { get; set; }
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;
        public bool HasZoneAssigned { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Couple
{
    public class CoupleFilterDto
    {
        public string? Nickname { get; set; } // Apodo o nombre de la pareja (opcional)
        public string? PlayerName { get; set; } // Nombre de alguno de los jugadores (opcional)
        public Guid? ZoneId { get; set; } // Filtrar por zona asignada (opcional)
        public bool? HasZoneAssigned { get; set; } // Filtrar por parejas que tengan o no asignada una zona (opcional)
    }
}

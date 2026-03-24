
using System.Collections.Generic;
using System.Text;
using System;

namespace PadelManager.Application.DTOs.Registration
{
    public class CreateRegistrationDto
    {
        // Solo necesitamos saber qué pareja se anota, a qué categoría y a qué torneo
        public required Guid CoupleId { get; set; }

        public required Guid CategoryId { get; set; }

        public required Guid TournamentId { get; set; }

        // RegistrationDate y RegistrationTime no van acá
        // El Service se va a encargar de poner DateOnly.FromDateTime(DateTime.Now)
    }
}

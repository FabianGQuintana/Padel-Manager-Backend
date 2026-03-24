using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Registration
{
    public class UpdateRegistrationDto
    {
        // Todo opcional (?). 
        // Permite cambiar a la pareja de torneo o categoría si hubo un error administrativo.
        public Guid? CoupleId { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? TournamentId { get; set; }
    }
}

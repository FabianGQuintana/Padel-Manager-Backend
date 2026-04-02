using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Application.DTOs.Manager
{
    public class CreateManagerDto
    {
        // El ID del usuario al que le vamos a crear el perfil
        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        public Guid UserId { get; set; }

        public byte? YearExperience { get; set; }

        public string? LicenceAPA { get; set; }
    }
}

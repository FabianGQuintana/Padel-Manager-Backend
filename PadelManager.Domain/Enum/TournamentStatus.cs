using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Enum
{
    public enum TournamentStatus
    {
        Draft = 0, //Torneo Borrador
        RegistrationOpen = 1, // Torneo Inscripciones Abiertas
        InProgress = 2, // Torneo En Progreso
        Finished = 3, // Torneo Finalizado
        Cancelled = 4 // Torneo Cancelado
    }
}

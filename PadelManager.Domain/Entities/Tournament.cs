using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public required string Name { get; set; }

        public required DateTime StartDate { get; set; }

        public required string Regulations { get; set; }

        public required TournamentStatus Status { get; set; } = TournamentStatus.Draft;

        public required string TournamentType { get; set; } //Para diferenciar si es "Veteranos", "Libres" o "Menores".

        [Range(6, 48, ErrorMessage = "El torneo debe tener entre 6 y 48 parejas.")]
        public required int MaxTeamsPerCategory { get; set; } = 48;

        // Lógica de Negocio que usaremos en los SERVICES
        public bool IsFull(int registrationCount) => registrationCount >= MaxTeamsPerCategory;

        public bool CanStart(int registrationCount) => registrationCount >= 6 && registrationCount <= MaxTeamsPerCategory;

        public bool IsIdealForZones(int registrationCount) => registrationCount % 3 == 0;


        //Relationships FK
        public Guid ManagerId { get; set; }
        
        //Navigation properties
        public  ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Manager> Managers { get; set; } = new List<Manager>();

        // Método para validar si se puede cambiar el estado a "InProgress" 
        public bool CanChangeStatusToInProgress(int registrationCount)
        {
            //  Mínimo 6, Máximo el tope configurado
            if (registrationCount < 6 || registrationCount > MaxTeamsPerCategory)
            {
                return false;
            }

            //  No se puede empezar un torneo que ya terminó o ya está en curso
            if (Status != TournamentStatus.Draft)
            {
                return false;
            }

            return true;
        }

    }
}

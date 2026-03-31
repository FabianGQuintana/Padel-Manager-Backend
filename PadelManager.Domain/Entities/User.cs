using PadelManager.Domain.Entities;
using System.Collections.ObjectModel;

namespace PadelManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; }

        public required string LastName { get; set; }

        public required string Dni { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        //Fk
        public Guid RoleId { get; set; }

        //Navegations Relation
        public Role Role { get; set; } = null!;

        public Manager? Manager { get; set; } = null!;


    }
}

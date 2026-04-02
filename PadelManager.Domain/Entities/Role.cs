using PadelManager.Domain.Enum;

namespace PadelManager.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required TypeUser NameRol { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

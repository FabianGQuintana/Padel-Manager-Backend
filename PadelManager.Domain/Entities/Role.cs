namespace PadelManager.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string NameRol { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

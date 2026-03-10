using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Collections.Generic;

namespace PadelManager.Domain.Entities
{
    public class Instance : BaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        //Relationships FK
        public Guid StageId { get; set; }

        //Navigation properties
        public required Stage Stage { get; set; }

        public  ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
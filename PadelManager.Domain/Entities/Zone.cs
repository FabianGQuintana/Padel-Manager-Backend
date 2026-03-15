using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PadelManager.Domain.Entities
{
    public class Zone : BaseEntity
    {
        public required string Name { get; set; }

        //Relationships FK
        public Guid StageId { get; set; }

        //Navigation properties
        public required Stage Stage { get; set; }

        public  ICollection<Couple> Couples { get; set; } = new List<Couple>();

        public  ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}

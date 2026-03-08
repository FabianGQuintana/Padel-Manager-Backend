using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Couple_Match : BaseEntity
    {
        
            // Foreign Keys
            public Guid CoupleId { get; set; }
            public Guid MatchId { get; set; }

            // Navigation properties
            public required Couple Couple { get; set; }
            public required Match Match { get; set; }
        
    }
}

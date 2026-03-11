using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Match : BaseEntity   
    {
        public required string Loser { get; set; }
        public DateTime DateTime { get; set; }
        public required MatchStatus Status { get; set; }
        public required string LocationName { get; set; }
        public required string CourtName { get; set; }
        public required int  Set1_coupleA { get; set; }
        public required int Set1_coupleB { get; set; }
        public required int Set2_coupleA { get; set; }
        public required int Set2_coupleB { get; set; }
        public  int? Set3_coupleA { get; set; }
        public  int? Set3_coupleB { get; set; }

        //Relationships FK
        public Guid InstanceId { get; set; }

        public Guid CoupleId { get; set; }

        public Guid CoupleId2 { get; set; }

        //Navigation properties
        public  required Instance Instance { get; set; } 

        public required Couple Couple { get; set; }

        public required Couple Couple2 { get; set; }
    }
}

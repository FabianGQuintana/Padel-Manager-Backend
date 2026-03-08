using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    internal class Match : BaseEntity   
    {
        public required int LoserId { get; set; }
        public DateOnly DateTime { get; set; }
        public required bool status { get; set; }
        public required string LocationName { get; set; }
        public required int  set1_coupleA { get; set; }
        public required int set1_coupleB { get; set; }
        public required int set2_coupleA { get; set; }
        public required int set2_coupleB { get; set; }
        public required int set3_coupleA { get; set; }
        public required int set3_coupleB { get; set; }

        //Relationships FK
        public Guid IdInstance { get; set; }
        //Navigation properties
        public required Instance Instance { get; set; } 
    }
}

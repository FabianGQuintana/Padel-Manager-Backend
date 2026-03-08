using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    public class Statistics : BaseEntity
    {
        public int Wo { get; set; }

        public int SetsWon { get; set; }

        public int SetsLost { get; set; }

        public int GamesWon { get; set; }

        public int GamesLost { get; set; }

        //Relationships FK
        public Guid CoupleId { get; set; }

        public Guid ZoneId { get; set; }

        //Navigation properties
        public required Couple Couple { get; set; }

        public required Zone Zone { get; set; }
    }
}

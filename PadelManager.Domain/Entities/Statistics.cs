using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Entities
{
    internal class Statistics
    {
        public int IdStatistics { get; set; }
        public int Wo { get; set; }
        public int SetsWon { get; set; }
        public int SetsLost { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }

        public int IdCouple { get; set; }
        public int IdZone { get; set; }

        public virtual Couple Couple { get; set; } = null!;
        public virtual Zone Zone { get; set; } = null!;
    }
}

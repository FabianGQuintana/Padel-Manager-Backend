using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Couple
{
    public class ReplaceCouplePlayerDto
    {
        public Guid OldPlayerId { get; set; }
        public Guid NewPlayerId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PadelManager.Application.DTOs.Match
{
    public class CreateMatchDto
    {
        public required DateTime DateTime { get; set; }

        public required string LocationName { get; set; }

        public required string CourtName { get; set; }


        // Match entity instantiation relationships

        public required Guid StageId { get; set; }

        public Guid? ZoneId { get; set; }

        public required Guid CoupleId { get; set; }

        public required Guid CoupleId2 { get; set; }
    }
}

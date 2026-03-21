using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.DTOs.Zone
{
    public class CreateZoneDto
    {
        public string Name { get; set; } = null!;

        public Guid stageId {  get; set; }
    }
}

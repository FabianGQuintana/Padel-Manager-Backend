using System;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Stage
{
    public class UpdateStageDto
    {
        public string? Name { get; set; }

        public StageType? Type { get; set; }

        public int? Order { get; set; }
    }
}

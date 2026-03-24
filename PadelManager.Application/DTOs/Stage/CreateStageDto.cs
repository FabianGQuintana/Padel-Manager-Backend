using System;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.DTOs.Stage
{
    public class CreateStageDto
    {

        public required string Name { get; set; }

        public required StageType Type { get; set; }

        public required int Order { get; set; }

        public required Guid CategoryId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PadelManager.Domain.Entities
{
    internal class Zone : BaseEntity
    {
        public required string Name { get; set; }

        //Relationships FK
        public Guid IdStage { get; set; }

        //Navigation properties
        public required Stage Stage { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PadelManager.Domain.Entities;

public class Instance
{
    public int IdInstance { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int IdStage { get; set; }
    public virtual Stage Stage { get; set; } = null!;
    public virtual ICollection<System.Text.RegularExpressions.Match> Matches { get; set; } = new List<System.Text.RegularExpressions.Match>();
}
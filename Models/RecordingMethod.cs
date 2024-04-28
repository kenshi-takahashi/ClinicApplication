using System;
using System.Collections.Generic;

namespace Сlinic.Models;

public partial class RecordingMethod
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Registry> Registries { get; set; } = new List<Registry>();
}

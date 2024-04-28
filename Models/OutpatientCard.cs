using System;
using System.Collections.Generic;

namespace Сlinic.Models;

public partial class OutpatientCard
{
    public int Id { get; set; }

    public string? CardNumber { get; set; }

    public int? PatientId { get; set; }

    public virtual Patient? Patient { get; set; }
}

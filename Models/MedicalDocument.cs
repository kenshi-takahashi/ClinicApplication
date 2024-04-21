using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class MedicalDocument
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? PatientId { get; set; }

    public virtual Patient? Patient { get; set; }
}

using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class DisabilitySheet
{
    public int Id { get; set; }

    public string? SheetNumber { get; set; }

    public DateOnly? IssueDate { get; set; }

    public int? DoctorId { get; set; }

    public virtual Doctor? Doctor { get; set; }
}

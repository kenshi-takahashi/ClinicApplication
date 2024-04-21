using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int? DoctorId { get; set; }

    public string? DayOfWeek { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual Doctor? Doctor { get; set; }
}

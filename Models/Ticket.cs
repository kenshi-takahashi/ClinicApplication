using System;
using System.Collections.Generic;

namespace Сlinic.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int? DoctorId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public TimeOnly? AppointmentTime { get; set; }

    public int? PatientId { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}

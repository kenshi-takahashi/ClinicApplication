using System;
using System.Collections.Generic;

namespace Сlinic.Models;

public partial class DoctorSpecialty
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}

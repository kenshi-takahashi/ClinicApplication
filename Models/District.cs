using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class District
{
    public int Id { get; set; }

    public int? DistrictNumber { get; set; }

    public int? DoctorId { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

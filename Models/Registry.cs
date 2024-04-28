using System;
using System.Collections.Generic;

namespace Сlinic.Models;

public partial class Registry
{
    public int Id { get; set; }

    public string? SubdivisionName { get; set; }

    public int? DepartmentNumber { get; set; }

    public string? Head { get; set; }

    public string? OrganizationType { get; set; }

    public string? Street { get; set; }

    public string? HouseNumber { get; set; }

    public string? City { get; set; }

    public int? RecordingMethodId { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual RecordingMethod? RecordingMethod { get; set; }

    public virtual ICollection<Registrar> Registrars { get; set; } = new List<Registrar>();
}

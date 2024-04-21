using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public int? SpecialtyId { get; set; }

    public int? RegistryId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<DisabilitySheet> DisabilitySheets { get; set; } = new List<DisabilitySheet>();

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual Registry? Registry { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual DoctorSpecialty? Specialty { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

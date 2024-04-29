using System;
using System.Collections.Generic;

namespace Clinic.Models;

public partial class Patient
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<MedicalDocument> MedicalDocuments { get; set; } = new List<MedicalDocument>();

    public virtual ICollection<OutpatientCard> OutpatientCards { get; set; } = new List<OutpatientCard>();

    public virtual ICollection<ReasonsForVisit> ReasonsForVisits { get; set; } = new List<ReasonsForVisit>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public string FullName
    {
        get
        {
            return (LastName ?? "") + " " + (FirstName ?? "") + " " + (MiddleName ?? "");
        }
    }
}

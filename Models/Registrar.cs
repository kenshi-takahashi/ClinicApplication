using System;
using System.Collections.Generic;

namespace Сlinic.Models;

public partial class Registrar
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public int? RegistryId { get; set; }

    public virtual Registry? Registry { get; set; }
}

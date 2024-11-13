using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class ErrorMa
{
    public uint Id { get; set; }

    public DateTime? DataErr { get; set; }

    public string? Recepte { get; set; }

    public int? SumEr { get; set; }

    public string? Comments { get; set; }
}

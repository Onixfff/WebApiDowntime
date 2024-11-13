using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Downtime
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public DateTime TimestampEnd { get; set; }

    public TimeOnly Difference { get; set; }

    public int IdIdle { get; set; }

    public string Comment { get; set; } = default!;

    public string? Recept { get; set; }

    public bool? isUpdate { get; set; }

    public virtual Recepttime? ReceptNavigation { get; set; }
}

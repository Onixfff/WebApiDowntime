using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Recepttime
{
    public string Name { get; set; } = null!;

    public TimeOnly Time { get; set; }

    public virtual ICollection<Downtime> Downtimes { get; set; } = new List<Downtime>();
}

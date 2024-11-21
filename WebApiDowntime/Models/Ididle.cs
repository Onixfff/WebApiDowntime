using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Ididle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Downtime> Downtimes { get; set; } = new List<Downtime>();
}

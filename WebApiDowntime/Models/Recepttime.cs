using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiDowntime.Models;

public partial class Recepttime
{
    public string Name { get; set; } = null!;

    public TimeOnly Time { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Downtime> Downtimes { get; set; } = new List<Downtime>();
}

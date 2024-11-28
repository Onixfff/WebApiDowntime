using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApiDowntime.Models;

public partial class Ididle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    [JsonIgnore]
    public virtual ICollection<Downtime> Downtimes { get; set; } = new List<Downtime>();
}

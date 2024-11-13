using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Verpackung
{
    public uint Dbid { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Data122 { get; set; }
}

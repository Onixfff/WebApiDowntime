using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Diagramm
{
    public uint Dbid { get; set; }

    public DateTime Timestamp { get; set; }

    public int? Data111 { get; set; }

    public int? Data112 { get; set; }

    public int? Data113 { get; set; }
}

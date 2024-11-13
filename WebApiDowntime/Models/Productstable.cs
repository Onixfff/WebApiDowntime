using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Productstable
{
    public uint Dbid { get; set; }

    public int Position { get; set; }

    public string? Product1 { get; set; }

    public string? Product2 { get; set; }

    public string? Product3 { get; set; }

    public double? Product4 { get; set; }

    public double? Product5 { get; set; }

    public int? Product6 { get; set; }
}

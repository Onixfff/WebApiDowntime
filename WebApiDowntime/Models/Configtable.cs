using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Configtable
{
    public int Dbid { get; set; }

    public int Position { get; set; }

    public string? Name { get; set; }

    public string? Group { get; set; }

    public int? OpcserverDbid { get; set; }

    public string? AccessPath { get; set; }

    public string? Opctag { get; set; }

    public int DataType { get; set; }

    public int Trigger { get; set; }

    public double Multiplier { get; set; }

    public string? EngUnit { get; set; }

    public bool IsSupplier { get; set; }

    public int PhysProperty { get; set; }

    public bool? Active { get; set; }

    public bool? Visible { get; set; }

    public int ProductColumnId { get; set; }

    public string? Remarks { get; set; }

    public string? LocalizedName { get; set; }
}

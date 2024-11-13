using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Triggertable
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

    public string? SourceTable { get; set; }

    public string? SourceColumn { get; set; }

    public bool? Active { get; set; }
}

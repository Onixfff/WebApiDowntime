using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Opcserverconfigtable
{
    public int Dbid { get; set; }

    public int Position { get; set; }

    public string? Name { get; set; }

    public string? RemoteMachineName { get; set; }

    public string? ServerProgId { get; set; }

    public string? ServerClassId { get; set; }

    public int OpcdataAccessType { get; set; }

    public int UpdateRate { get; set; }
}

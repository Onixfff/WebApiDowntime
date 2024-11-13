using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Productsconfigtable
{
    public int Dbid { get; set; }

    public int Position { get; set; }

    public string? Name { get; set; }

    public int DataType { get; set; }
}

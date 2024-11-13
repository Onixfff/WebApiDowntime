using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Product
{
    public uint Dbid { get; set; }

    public DateTime Timestamp { get; set; }
}

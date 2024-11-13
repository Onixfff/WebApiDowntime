using System;
using System.Collections.Generic;

namespace WebApiDowntime.Models;

public partial class Suppliertable
{
    public int Dbid { get; set; }

    public int Position { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Location { get; set; }

    public string? Zipcode { get; set; }

    public string? Remarks { get; set; }
}

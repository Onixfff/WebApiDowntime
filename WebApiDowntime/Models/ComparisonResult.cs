namespace WebApiDowntime.Models;

public partial class ComparisonResult
{
    public int Id { get; set; }

    public int Table1Id { get; set; }

    public int Table2Id { get; set; }

    public TimeOnly Difference { get; set; }

    public string Data52 { get; set; } = null!;

    public bool? Processed { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}

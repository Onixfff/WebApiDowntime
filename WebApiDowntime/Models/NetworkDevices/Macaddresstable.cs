namespace WebApiDowntime.Models.NetworkDevices;

public partial class Macaddresstable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string IpAdres { get; set; } = null!;

    public string MacAdres { get; set; } = null!;

    public string? Comment { get; set; }
}

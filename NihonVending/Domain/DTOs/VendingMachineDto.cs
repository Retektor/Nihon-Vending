namespace NihonVending.Domain.DTOs;

public class VendingMachineDto
{
    public long Id { get; set; }
    public virtual required string Name { get; set; }
    public virtual decimal Latitude { get; set; }
    public virtual decimal Longitude { get; set; }
    public virtual string? City { get; set; }
    public virtual required string Category { get; set; }
    public virtual required string Source { get; set; }
}


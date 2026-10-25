namespace NihonVending.Domain.DTOs;

public class VendingMachineCreateRequest
{
    public virtual required string Name { get; set; }
    public virtual decimal Latitude { get; set; }
    public virtual decimal Longitude { get; set; }
    public virtual string? City { get; set; }
    public virtual required string Category { get; set; }
    public virtual required string Source { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name is required.", nameof(Name));

        if (string.IsNullOrWhiteSpace(Category))
            throw new ArgumentException("Category is required.", nameof(Category));

        if (string.IsNullOrWhiteSpace(Source))
            throw new ArgumentException("Source is required.", nameof(Source));

        if (Latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(nameof(Latitude));

        if (Longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(nameof(Longitude));
    }
}

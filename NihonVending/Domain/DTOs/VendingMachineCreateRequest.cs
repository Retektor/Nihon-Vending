using NihonVending.Domain.Entities;

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

        var normalizedCategory = Category.Trim().ToLowerInvariant();

        if (!VendingMachineCategories.IsValid(Category))
        {
            var available = string.Join(", ", VendingMachineCategories.Allowed);
            throw new ArgumentException(
                $"Unknown category type. Available categories: {available}",
                nameof(Category));
        }

        Category = normalizedCategory;

        if (string.IsNullOrWhiteSpace(Source))
            throw new ArgumentException("Source is required.", nameof(Source));

        if (Latitude is < -90 or > 90)
            throw new ArgumentOutOfRangeException(
                nameof(Latitude),
                "Latitude must be between -90 and 90.");

        if (Longitude is < -180 or > 180)
            throw new ArgumentOutOfRangeException(
                nameof(Longitude),
                "Longitude must be between -180 and 180.");
    }
}

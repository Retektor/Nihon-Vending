namespace NihonVending.Domain.Entities;

public static class VendingMachineCategories
{
    public static readonly IReadOnlyList<string> Allowed =
    [
        "drinks",
        "snacks",
        "other"
    ];

    public static bool IsValid(string? category) =>
        !string.IsNullOrWhiteSpace(category) &&
        Allowed.Contains(category.Trim().ToLowerInvariant());
}

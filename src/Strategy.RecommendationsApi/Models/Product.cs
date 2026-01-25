namespace strategy.Models
{
    public sealed record Product(
        Guid Id,
        string Name,
        string Category,
        DateTime CreatedAt,
        int PopularityScore
    );
}

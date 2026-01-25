namespace strategy.Models
{
    public sealed record UserContext(
        Guid UserId,
        IReadOnlyList<string> Interests
    );
}

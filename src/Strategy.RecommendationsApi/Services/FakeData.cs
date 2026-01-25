using strategy.Models;

namespace strategy.Services
{
    public sealed class FakeData
    {
        public IReadOnlyList<Product> Products { get; } = new List<Product>
        {
            new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Red Sneakers", "Shoes", DateTime.UtcNow.AddDays(-3), 90),
            new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Trail Shoes", "Shoes", DateTime.UtcNow.AddDays(-10), 120),
            new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Blue Hoodie", "Clothing", DateTime.UtcNow.AddDays(-2), 70),
            new(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Running Shorts", "Clothing", DateTime.UtcNow.AddDays(-7), 55),
            new(Guid.Parse("55555555-5555-5555-5555-555555555555"), "Leather Wallet", "Accessories", DateTime.UtcNow.AddDays(-30), 40),
            new(Guid.Parse("66666666-6666-6666-6666-666666666666"), "Cap", "Accessories", DateTime.UtcNow.AddDays(-1), 65),
        };

    }
}

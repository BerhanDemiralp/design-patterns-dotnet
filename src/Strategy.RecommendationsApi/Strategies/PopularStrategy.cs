using strategy.Models;

namespace strategy.Strategies
{
    public sealed class PopularStrategy : IRecommendationStrategy
    {
        public string Key => "popular";

        public IReadOnlyList<Product> Recommend(
            UserContext user,
            IReadOnlyList<Product> products,
            int limit)
        {
            return products
                .OrderByDescending(p => p.PopularityScore)
                .ThenByDescending(p => p.CreatedAt)
                .Take(limit)
                .ToList();
        }
    }
}

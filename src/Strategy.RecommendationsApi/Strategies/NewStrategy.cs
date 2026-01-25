using strategy.Models;

namespace strategy.Strategies
{
    public sealed class NewStrategy : IRecommendationStrategy 
    {
        public string Key => "new";
        public IReadOnlyList<Product> Recommend(
            UserContext user,
            IReadOnlyList<Product> products,
            int limit)
        {
            return products
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.PopularityScore)
                .Take(limit)
                .ToList();
        }
    }
}

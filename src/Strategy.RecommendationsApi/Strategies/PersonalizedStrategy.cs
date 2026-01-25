using strategy.Models;

namespace strategy.Strategies
{
    public sealed class PersonalizedStrategy: IRecommendationStrategy
    {
        public string Key => "personalized";
        public IReadOnlyList<Product> Recommend(
            UserContext user,
            IReadOnlyList<Product> products,
            int limit)
        {
            return products.Select(p =>
            {
                var match = user.Interests.Any(i =>
                    p.Category.Equals(i, StringComparison.OrdinalIgnoreCase));

                var score = (match ? 1000 : 0) + p.PopularityScore;

                return (Product: p, Score: score);
            })
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.Product.CreatedAt)
            .Take(limit)
            .Select(x => x.Product)
            .ToList();
        }
    }
}

using strategy.Models;

namespace strategy.Strategies
{
    public sealed class CategoryStrategy : IRecommendationStrategy
    {
        public string Key => "category";

        public IReadOnlyList<Product> Recommend(UserContext user, IReadOnlyList<Product> products, int limit)
        {
            // If the user has no interests, fall back to "newest"
            if (user.Interests.Count == 0)
            {
                return products
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(limit)
                    .ToList();
            }

            // Use the first interest as the target category (simple but clear)
            var targetCategory = user.Interests[0];

            var primary = products
                .Where(p => p.Category.Equals(targetCategory, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(p => p.PopularityScore)
                .ThenByDescending(p => p.CreatedAt)
                .ToList();

            if (primary.Count >= limit)
                return primary.Take(limit).ToList();

            // Fill the rest with popular items from other categories
            var fallback = products
                .Except(primary)
                .OrderByDescending(p => p.PopularityScore)
                .ThenByDescending(p => p.CreatedAt)
                .Take(limit - primary.Count);

            return primary.Concat(fallback).ToList();
        }
    }

}

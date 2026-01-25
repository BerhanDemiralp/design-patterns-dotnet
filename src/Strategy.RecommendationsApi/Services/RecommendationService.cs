using strategy.Models;
using strategy.Strategies;

namespace strategy.Services
{
    public sealed class RecommendationService
    {
        private readonly FakeData _data;
        private readonly Dictionary<string, IRecommendationStrategy> _map;

        public RecommendationService(FakeData data, IEnumerable<IRecommendationStrategy> strategies)
        {
            _data = data;
            _map = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyList<string> AvailableTypes => _map.Keys.OrderBy(x => x).ToList();

        public IReadOnlyList<Product> Recommend(string? type, int limit, UserContext user)
        {
            if (limit <= 0) throw new ArgumentException("Limit must be greater than 0.");
            if (limit > 50) throw new ArgumentException($"Limit cannot be greater than 50.");

            var key = string.IsNullOrWhiteSpace(type) ? "popular" : type.Trim();

            if (!_map.TryGetValue(key, out var strategy))
                throw new ArgumentException($"Unknown recommendation type: '{key}'. Valid types: {string.Join(", ", AvailableTypes)}");

            return strategy.Recommend(user, _data.Products, limit);
        }
    }
}

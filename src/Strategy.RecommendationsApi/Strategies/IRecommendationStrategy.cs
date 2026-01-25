using strategy.Models;

namespace strategy.Strategies
{
    public interface IRecommendationStrategy
    {
        // query param ile seçilecek anahtar: popular / new / personalized
        string Key { get; }

        IReadOnlyList<Product> Recommend(
            UserContext user,
            IReadOnlyList<Product> products,
            int limit);
    }
}

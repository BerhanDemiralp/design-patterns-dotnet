using Factory.PaymentsApi.Providers;

namespace Factory.PaymentsApi.Factory
{
    public sealed class PaymentProviderFactory
    {
        private readonly Dictionary<string, IPaymentProvider> _map;

        public PaymentProviderFactory(IEnumerable<IPaymentProvider> providers)
        {
            _map = providers.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
        }

        public IPaymentProvider Create(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
                throw new ArgumentException("Provider is required.");

            if (_map.TryGetValue(providerName.Trim(), out var provider))
                return provider;

            throw new ArgumentException(
                $"Unknown provider: '{providerName}'. Valid: {string.Join(", ", _map.Keys)}"
            );
        }

        public IReadOnlyList<string> AvailableProviders =>
            _map.Keys.OrderBy(x => x).ToList();
    }
}

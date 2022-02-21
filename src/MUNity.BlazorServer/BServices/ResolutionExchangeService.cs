using MUNity.Database.Models.Resolution;
using MUNity.Services;

namespace MUNity.BlazorServer.BServices
{
    public class ResolutionExchangeService : IDisposable
    {
        private List<ResolutionExchange> _resolutions;

        private readonly IServiceScopeFactory _scopeFactory;

        public event EventHandler<ResolutionExchangeUpdatedEventArgs> ResolutionExchangeUpdated;

        public ResolutionExchange CreateResolution(string committeeId)
        {
            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            var resolution = service.CreateResolutionForCommittee(committeeId);
            var exchange = new ResolutionExchange(_scopeFactory)
            {
                Resolution = resolution,
            };
            _resolutions.Add(exchange);
            var args = new ResolutionExchangeUpdatedEventArgs()
            {
                CommitteeId = committeeId,
                ResolutionId = resolution.ResaElementId,
                Topic = resolution.Topic,
            };
            ResolutionExchangeUpdated?.Invoke(this, args);
            return exchange;
        }

        public ResolutionExchange GetResolution(string resolutionId)
        {
            var exchange = _resolutions.FirstOrDefault(n => n.Resolution.ResaElementId == resolutionId);
            if (exchange == null)
            {
                using var scope = _scopeFactory.CreateScope();
                using var service = scope.ServiceProvider.GetRequiredService<ResolutionService>();

                exchange = new ResolutionExchange(_scopeFactory);
                exchange.Resolution = service.GetResolution(resolutionId);
                if (exchange.Resolution == null)
                {
                    throw new ArgumentNullException($"Resolution with id {resolutionId} not found!");
                }
                _resolutions.Add(exchange);
            }
            return exchange;
        }

        public void Dispose()
        {
            
        }

        public ResolutionExchangeService(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
            this._resolutions = new List<ResolutionExchange>();
        }
    }

    public class ResolutionExchangeUpdatedEventArgs : EventArgs
    {
        public string ResolutionId { get; set; }

        public string Topic { get; set; }

        public string CommitteeId { get; set; }
    }
}

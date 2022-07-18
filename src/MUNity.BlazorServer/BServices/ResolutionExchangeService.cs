using MUNity.Database.Models.Resolution;
using MUNity.Services;

namespace MUNity.BlazorServer.BServices
{
    public class ResolutionExchangeService : IDisposable
    {
        private List<ResolutionExchange> _resolutions;

        private readonly IServiceScopeFactory _scopeFactory;

        public event EventHandler<ResolutionExchangeUpdatedEventArgs> ResolutionExchangeUpdated;

        public ResolutionExchange CreateResolution(string committeeId, int? roleId = null)
        {
            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResolutionExchangeService>>();
            var resolution = service.CreateResolutionForCommittee(committeeId, roleId);
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
                exchange = new ResolutionExchange(_scopeFactory);
                _resolutions.Add(exchange);
            }

            if (exchange.Resolution == null)
            {
                LoadResolutionForExchange(exchange, resolutionId);
            }
            return exchange;
        }

        private void LoadResolutionForExchange(ResolutionExchange exchange, string resolutionId)
        {
            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResolutionExchangeService>>();
            var resolution = service.GetResolution(resolutionId);
            if (resolution == null)
            {
                logger?.LogInformation("No Resolution with id {0} was found", resolutionId);
            }
            else
            {
                exchange.Resolution = resolution;
                _resolutions.Add(exchange);
            }
        }

        public void RemoveResolution(ResolutionExchange exchange)
        {
            this._resolutions.Add(exchange);
            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<ResolutionService>();
            service.RemoveResolution(exchange.Resolution);
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

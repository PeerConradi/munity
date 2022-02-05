namespace MUNity.BlazorServer.BServices
{
    public class VirtualCommiteeParticipationService : IDisposable
    {
        private ILogger<VirtualCommiteeParticipationService> _logger;
        private VirtualCommitteeExchangeService _committeeExchangeService;

        private VirtualCommitteeExchange _exchange;
        // Virtual Committee ViewModel

        private string _committeeId;
        private string _secret;
        private int _roleId;

        // Role
        public void Dispose()
        {
            SignOff();
        }

        private void SignOff()
        {
            if (_exchange?.connectedRoles.TryRemove(_roleId, out var t) == true)
            {
                _exchange.NotifyUserDisconnected();
            }
        }

        public void SignIn(string committeeId, string secret, int myRoleId)
        {
            SignOff();

            this._committeeId = committeeId;
            this._secret = secret;
            this._roleId = myRoleId;
            this._exchange = _committeeExchangeService.GetExchange(committeeId);
            if (_exchange != null)
            {
                _exchange.connectedRoles.TryAdd(myRoleId, true);
                _exchange.NotifyUserConnected();
            }
        }

        public VirtualCommiteeParticipationService(ILogger<VirtualCommiteeParticipationService> logger, VirtualCommitteeExchangeService exchangeService)
        {
            this._logger = logger;
            this._committeeExchangeService = exchangeService;
        }
    }
}

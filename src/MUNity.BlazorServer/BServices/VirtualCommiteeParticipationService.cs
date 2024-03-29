﻿using Microsoft.EntityFrameworkCore;
using MUNity.Database.Context;
using MUNity.Database.Models.Session;
using MUNity.VirtualCommittees.Dtos;

namespace MUNity.BlazorServer.BServices
{

    /// <summary>
    /// This service handles the current state of the user that is inside a virtual committee.
    /// Every user can only be inside one virtual committee.
    /// </summary>
    public class VirtualCommiteeParticipationService : IDisposable
    {
        private ILogger<VirtualCommiteeParticipationService> _logger;
        private VirtualCommitteeExchangeService _committeeExchangeService;
        private MunityContext _dbContext;

        private VirtualCommitteeExchange _exchange;
        // Virtual Committee ViewModel

        private string _committeeId;
        private string _secret;
        private int? _roleId;

        public int? RoleId => _roleId;

        private string _roleName;
        public string RoleName => _roleName;

        private string _roleIso;
        public string RoleIso => _roleIso;

        public string DelegateType { get; private set; }

        public bool IsTeamMember { get; set; }

        /// <summary>
        /// Callback event when the user is done with SignIn into the conference
        /// </summary>
        public event EventHandler Registered;

        public event EventHandler<string> EditingPreambleParagraphChanged;
        public event EventHandler<string> EditingOperativeParagraphChanged;

        private string lastEditedPreambleParagraphId = null;
        public string LastEditedPreambleParagraphId 
        {
            get => lastEditedPreambleParagraphId;
            set
            {
                if (lastEditedPreambleParagraphId != value)
                {
                    lastEditedPreambleParagraphId = value;
                    EditingPreambleParagraphChanged?.Invoke(this, value);
                }
            }
        }

        private string lastEditedOperativeParagraphId = null;
        public string LastEditedOperativeParagraphId
        {
            get => lastEditedOperativeParagraphId;
            set
            {
                if (lastEditedOperativeParagraphId != value)
                {
                    lastEditedOperativeParagraphId = value;
                    EditingOperativeParagraphChanged?.Invoke(this, value);
                }
            }
        }

        public bool IsActiveForCommittee(string committeeId)
        {
            return _committeeId == committeeId;
        }

        // Role
        public void Dispose()
        {
            SignOff();
        }

        public void SignOff()
        {
            if (_exchange == null)
                return;

            if (_roleId != null && _exchange.connectedRoles.TryRemove(_roleId.Value, out var t) == true)
            {
                _exchange.NotifyUserDisconnected();
            }
            this._roleId = null;
            this._roleName = string.Empty;
            this._roleIso = string.Empty;
        }

        public bool SignIn(string committeeId, string secret)
        {
            SignOff();
            var roleInfo = this._dbContext.Delegates
                .Where(n => n.RoleSecret == secret)
                .AsNoTracking()
                .Select(n => new
                {
                    n.RoleId,
                    n.RoleName,
                    n.DelegateCountry.Iso,
                    n.RoleType,
                    n.DelegateType
                })
                .FirstOrDefault();

            if (roleInfo == null)
                return false;

            this._committeeId = committeeId;
            this._secret = secret;
            this._roleId = roleInfo?.RoleId;
            this._roleName = roleInfo?.RoleName;
            this._roleIso = roleInfo?.Iso ?? "un";
            this.DelegateType = roleInfo?.DelegateType;
            this._exchange = _committeeExchangeService.GetExchange(committeeId);
            if (_exchange != null && this._roleId != null)
            {
                _exchange.connectedRoles.TryAdd(this._roleId.Value, true);
                _exchange.NotifyUserConnected();
            }
            this.Registered?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public VirtualCommiteeParticipationService(ILogger<VirtualCommiteeParticipationService> logger, 
            VirtualCommitteeExchangeService exchangeService,
            MunityContext dbContext)
        {
            this._logger = logger;
            this._committeeExchangeService = exchangeService;
            this._dbContext = dbContext;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework
{
    public class MunityContext : DbContext
    {
        public DbSet<Models.Admin> Admins { get; set; }

        public DbSet<Models.AuthKey> AuthKey { get; set; }

        public DbSet<Models.Committee> Committees { get; set; }

        public DbSet<Models.CommitteeDelegation> CommitteeDelegations { get; set; }

        public DbSet<Models.CommitteeLeader> CommitteeLeader { get; set; }

        public DbSet<Models.CommitteeStatus> CommitteeStatuses { get; set; }

        public DbSet<Models.Conference> Conferences { get; set; }

        public DbSet<Models.ConferenceUserAuth> ConferenceUserAuths { get; set; }

        public DbSet<Models.Delegation> Delegations { get; set; }

        public DbSet<Models.DelegationUser> DelegationUsers { get; set; }

        public DbSet<Models.Gallery> Galleries { get; set; }

        public DbSet<Models.MediaImage> MediaImages { get; set; }

        public DbSet<Models.Resolution> Resolutions { get; set; }

        public DbSet<Models.ResolutionConference> ConferenceResolutions { get; set; }

        public DbSet<Models.ResolutionUser> ResolutionUsers { get; set; }

        public DbSet<Models.TeamRole> TeamRoles { get; set; }

        public DbSet<Models.TeamUser> TeamUsers { get; set; }

        public DbSet<Models.User> Users { get; set; }

        public DbSet<Models.UserAuths> UserAuths { get; set; }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }
    }
}

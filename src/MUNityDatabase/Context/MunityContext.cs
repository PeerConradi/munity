using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNityCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MUNity.Database.General;
using MUNity.Database.Models.User;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.Simulation;
using MUNity.Database.Models.LoS;
using MUNity.Database.Models;
using MUNity.Database.Models.Resolution;
using MUNity.Database.Models.Session;
using MUNity.Database.Interfaces;
using MUNity.Database.Model.Conference;
using MUNity.Database.Model.User;
using MUNity.Database.Model.Website;

namespace MUNity.Database.Context
{
    public class MunityContext 
        : IdentityDbContext<MunityUser, 
            MunityRole, 
            string, 
            IdentityUserClaim<string>, 
            MunityUserRole, 
            IdentityUserLogin<string>, 
            IdentityRoleClaim<string>, 
            IdentityUserToken<string>>
    {

        public DbSet<Country> Countries { get; set; }

        public DbSet<CountryNameTranslation> CountryNameTranslations { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<OrganizationRole> OrganizationRoles { get; set; }

        public DbSet<OrganizationMember> OrganizationMembers { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Committee> Committees { get; set; }

        public DbSet<Delegation> Delegations { get; set; }

        public DbSet<ConferenceDelegateRole> Delegates { get; set; }

        public DbSet<ConferenceTeamRole> ConferenceTeamRoles { get; set; }

        public DbSet<ConferenceSecretaryGeneralRole> SecretaryGenerals { get; set; }

        public DbSet<Conference> Conferences { get; set; }

        public DbSet<ConferenceParticipationCostRule> ConferenceParticipationCostRules { get; set; }

        public DbSet<CommitteeTopic> CommitteeTopics { get; set; }

        public DbSet<AttendanceState> AttendanceStates { get; set; }

        public DbSet<AttendanceCheck> AttendanceChecks { get; set; }

        public DbSet<CommitteeSession> CommitteeSessions { get; set; }

        public DbSet<Participation> Participations { get; set; }

        public DbSet<ConferenceRoleAuth> ConferenceRoleAuthorizations { get; set; }

        public DbSet<RoleApplication> RoleApplications { get; set; }

        public DbSet<DelegationApplication> DelegationApplications { get; set; }

        public DbSet<DelegationApplicationUserEntry> DelegationApplicationUserEntries { get; set; }

        public DbSet<DelegationApplicationPickedDelegation> DelegationApplicationPickedDelegations { get; set; }

        public DbSet<TeamRoleGroup> TeamRoleGroups { get; set; }

        public DbSet<ResolutionAuth> ResolutionAuths { get; set; }

        public DbSet<ResolutionUser> ResolutionUsers { get; set; }

        //public DbSet<Simulation> Simulations { get; set; }

        //public DbSet<SimulationRole> SimulationRoles { get; set; }

        //public DbSet<SimulationUser> SimulationUser { get; set; }

        //public DbSet<SimulationVoting> SimulationVotings { get; set; }

        //public DbSet<SimulationVotingSlot> VotingSlots { get; set; }

        //public DbSet<SimulationLog> SimulationLog { get; set; }

        //public DbSet<SimulationInvite> SimulationInvites { get; set; }

        //public DbSet<AgendaItem> AgendaItems { get; set; }

        //public DbSet<PetitionType> PetitionTypes { get; set; }

        //public DbSet<Petition> Petitions { get; set; }

        //public DbSet<PetitionTypeSimulation> SimulationPetitionTypes { get; set; }

        //public DbSet<SimulationStatus> SimulationStatuses { get; set; }

        //public DbSet<SimulationPresents> PresentChecks { get; set; }

        //public DbSet<PresentsState> PresentStates { get; set; }

        public DbSet<ListOfSpeakers> ListOfSpeakers { get; set; }

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<ListOfSpeakersLog> ListOfSpeakersLogs { get; set; }

        public DbSet<MunitySetting> Settings { get; set; }

        public DbSet<ResaElement> Resolutions { get; set; }

        public DbSet<ResaPreambleParagraph> PreambleParagraphs { get; set; }

        public DbSet<ResaOperativeParagraph> OperativeParagraphs { get; set; }

        public DbSet<ResaSupporter> ResolutionSupporters { get; set; }

        public DbSet<ResaDeleteAmendment> ResolutionDeleteAmendments { get; set; }
        public DbSet<ResaChangeAmendment> ResolutionChangeAmendments { get; set; }
        public DbSet<ResaMoveAmendment> ResolutionMoveAmendments { get; set; }
        public DbSet<ResaAddAmendment> ResolutionAddAmendments { get; set; }

        public DbSet<ConferenceWebPage> ConferenceWebPages { get; set; }

        public DbSet<ConferenceApplicationPage> ConferenceApplicationPages { get; set; }

        public DbSet<ConferencePageColorScheme> ConferencePageColorSchemes { get; set; }


        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<UserNotificationCategory> UserNotificationCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasMany(n => n.Translations)
                .WithOne(n => n.Country)
                .OnDelete(DeleteBehavior.Cascade);

           

            modelBuilder.Entity<CountryNameTranslation>()
                .HasKey(n => new {n.CountryId, n.LanguageCode});

            modelBuilder.Entity<MunityUser>().HasKey(n => n.Id);

            modelBuilder.Entity<MunityUser>().HasMany(n => n.CreatedResolutions).WithOne(a => a.CreationUser).IsRequired(false);

            // Each User can have many entries in the UserRole join table
            modelBuilder.Entity<MunityUser>().HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();



            modelBuilder.Entity<OrganizationRole>()
                .HasOne(n => n.Organization)
                .WithMany(a => a.Roles);

            modelBuilder.Entity<OrganizationMember>()
                .HasOne(n => n.Organization)
                .WithMany(n => n.Member);

            modelBuilder.Entity<OrganizationRole>()
                .HasMany(n => n.MembersWithRole)
                .WithOne(n => n.Role);

            modelBuilder.Entity<Project>()
                .HasOne(n => n.ProjectOrganization)
                .WithMany(n => n.Projects)
                .IsRequired();


            // Conference
            modelBuilder.Entity<Conference>().HasOne(n => n.ConferenceProject)
                .WithMany(n => n.Conferences);

            modelBuilder.Entity<AbstractConferenceRole>().HasOne(n => n.Conference).WithMany(n => n.Roles);

            modelBuilder.Entity<AbstractConferenceRole>().HasDiscriminator(n => n.RoleType)
                .HasValue<ConferenceDelegateRole>("DelegateRole")
                .HasValue<ConferenceSecretaryGeneralRole>("SecretaryGeneralRole")
                .HasValue<ConferenceTeamRole>("TeamRole")
                .HasValue<ConferenceVisitorRole>("VisitorRole");

            modelBuilder.Entity<AbstractConferenceRole>().HasMany(n => n.Participations)
                .WithOne(n => n.Role);

            modelBuilder.Entity<ConferenceTeamRole>().HasOne(n => n.TeamRoleGroup).WithMany(n => n.TeamRoles);


            modelBuilder.Entity<Committee>().HasMany(n => n.Sessions).WithOne(n =>
                n.Committee);

            modelBuilder.Entity<Committee>().HasOne(n => n.Conference)
                .WithMany(a => a.Committees).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Committee>().HasMany(n => n.Resolutions)
                .WithOne(a => a.Committee).IsRequired(false);

            modelBuilder.Entity<Committee>().HasOne(n => n.ResolutlyCommittee)
                .WithMany(n => n.ChildCommittees).IsRequired(false);

            modelBuilder.Entity<Delegation>()
                .HasOne(n => n.ParentDelegation)
                .WithMany(a => a.ChildDelegations)
                .OnDelete(DeleteBehavior.SetNull);

            
            //modelBuilder.Entity<SimulationUser>().HasOne(n => n.Simulation).WithMany(n =>
            //    n.Users);

            modelBuilder.Entity<ListOfSpeakers>().HasMany(n => n.AllSpeakers).WithOne(n => n.ListOfSpeakers);

            modelBuilder.Entity<Speaker>().HasKey(n => n.Id);

            // Resolution
            modelBuilder.Entity<ResolutionAuth>()
                .HasMany(n => n.Users).WithOne(n => n.Auth);


            modelBuilder.Entity<ResolutionAuth>()
                .HasOne(n => n.Simulation)
                .WithMany(n => n.Resolutions);

            modelBuilder.Entity<ResaElement>()
                .HasMany(n => n.PreambleParagraphs)
                .WithOne(n => n.ResaElement)
                .IsRequired();

            modelBuilder.Entity<ResaElement>()
                .HasMany(n => n.OperativeParagraphs)
                .WithOne(n => n.Resolution)
                .IsRequired();

            modelBuilder.Entity<ResaElement>()
                .HasOne(n => n.Authorization)
                .WithOne(n => n.Resolution)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResaOperativeParagraph>()
                .HasOne(n => n.Parent)
                .WithMany(n => n.Children)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResaOperativeParagraph>()
                .HasMany(n => n.DeleteAmendments)
                .WithOne(n => n.TargetParagraph)
                .IsRequired();

            modelBuilder.Entity<ResaOperativeParagraph>()
                .HasMany(n => n.ChangeAmendments)
                .WithOne(n => n.TargetParagraph)
                .IsRequired();

            modelBuilder.Entity<ResaOperativeParagraph>()
                .HasMany(n => n.MoveAmendments)
                .WithOne(n => n.SourceParagraph)
                .IsRequired();

            modelBuilder.Entity<ResaElement>()
                .HasMany(n => n.AddAmendments)
                .WithOne(n => n.Resolution);

            // Simulations
            modelBuilder.Entity<SimulationRole>().HasOne(n => n.Simulation).WithMany(n =>
                n.Roles);

            modelBuilder.Entity<Simulation>().HasMany(n => n.Users).WithOne(n => n.Simulation);


            modelBuilder.Entity<SimulationPresents>()
                .HasMany(n => n.CheckedUsers)
                .WithOne(n => n.SimulationPresents);

            modelBuilder.Entity<Simulation>().HasMany(n => n.PresentChecks).WithOne(n => n.Simulation);
        }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is IIsDeleted entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag - only this will get sent to the Db
                    entity.IsDeleted = true;
                }
            }

            HandleEasyId();

            
            return base.SaveChanges();
        }

        private void HandleEasyId()
        {
            var markedAsNew = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach (var entityEntry in markedAsNew)
            {
                switch (entityEntry.Entity)
                {
                    case Organization organization:
                        HandleEasyIdOrganization(organization);
                        break;
                    case Project project:
                        HandleEasyIdProject(project);
                        break;
                    case Conference conference:
                        HandleEasyIdConference(conference);
                        break;
                    case Committee committee:
                        HandleEasyIdCommittee(committee);
                        break;
                    case Delegation delegation:
                        HandleEasyDelegationId(delegation);
                        break;
                }
            }
        }

        

        private void HandleEasyIdOrganization(Organization organization)
        {
            if (!string.IsNullOrWhiteSpace(organization.OrganizationId)) return;

            if (string.IsNullOrWhiteSpace(organization.OrganizationShort))
            {
                organization.OrganizationId = Guid.NewGuid().ToString();
                return;
            }

            var easyId = Util.IdGenerator.AsPrimaryKey(organization.OrganizationShort);
            if (string.IsNullOrWhiteSpace(easyId)) return;

            if (Organizations.All(n => n.OrganizationId != easyId))
                organization.OrganizationId = easyId;
            else
                organization.OrganizationId = Guid.NewGuid().ToString();
            
        }

        private void HandleEasyIdProject(Project project)
        {
            if (!string.IsNullOrWhiteSpace(project.ProjectId)) return;

            if (string.IsNullOrWhiteSpace(project.ProjectShort))
            {
                project.ProjectId = Guid.NewGuid().ToString();
                return;
            }

            var easyId = Util.IdGenerator.AsPrimaryKey(project.ProjectShort);
            if (string.IsNullOrWhiteSpace(easyId)) return;

            if (Projects.All(n => n.ProjectId != easyId))
                project.ProjectId = easyId;
            else
                project.ProjectId = Guid.NewGuid().ToString();
        }

        private void HandleEasyIdConference(Conference conference)
        {
            if (!string.IsNullOrWhiteSpace(conference.ConferenceId)) return;

            if (string.IsNullOrWhiteSpace(conference.ConferenceShort))
            {
                conference.ConferenceId = Guid.NewGuid().ToString();
                return;
            }

            var easyId = Util.IdGenerator.AsPrimaryKey(conference.ConferenceShort);
            if (string.IsNullOrWhiteSpace(easyId)) return;

            if (Conferences.All(n => n.ConferenceId != easyId))
                conference.ConferenceId = easyId;
            else
                conference.ConferenceId = Guid.NewGuid().ToString();
        }

        private void HandleEasyIdCommittee(Committee committee)
        {
            if (!string.IsNullOrWhiteSpace(committee.CommitteeId)) return;

            if (committee.Conference == null || string.IsNullOrEmpty(committee.CommitteeShort))
            {
                committee.CommitteeId = Guid.NewGuid().ToString();
                return;
            }

            var committeeEasy = Util.IdGenerator.AsPrimaryKey(committee.CommitteeShort);
            if (string.IsNullOrWhiteSpace(committeeEasy)) return;

            var easyCommitteeId = committee.Conference.ConferenceId + "-" +
                                  committeeEasy;

            if (this.Committees.All(n => n.CommitteeId != easyCommitteeId))
                committee.CommitteeId = easyCommitteeId;
            else
                committee.CommitteeId = Guid.NewGuid().ToString();
        }

        private void HandleEasyDelegationId(Delegation delegation)
        {
            if (!string.IsNullOrWhiteSpace(delegation.DelegationId)) return;

            if (delegation.Conference == null || string.IsNullOrWhiteSpace(delegation.Name))
            {
                delegation.DelegationId = Guid.NewGuid().ToString();
                return;
            }

            var easyNameDelegation = Util.IdGenerator.AsPrimaryKey(delegation.Name);
            var easyId = delegation.Conference.ConferenceId + "-" + easyNameDelegation;
            if (this.Delegations.All(n => n.DelegationId != easyId))
                delegation.DelegationId = easyId;
            else
                delegation.DelegationId = Guid.NewGuid().ToString();
            

        }

        /// <summary>
        /// Saves the Database Changes but ignores all elements that are protected by soft-deletion.
        /// This will completely remove these elements
        /// </summary>
        /// <returns></returns>
        public int SaveChangesWithoutSoftDelete()
        {
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves the Database Changes but ignores all elements that are protected by soft-deletion.
        /// This will completely remove these elements
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesWithoutSoftDeleteAsync()
        {
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();
            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is IIsDeleted entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag - only this will get sent to the Db
                    entity.IsDeleted = true;
                }
            }

            HandleEasyId();

            return base.SaveChangesAsync(cancellationToken);
        }

        public static MunityContext FromSqlLite(string databaseName)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MunityContext>();
            optionsBuilder.UseSqlite($"Data Source={databaseName}.db");
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.LogTo(Console.WriteLine);
            var context = new MunityContext(optionsBuilder.Options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}

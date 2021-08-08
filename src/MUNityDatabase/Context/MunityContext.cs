using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNityCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MUNity.Database.Models.User;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Organization;
using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.Resolution.V2;
using MUNity.Database.Models.Simulation;
using MUNity.Database.Models.LoS;
using MUNity.Database.Models;
using MUNity.Database.Models.Resolution.SqlResa;

namespace MUNity.Database.Context
{
    public class MunityContext : IdentityDbContext<MunityUser, MunityRole, string>
    {

        public DbSet<Country> Countries { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<OrganizationRole> OrganizationRoles { get; set; }

        public DbSet<OrganizationMember> OrganizationMember { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Committee> Committees { get; set; }

        public DbSet<Delegation> Delegation { get; set; }

        public DbSet<ConferenceVisitorRole> Visitors { get; set; }

        public DbSet<ConferenceDelegateRole> Delegates { get; set; }

        public DbSet<ConferenceTeamRole> TeamRoles { get; set; }

        public DbSet<ConferenceNgoRole> NgoRoles { get; set; }

        public DbSet<ConferencePressRole> PressRoles { get; set; }

        public DbSet<ConferenceSecretaryGeneralRole> SecretaryGenerals { get; set; }

        public DbSet<Conference> Conferences { get; set; }

        public DbSet<Participation> Participations { get; set; }

        public DbSet<RoleAuth> RoleAuths { get; set; }

        public DbSet<MunityUserAuth> UserAuths { get; set; }

        public DbSet<RoleApplication> RoleApplications { get; set; }

        public DbSet<GroupApplication> GroupApplications { get; set; }

        public DbSet<GroupedRoleApplication> GroupedRoleApplications { get; set; }

        public DbSet<TeamRoleGroup> TeamRoleGroups { get; set; }

        public DbSet<ResolutionAuth> ResolutionAuths { get; set; }

        public DbSet<ResolutionUser> ResolutionUsers { get; set; }

        public DbSet<Simulation> Simulations { get; set; }

        public DbSet<SimulationRole> SimulationRoles { get; set; }

        public DbSet<SimulationUser> SimulationUser { get; set; }

        public DbSet<ListOfSpeakers> ListOfSpeakers { get; set; }

        public DbSet<AgendaItem> AgendaItems { get; set; }

        public DbSet<PetitionType> PetitionTypes { get; set; }

        public DbSet<Petition> Petitions { get; set; }

        public DbSet<PetitionTypeSimulation> SimulationPetitionTypes { get; set; }

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<MunitySetting> Settings { get; set; }

        public DbSet<SimulationStatus> SimulationStatuses { get; set; }

        public DbSet<ResaElement> Resolutions { get; set; }

        public DbSet<ResaPreambleParagraph> PreambleParagraphs { get; set; }

        public DbSet<ResaOperativeParagraph> OperativeParagraphs { get; set; }

        public DbSet<ResaSupporter> ResolutionSupporters { get; set; }

        public DbSet<ResaAmendment> Amendments { get; set; }

        public DbSet<ResaDeleteAmendment> DeleteAmendments { get; set; }
        public DbSet<ResaChangeAmendment> ChangeAmendments { get; set; }
        public DbSet<ResaMoveAmendment> MoveAmendments { get; set; }
        public DbSet<ResaAddAmendment> AddAmendments { get; set; }

        public DbSet<SimulationVoting> SimulationVotings { get; set; }

        public DbSet<SimulationVotingSlot> VotingSlots { get; set; }

        public DbSet<SimulationLog> SimulationLog { get; set; }

        public DbSet<SimulationInvite> SimulationInvites { get; set; }

        public DbSet<SimulationPresents> PresentChecks { get; set; }

        public DbSet<PresentsState> PresentStates { get; set; }

        public DbSet<ListOfSpeakersLog> ListOfSpeakersLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MunityUser>().HasKey(n => n.Id);

            //modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(n => n.UserId);

            //modelBuilder.Entity<IdentityUserRole<string>>().HasKey(n => n.UserId);

            //modelBuilder.Entity<IdentityUserToken<string>>().HasKey(n => n.UserId);

            modelBuilder.Entity<Committee>().HasOne(n => n.Conference)
                .WithMany(a => a.Committees).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Committee>().HasMany(n => n.Resolutions)
                .WithOne(a => a.Committee).IsRequired(false);

            modelBuilder.Entity<OrganizationRole>().HasOne(n => n.Organization)
                .WithMany(a => a.Roles);

            modelBuilder.Entity<OrganizationMember>().HasOne(n => n.Organization)
                .WithMany(n => n.Member);

            modelBuilder.Entity<OrganizationRole>().HasMany(n => n.MembersWithRole).WithOne(n => n.Role);

            modelBuilder.Entity<Project>().HasOne(n => n.ProjectOrganization)
                .WithMany(n => n.Projects);

            modelBuilder.Entity<Conference>().HasOne(n => n.ConferenceProject)
                .WithMany(n => n.Conferences);

            modelBuilder.Entity<AbstractConferenceRole>().HasOne(n => n.Conference).WithMany(n => n.Roles);

            modelBuilder.Entity<ConferenceTeamRole>().HasOne(n => n.TeamRoleGroup).WithMany(n => n.TeamRoles);

            modelBuilder.Entity<MunityUser>().HasOne(n => n.Auth).WithMany(n => n.Users);

            modelBuilder.Entity<MunityUser>().HasOne(n => n.PrivacySettings).WithOne(n => n.User).HasForeignKey<UserPrivacySettings>(n => n.UserRef);

            modelBuilder.Entity<MunityUser>().HasMany(n => n.CreatedResolutions).WithOne(a => a.CreationUser).IsRequired(false);

            modelBuilder.Entity<AbstractConferenceRole>().HasDiscriminator(n => n.RoleType)
                .HasValue<ConferenceDelegateRole>("DelegateRole")
                .HasValue<ConferenceNgoRole>("NgoRole")
                .HasValue<ConferencePressRole>("PressRole")
                .HasValue<ConferenceSecretaryGeneralRole>("SecretaryGeneralRole")
                .HasValue<ConferenceTeamRole>("TeamRole")
                .HasValue<ConferenceVisitorRole>("VisitorRole");


            modelBuilder.Entity<Committee>().HasMany(n => n.Sessions).WithOne(n =>
                n.Committee);



            // The ResolutionId is also the Primary Key.
            modelBuilder.Entity<ResolutionAuth>().HasKey(n => n.ResolutionId);

            //modelBuilder.Entity<ResolutionUser>().HasKey(n => new { n.Auth.ResolutionId, n.CoreUserId });

            modelBuilder.Entity<ResolutionAuth>()
                .HasMany(n => n.Users).WithOne(n => n.Auth);


            modelBuilder.Entity<ResolutionAuth>()
                .HasOne(n => n.Simulation)
                .WithMany(n => n.Resolutions);

            modelBuilder.Entity<SimulationRole>().HasOne(n => n.Simulation).WithMany(n =>
                n.Roles);

            modelBuilder.Entity<Simulation>().HasMany(n => n.Users).WithOne(n => n.Simulation);

            //modelBuilder.Entity<SimulationUser>().HasOne(n => n.Simulation).WithMany(n =>
            //    n.Users);

            modelBuilder.Entity<ListOfSpeakers>().HasMany(n => n.AllSpeakers).WithOne(n => n.ListOfSpeakers);

            modelBuilder.Entity<Speaker>().HasKey(n => n.Id);

            modelBuilder.Entity<ResaElement>().HasMany(n => n.PreambleParagraphs).WithOne(n => n.ResaElement);

            modelBuilder.Entity<ResaElement>().HasMany(n => n.OperativeParagraphs).WithOne(n => n.Resolution);

            modelBuilder.Entity<ResaOperativeParagraph>().HasOne(n => n.Parent).WithMany(n => n.Children);

            modelBuilder.Entity<ResaOperativeParagraph>().HasMany(n => n.DeleteAmendments).WithOne(n => n.TargetParagraph);

            modelBuilder.Entity<ResaOperativeParagraph>().HasMany(n => n.MoveAmendments).WithOne(n => n.SourceParagraph);

            modelBuilder.Entity<ResaElement>().HasMany(n => n.Amendments).WithOne(n => n.Resolution);

            modelBuilder.Entity<SimulationPresents>().HasMany(n => n.CheckedUsers).WithOne(n => n.SimulationPresents);

            modelBuilder.Entity<Simulation>().HasMany(n => n.PresentChecks).WithOne(n => n.Simulation);
        }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }
    }
}

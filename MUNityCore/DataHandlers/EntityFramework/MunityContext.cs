using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.Organization;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Models.Simulation;
using MUNityCore.Models.User;
using MUNityCore.Models;
using MUNity.Models.ListOfSpeakers;

namespace MUNityCore.DataHandlers.EntityFramework
{
    public class MunityContext : DbContext
    {
        public DbSet<MunityUser> Users { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<OrganizationRole> OrganizationRoles { get; set; }

        public DbSet<OrganizationMember> OrganizationMember { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Committee> Committees { get; set; }

        public DbSet<Delegation> Delegation { get; set; }

        public DbSet<VisitorRole> Visitors { get; set; }

        public DbSet<DelegateRole> Delegates { get; set; }

        public DbSet<TeamRole> TeamRoles { get; set; }

        public DbSet<NgoRole> NgoRoles { get; set; }

        public DbSet<PressRole> PressRoles { get; set; }

        public DbSet<SecretaryGeneralRole> SecretaryGenerals { get; set; }

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

        public DbSet<SimulationHubConnection> SimulationHubConnections { get; set; }

        public DbSet<ListOfSpeakers> ListOfSpeakers { get; set; }

        

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<MunitySetting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<AbstractRole>().HasOne(n => n.Conference).WithMany(n => n.Roles);

            modelBuilder.Entity<TeamRole>().HasOne(n => n.TeamRoleGroup).WithMany(n => n.TeamRoles);

            modelBuilder.Entity<MunityUser>().HasOne(n => n.Auth).WithMany(n => n.Users);

            modelBuilder.Entity<MunityUser>().HasOne(n => n.PrivacySettings).WithOne(n => n.User).HasForeignKey<UserPrivacySettings>(n => n.UserRef);

            modelBuilder.Entity<MunityUser>().HasMany(n => n.CreatedResolutions).WithOne(a => a.CreationUser).IsRequired(false);

            modelBuilder.Entity<AbstractRole>().HasDiscriminator(n => n.RoleType)
                .HasValue<DelegateRole>("DelegateRole")
                .HasValue<NgoRole>("NgoRole")
                .HasValue<PressRole>("PressRole")
                .HasValue<SecretaryGeneralRole>("SecretaryGeneralRole")
                .HasValue<TeamRole>("TeamRole")
                .HasValue<VisitorRole>("VisitorRole");


            modelBuilder.Entity<Committee>().HasMany(n => n.Sessions).WithOne(n =>
                n.Committee);



            // The ResolutionId is also the Primary Key.
            modelBuilder.Entity<ResolutionAuth>().HasKey(n => n.ResolutionId);

            //modelBuilder.Entity<ResolutionUser>().HasKey(n => new { n.Auth.ResolutionId, n.CoreUserId });

            modelBuilder.Entity<ResolutionAuth>()
                .HasMany(n => n.Users).WithOne(n => n.Auth);


            modelBuilder.Entity<ResolutionAuth>().HasOne(n => n.Simulation).WithMany(n => n.Resolutions);

            modelBuilder.Entity<SimulationRole>().HasOne(n => n.Simulation).WithMany(n =>
                n.Roles);

            modelBuilder.Entity<SimulationUser>().HasOne(n => n.Simulation).WithMany(n =>
                n.Users);

            modelBuilder.Entity<SimulationHubConnection>().HasOne(n => n.User).WithMany(n => n.HubConnections);

            modelBuilder.Entity<ListOfSpeakers>().HasMany(n => n.AllSpeakers).WithOne(n => n.ListOfSpeakers);
            modelBuilder.Entity<ListOfSpeakers>().Ignore(n => n.CurrentSpeaker);
            modelBuilder.Entity<ListOfSpeakers>().Ignore(n => n.CurrentQuestion);
            modelBuilder.Entity<ListOfSpeakers>().Ignore(n => n.Speakers);
            modelBuilder.Entity<ListOfSpeakers>().Ignore(n => n.Questions);

            modelBuilder.Entity<Speaker>().HasKey(n => n.Id);
        }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }
    }
}

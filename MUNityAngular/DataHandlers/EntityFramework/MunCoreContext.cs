using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Conference.Roles;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Organisation;

namespace MUNityAngular.DataHandlers.EntityFramework
{
    public class MunCoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<OrganisationRole> OrganisationRoles { get; set; }

        public DbSet<OrganisationMember> OrganisationMember { get; set; }

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

        public DbSet<UserAuth> UserAuths { get; set; }

        public DbSet<RoleApplication> RoleApplications { get; set; }

        public DbSet<GroupApplication> GroupApplications { get; set; }

        public DbSet<GroupedRoleApplication> GroupedRoleApplications { get; set; }

        public DbSet<TeamRoleGroup> TeamRoleGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Committee>().HasOne(n => n.Conference)
                .WithMany(a => a.Committees).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrganisationRole>().HasOne(n => n.Organisation)
                .WithMany(a => a.Roles);

            modelBuilder.Entity<OrganisationMember>().HasOne(n => n.Organisation)
                .WithMany(n => n.Member);

            modelBuilder.Entity<Project>().HasOne(n => n.ProjectOrganisation)
                .WithMany(n => n.Projects);

            modelBuilder.Entity<Conference>().HasOne(n => n.ConferenceProject)
                .WithMany(n => n.Conferences);

            modelBuilder.Entity<AbstractRole>().HasOne(n => n.Conference).WithMany(n => n.Roles);

            modelBuilder.Entity<TeamRole>().HasOne(n => n.TeamRoleGroup).WithMany(n => n.TeamRoles);

            modelBuilder.Entity<User>().HasOne(n => n.Auth).WithMany(n => n.Users);

            modelBuilder.Entity<AbstractRole>().HasDiscriminator(n => n.RoleType)
                .HasValue<DelegateRole>("DelegateRole")
                .HasValue<NgoRole>("NgoRole")
                .HasValue<PressRole>("PressRole")
                .HasValue<SecretaryGeneralRole>("SecretaryGeneralRole")
                .HasValue<TeamRole>("TeamRole")
                .HasValue<VisitorRole>("VisitorRole");

            // Ignore a lot of Data Members to use them inside the Object Model of the API
            modelBuilder.Entity<OrganisationRole>().Ignore(n => n.OrganisationId);

            modelBuilder.Entity<OrganisationMember>().Ignore(n => n.Username)
                .Ignore(n => n.OrganisationId)
                .Ignore(n => n.RoleId);

            modelBuilder.Entity<Project>().Ignore(n => n.ProjectOrganisationId);

            modelBuilder.Entity<Committee>().Ignore(n => n.ResolutlyCommitteeId)
                .Ignore(n => n.ConferenceId);

            modelBuilder.Entity<Delegation>().Ignore(n => n.ConferenceId);



            modelBuilder.Entity<DelegateRole>().Ignore(n => n.DelegateStateId)
                .Ignore(n => n.CommitteeId)
                .Ignore(n => n.DelegateStateId);

            modelBuilder.Entity<TeamRole>().Ignore(n => n.ParentTeamRoleId)
                .Ignore(n => n.ParentTeamRoleId);

            modelBuilder.Entity<Conference>().Ignore(n => n.ConferenceProjectId);

            modelBuilder.Entity<Participation>().Ignore(n => n.RoleId)
                .Ignore(n => n.Username);

            modelBuilder.Entity<RoleAuth>().Ignore(n => n.ConferenceId);

            modelBuilder.Entity<RoleApplication>().Ignore(n => n.Username)
                .Ignore(n => n.RoleId);


        }

        public MunCoreContext(DbContextOptions<MunCoreContext> options) : base(options)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Conference.Roles;

namespace MUNityAngular.DataHandlers.EntityFramework
{
    public class ConferenceContext : DbContext
    {
        public DbSet<MUNityAngular.Models.Organisation.Organisation> Organisations { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Committee> Committees { get; set; }

        public DbSet<Delegation> Delegation { get; set; }

        public DbSet<VisitorRole> Visitors { get; set; }

        public DbSet<DelegateRole> Delegates { get; set; }

        public DbSet<TeamRole> TeamRoles { get; set; }

        public DbSet<NgoRole> NgoRoles { get; set; }

        public DbSet<PressRole> PressRoles { get; set; }
        
        public DbSet<Conference> Conferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Committee>().HasOne(n => n.Conference).WithMany(a => a.Committees)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public ConferenceContext(DbContextOptions<ConferenceContext> options) : base(options)
        {
            
        }
    }
}

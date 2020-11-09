using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Models.Simulation;

namespace MUNityCore.DataHandlers.EntityFramework
{
    public class MunityContext : DbContext
    {

        public DbSet<ResolutionAuth> ResolutionAuths { get; set; }

        public DbSet<ResolutionUser> ResolutionUsers { get; set; }

        public DbSet<Simulation> Simulations { get; set; }

        public DbSet<SimulationRole> SimulationRoles { get; set; }

        public DbSet<SimulationUser> SimulationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // The ResolutionId is also the Primary Key.
            modelBuilder.Entity<ResolutionAuth>().HasKey(n => n.ResolutionId);

            //modelBuilder.Entity<ResolutionUser>().HasKey(n => new { n.Auth.ResolutionId, n.CoreUserId });

            modelBuilder.Entity<ResolutionAuth>()
                .HasMany(n => n.Users).WithOne(n => n.Auth);


            modelBuilder.Entity<SimulationRole>().HasOne(n => n.Simulation).WithMany(n =>
                n.Roles);

            modelBuilder.Entity<SimulationUser>().HasOne(n => n.Simulation).WithMany(n =>
                n.Users);
        }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }
    }
}

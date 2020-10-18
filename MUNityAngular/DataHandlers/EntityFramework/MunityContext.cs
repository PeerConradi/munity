using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution.V2;

namespace MUNityAngular.DataHandlers.EntityFramework
{
    public class MunityContext : DbContext
    {

        public DbSet<ResolutionAuth> ResolutionAuths { get; set; }

        public DbSet<ResolutionUser> ResolutionUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // The ResolutionId is also the Primary Key.
            modelBuilder.Entity<ResolutionAuth>().HasKey(n => n.ResolutionId);

            //modelBuilder.Entity<ResolutionUser>().HasKey(n => new { n.Auth.ResolutionId, n.CoreUserId });

            modelBuilder.Entity<ResolutionAuth>()
                .HasMany(n => n.Users).WithOne(n => n.Auth);


            //modelBuilder.Entity<Models.UserAuths>().HasKey(n => n.User);
            //modelBuilder.Entity<Models.CommitteeDelegation>().HasKey(n => n.CommitteeDelegationId);
        }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }
    }
}

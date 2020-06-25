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

        public DbSet<Models.Gallery> Galleries { get; set; }

        public DbSet<Models.MediaImage> MediaImages { get; set; }

        public DbSet<ResolutionAuth> ResolutionAuths { get; set; }

        public DbSet<ResolutionUser> ResolutionUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Models.AuthKey>().HasKey(n => n.AuthKeyValue);

            // The ResolutionId is also the Primary Key.
            modelBuilder.Entity<ResolutionAuth>().HasKey(n => n.ResolutionId);
            //modelBuilder.Entity<Models.ResolutionUser>().HasKey(n => new { n.User, n.Resolution });
            //modelBuilder.Entity<Models.UserAuths>().HasKey(n => n.User);
            //modelBuilder.Entity<Models.CommitteeDelegation>().HasKey(n => n.CommitteeDelegationId);
        }

        public MunityContext(DbContextOptions<MunityContext> options) : base(options)
        {

        }
    }
}

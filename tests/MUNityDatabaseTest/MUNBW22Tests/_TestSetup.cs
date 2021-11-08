using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Test.MUNBW22Tests
{
    public partial class FullMUNBW22Tests
    {
        private MunityContext _context;

        private IServiceProvider _serviceProvider;




        [OneTimeSetUp]
        public void SetupDatabase()
        {

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<MunityContext>(options =>
                options.UseSqlite("Data Source=testmunbw.db"));

            serviceCollection.AddIdentity<MunityUser, MunityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<MunityContext>();

            // Needed to get the Identity Provider to run!
            serviceCollection.AddLogging();

            this._serviceProvider = serviceCollection.BuildServiceProvider();

            // Reset the Database.
            _context = _serviceProvider.GetRequiredService<MunityContext>();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [SetUp]
        public void ReloadContextToClearBuffer()
        {
            _context = _serviceProvider.GetRequiredService<MunityContext>();
        }

        public static class TestUsers
        {
            public static IEnumerable<MunityUser> AllUsers
            {
                get
                {
                    yield return Avangers.FoundingMembers.TonyStark;

                    yield return Avangers.SixtiesRecruits.CaptainAmerica;

                    yield return Avangers.SeventiesRecruits.BlackWidow;

                    yield return Avangers.NinetiesRecruits.PeterParker;

                    yield return XMen.OriginalMembers.ProfessorX;
                    yield return XMen.OriginalMembers.Cyclops;
                    yield return XMen.OriginalMembers.Iceman;
                    yield return XMen.OriginalMembers.Beast;
                    yield return XMen.OriginalMembers.Angel;
                    yield return XMen.OriginalMembers.MarvelGirl;
                }
            }

            public static class Avangers
            {
                /// <summary>
                ///  Use the founding members as EPL
                /// </summary>
                public static class FoundingMembers
                {
                    /// <summary>
                    /// User is the owner of the Organization (Clemens)
                    /// weitere Organisationsbenutzer sind in diesem Test nicht notwendig, da es hier um MUNBW und nicht um organisationen an sich geht...
                    /// </summary>
                    public static MunityUser TonyStark { get; set; } = new MunityUser("tonystark", "tony@stark-industries.com") { Forename = "Antony", Lastname = "Stark" };
                    public static MunityUser HankPym { get; set; } = new MunityUser("hankpym", "hank-pym@avangers.com") { Forename = "Henry Jonathan", Lastname = "Pym" };

                    /// <summary>
                    /// Projektleiter 1 (J. T.)
                    /// </summary>
                    public static MunityUser JanetVanDyne { get; set; } = new MunityUser("jandyne", "janet-dyne@avangers.com") { Forename = "Janet", Lastname = "van Dyne" };

                    /// <summary>
                    /// Projektleiter 2 (M. I.)
                    /// </summary>
                    public static MunityUser TheHulk { get; set; } = new MunityUser("hulk", "hulk@avangers.com") { Forename = "Robert Bruce", Lastname = "Banner" };

                    /// <summary>
                    /// Projektleiter 3 (T. S.)
                    /// </summary>
                    public static MunityUser Thor { get; set; } = new MunityUser("rickjones", "rock@avangers.com") { Forename = "Richard Milhouse", Lastname = "Jones" };

                }

                public static class SixtiesRecruits
                {
                    /// <summary>
                    /// Generalsekretär (J. M.)
                    /// </summary>
                    public static MunityUser CaptainAmerica { get; set; } = new MunityUser("muricaboi", "captain@amrica.com") { Forename = "Steve", Lastname = "Rogers" };

                    /// <summary>
                    /// Leitung Inhalt & Sekretariat (K. V.)
                    /// </summary>
                    public static MunityUser Hawkeye { get; set; } = new MunityUser("hawkeye", "hawkeye@amrica.com") { Forename = "Clinton Francis", Lastname = "Barton" };


                }

                public static class SeventiesRecruits
                {
                    public static MunityUser BlackWidow { get; set; } = new MunityUser("blackwidow", "b.widow@avangers.com") { Forename = "Natasha", Lastname = "Romanoff" };

                }

                public static class NinetiesRecruits
                {
                    public static MunityUser PeterParker { get; set; } = new MunityUser("pparker", "parker@spiderman.com") { Forename = "Peter Benjamin", Lastname = "Parker" };

                }




            }

            public static class XMen
            {
                /// <summary>
                /// Delegation aus 6
                /// </summary>
                public static class OriginalMembers
                {
                    public static MunityUser ProfessorX { get; set; } = new MunityUser("professorx", "professorx@x-men.com") { Forename = "Charles Francis", Lastname = "Xavier" };
                    public static MunityUser Cyclops { get; set; } = new MunityUser("cyclops", "cyclops@x-men.com") { Forename = "Scott", Lastname = "Summers" };
                    public static MunityUser Iceman { get; set; } = new MunityUser("iceman", "iceman@x-men.com") { Forename = "Robert Louis", Lastname = "Drake" };
                    public static MunityUser Beast { get; set; } = new MunityUser("beast", "beast@x-men.com") { Forename = "Henry Philip", Lastname = "McCoy" };
                    public static MunityUser Angel { get; set; } = new MunityUser("angel", "angel@x-men.com") { Forename = "Warren Kenneth", Lastname = "Worthington III" };
                    public static MunityUser MarvelGirl { get; set; } = new MunityUser("marvelgirl", "marvel.girl@x-men.com") { Forename = "Jean Elaine", Lastname = "Grey" };

                }
            }
        }
    }
}

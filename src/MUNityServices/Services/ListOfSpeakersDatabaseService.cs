using MUNity.Database.Context;
using MUNity.Database.Models.LoS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class ListOfSpeakersDatabaseService : IDisposable
    {
        private readonly MunityContext dbContext;

        public ListOfSpeakers GetList(string id)
        {
            throw new NotImplementedException();
        }

        public ListOfSpeakers CreateList(string id = null)
        {
            var databaseModel = new ListOfSpeakers();
            if (id != null)
            {
                databaseModel.ListOfSpeakersId = id;
            }
            dbContext.ListOfSpeakers.Add(databaseModel);
            dbContext.SaveChanges();
            return databaseModel;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public ListOfSpeakersDatabaseService(MunityContext context)
        {
            this.dbContext = context;
        }
    }
}

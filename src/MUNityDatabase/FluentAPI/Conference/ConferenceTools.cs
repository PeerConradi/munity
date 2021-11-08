using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.FluentAPI;

public class ConferenceTools
{
    private MunityContext _dbContext;

    public Conference AddConference(Action<ConferenceOptionsBuilder> options)
    {
        var builder = new ConferenceOptionsBuilder(_dbContext);
        options(builder);
        _dbContext.Conferences.Add(builder.Conference);
        _dbContext.SaveChanges();
        return builder.Conference;
    }

    public ConferenceTools(MunityContext context)
    {
        this._dbContext = context;
    }
}

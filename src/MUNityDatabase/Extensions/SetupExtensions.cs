using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Context;

namespace MUNity.Database.Extensions;

public static class SetupExtensions
{
    public static int SetupBaseCountries(this MunityContext context)
        => context.AddBaseCountries(BaseData.Countries.BaseCountries);

    public static int SetupBaseRoles(this MunityContext context)
    {

        if (!context.Roles.Any())
        {
            foreach (var munityRole in BaseData.DefaultAuthorizations.UserRoles)
            {
                context.Roles.Add(munityRole);
            }
        }

        return context.SaveChanges();
    }
}

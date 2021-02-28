using MUNityCore.DataHandlers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Models.Resolution.SqlResa;

namespace MUNityCore.Services
{
    public class SqlResolutionService
    {
        private MunityContext _context;

        public SqlResolutionService(MunityContext context)
        {
            this._context = context;
        }

        public async Task CreatePublicResolution(string name)
        {
            var resolution = new ResaElement()
            {
                Topic = name,
                Name = name,
                FullName = name
            };

            this._context.Resolutions.Add(resolution);
            await this._context.SaveChangesAsync();
        }
    }
}

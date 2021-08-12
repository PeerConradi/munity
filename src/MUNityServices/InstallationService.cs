using MUNity.Database.Context;
using MUNityCore.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Services
{
    public class InstallationService
    {
        private readonly MunityContext _context;

        public InstallationService(MunityContext context)
        {
            this._context = context;
        }

        public void Install()
        {
            // TODO!
        }
    }
}

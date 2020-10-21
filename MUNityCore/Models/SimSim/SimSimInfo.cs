using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.SimSim
{
    public class SimSimInfo
    {
        public int SimSimId { get; internal set; }

        public string Name { get; internal set; }

        public int UserCount { get; internal set; }

        public bool UsesPassword { get; internal set; }

        public static explicit operator SimSimInfo(SimSimModel f)
        {
            return new SimSimInfo(f);
        }

        //public static implicit operator SimSimInfo(SimSimModel f)
        //{
        //    return new SimSimInfo(f);
        //}

        public SimSimInfo(SimSimModel model)
        {
            this.SimSimId = model.SimSimId;
            this.Name = model.Name;
            this.UsesPassword = model.UsingPassword;
            this.UserCount = model.Users.Count;
        }
    }
}

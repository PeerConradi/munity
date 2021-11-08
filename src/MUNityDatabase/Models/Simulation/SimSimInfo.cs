using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation;

public class SimSimInfo
{
    public int SimSimId { get; internal set; }

    public string Name { get; internal set; }

    public int UserCount { get; internal set; }

    public bool UsesPassword { get; internal set; }

    public static explicit operator SimSimInfo(Simulation f)
    {
        return new SimSimInfo(f);
    }

    //public static implicit operator SimSimInfo(Simulation f)
    //{
    //    return new SimSimInfo(f);
    //}

    public SimSimInfo(Simulation model)
    {
        this.SimSimId = model.SimulationId;
        this.Name = model.Name;
        this.UsesPassword = !string.IsNullOrEmpty(model.Password);
        this.UserCount = model.Users.Count;
    }
}

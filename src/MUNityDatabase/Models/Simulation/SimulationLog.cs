using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation;

public class SimulationLog
{
    public int SimulationLogId { get; set; }

    public Simulation Simulation { get; set; }

    [MaxLength(250)]
    public string CategoryName { get; set; }

    [MaxLength(250)]
    public string Name { get; set; }

    [MaxLength(2000)]
    public string Text { get; set; }

    public DateTime Timestamp { get; set; }
}

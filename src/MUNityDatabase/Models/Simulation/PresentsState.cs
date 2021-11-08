using System;

namespace MUNity.Database.Models.Simulation;

public class PresentsState
{
    public enum PresentsStates
    {
        NotChecked,
        Present,
        Absent,
        Late
    }

    public int PresentsStateId { get; set; }

    public SimulationPresents SimulationPresents { get; set; }

    public SimulationUser SimulationUser { get; set; }

    public PresentsStates State { get; set; }

    public string StateValue { get; set; }

    public DateTime? RegistertTimestamp { get; set; }
}

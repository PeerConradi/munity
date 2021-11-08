using MUNity.Base;

namespace MUNity.Database.Models.Simulation;

public class PetitionTemplateEntry
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Reference { get; set; }

    public PetitionRulings Ruling { get; set; }

    public string Category { get; set; }

    public bool AllowChairs { get; set; }

    public bool AllowDelegates { get; set; }

    public bool AllowNgo { get; set; }

    public bool AllowSpectator { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation;

public class SimulationUser
{
    public int SimulationUserId { get; set; }

    public string Token { get; set; }

    public string DisplayName { get; set; }

    public string PublicUserId { get; set; }

    public string Password { get; set; }

    public int PinRetries { get; set; }

    public SimulationRole Role { get; set; }

    public bool CanCreateRole { get; set; }

    public bool CanSelectRole { get; set; }

    public bool CanEditResolution { get; set; } = false;

    public bool CanEditListOfSpeakers { get; set; } = false;

    public string LastKnownConnectionId { get; set; }

    public Simulation Simulation { get; set; }

    public ICollection<Petition> Petitions { get; set; }

    public SimulationUser()
    {
        this.Token = Util.IdGenerator.RandomString(20);
        this.Password = Util.IdGenerator.RandomString(8);
        this.PublicUserId = new Random().Next(100000000, 999999999).ToString();
        PinRetries = 0;
    }
}

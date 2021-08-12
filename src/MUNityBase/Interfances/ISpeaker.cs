using System;
using System.Collections.Generic;
using System.Text;

namespace MUNityBase.Interfances
{
    public interface ISpeaker : IComparable<ISpeaker>
    {
        string Id { get; set; }

        string Name { get; set; }

        string Iso { get; set; }

        SpeakerModes Mode { get; set; }

        int OrdnerIndex { get; set; }
    }
}

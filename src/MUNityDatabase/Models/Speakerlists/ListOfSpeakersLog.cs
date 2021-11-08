using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.LoS;

public class ListOfSpeakersLog
{
    public long ListOfSpeakersLogId { get; set; }

    public ListOfSpeakers Speakerlist { get; set; }

    public string SpeakerIso { get; set; }

    public string SpeakerName { get; set; }

    public long PermitedSpeakingSeconds { get; set; }

    public long PermitedQuestionsSeconds { get; set; }

    public long UsedSpeakerSeconds { get; set; }
    public long UsedQuestionSeconds { get; set; }

    public int TimesOnSpeakerlist { get; set; }

    public int TimesOnQuestions { get; set; }

    public int TimesSpeaking { get; set; }

    public int TimesQuestioning { get; set; }
}

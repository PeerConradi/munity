import { TimeSpan } from "./TimeSpan";
import { Delegation } from "./delegation.model";

export class Speakerlist {
  public ID: string;
  public PublicId: number;

  public Name: string 

  public Speakertime: TimeSpan;

  public Questiontime: TimeSpan;

  public RemainingSpeakerTime: TimeSpan;

  public RemainingQuestionTime: TimeSpan;

  public Speakers: Delegation[]; 

  public Questions: Delegation[];

  public CurrentSpeaker: Delegation;

  public CurrentQuestion: Delegation;

  public Status: number;

  public IsSpeaking: boolean;

  public IsQuestioning: boolean;

  public ListClosed: boolean;

  public QuestionsClosed: boolean;

  public LowTimeMark: TimeSpan;

  public SpeakerLowTime: boolean; 

  public QuestionLowTime: boolean;

  public SpeakerTimeout: boolean;

  public QuestionTimeout: boolean;

  public ConferenceId: string;

  public CommitteeId: string;
}

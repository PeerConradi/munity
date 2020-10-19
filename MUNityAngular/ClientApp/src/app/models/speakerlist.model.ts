import { TimeSpan } from "./TimeSpan";
import { Delegation } from "./conference/delegation.model";

export class Speakerlist {
  public id: string;
  public publicId: number;

  public name: string

  public speakertime: TimeSpan = new TimeSpan(0, 0, 3, 0, 0);

  public questiontime: TimeSpan = new TimeSpan(0, 30, 0, 0, 0);

  public remainingSpeakerTime: TimeSpan = new TimeSpan(0, 0, 3, 0, 0);

  public remainingQuestionTime: TimeSpan = new TimeSpan(0, 30, 0, 0, 0);

  public speakers: Speaker[];

  public questions: Speaker[];

  public currentSpeaker: Speaker;

  public currentQuestion: Speaker;

  public status: number;

  public isSpeaking: boolean;

  public isQuestioning: boolean;

  public listClosed: boolean;

  public questionsClosed: boolean;

  public lowTimeMark: TimeSpan;

  public speakerLowTime: boolean;

  public questionLowTime: boolean;

  public speakerTimeout: boolean;

  public questionTimeout: boolean;

  public conferenceId: string;

  public committeeId: string;
}

export class Speaker {
  public id: string;

  public name: string;

  // for an icon
  public iso: string;
}
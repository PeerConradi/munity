export class AbstractAmendment {

  get ID(): string { return this.id; }
  set ID(val: string) { this.id = val; }

  get Name(): string { return this.name; }
  set Name(val: string) { this.name = val; }

  get TargetSectionID(): string { return this.targetSectionID; }
  set TargetSectionID(val: string) { this.targetSectionID = val; }

  get Activated(): boolean { return this.activated; }
  set Activated(val: boolean) { this.activated = val; }

  get SubmitterName(): string { return this.submitterName; }
  set SubmitterName(val: string) { this.submitterName = val; }

  get SubmitTime(): Date { return this.submitTime; }
  set SubmitTime(val: Date) { this.submitTime = val; }

  activated: boolean;
  id: string;
  name: string;
  targetSectionID: string;
  submitterName: string;
  submitTime: Date;
}

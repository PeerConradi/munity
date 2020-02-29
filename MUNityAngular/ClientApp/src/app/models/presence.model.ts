import { Delegation } from "./delegation.model";

export class Presence {
  Id: string;
  Present: Delegation[] = [];
  Absent: Delegation[] = [];
  Remaining: Delegation[] = [];
  CommitteeId: string;
  CheckDate: Date;
}

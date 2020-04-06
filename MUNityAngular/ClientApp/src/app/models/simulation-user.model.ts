import { Delegation } from "./delegation.model";

export class SimulationUser {
  UserToken: string;
  DisplayName: string;
  Delegation: Delegation;
  IsChair: boolean;
}

import { Delegation } from "../conference/delegation.model";

export class SimulationUser {
  UserToken: string;
  DisplayName: string;
  Delegation: Delegation;
  IsChair: boolean;
  HiddenToken: string;
  Role: string;
}

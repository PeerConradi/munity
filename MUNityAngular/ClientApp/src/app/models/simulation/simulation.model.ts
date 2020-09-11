import { SimulationUser } from "./simulation-user.model";
import { SimulationMessage } from "./simulation-message.model";
import { Speakerlist } from "../speakerlist.model";
import { SimulationRequest } from "./simulation-request.model";

export class Simulation {
  SimSimId: string;
  Name: string;
  Users: SimulationUser[] = [];
  UsingPassword: boolean;
  CanJoin: boolean;
  AllChat: SimulationMessage[] = [];
  Speakerlist: Speakerlist;
  Requests: SimulationRequest[] = [];
}

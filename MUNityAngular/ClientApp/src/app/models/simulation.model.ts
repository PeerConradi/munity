import { SimulationUser } from "./simulation-user.model";
import { SimulationMessage } from "./simulation-message.model";

export class Simulation {
  SimSimId: string;
  Name: string;
  Users: SimulationUser[] = [];
  UsingPassword: boolean;
  CanJoin: boolean;
  AllChat: SimulationMessage[] = [];
}

import { SimulationUser } from "./simulation-user.model";

export class Simulation {
  SimSimId: string;
  Name: string;
  Users: SimulationUser[];
  UsingPassword: boolean;
  CanJoin: boolean;
}

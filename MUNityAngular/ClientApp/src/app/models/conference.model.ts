import { Committee } from "./committee.model";
import { Delegation } from "./delegation.model";

export class Conference {
  ID: string;
  Name: string;
  FullName: string;
  Abbreviation: string;
  Committees: Committee[];
  Delegations: Delegation[];
  CreationDate: Date;
  StartDate: Date;
  EndDate: Date;
  SecretaryGeneralTitle: string;
  SecretaryGeneralName: string;
}

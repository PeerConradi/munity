import { Committee } from "./committee.model";

export class Conference {
  ID: string;
  Name: string;
  FullName: string;
  Abbreviation: string;
  Committees: Committee[];
  CreationDate: Date;
  StartDate: Date;
  EndDate: Date;
  SecretaryGeneralTitle: string;
  SecretaryGeneralName: string;
}

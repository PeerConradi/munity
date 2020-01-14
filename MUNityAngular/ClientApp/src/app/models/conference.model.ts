import { Committee } from "./committee.model";

export class Conference {
  id: string;
  name: string;
  fullName: string;
  abbreviation: string;
  committees: Committee[];
  creationDate: Date;
  startDate: Date;
  endDate: Date;
  secretaryGeneralTitle: string;
  secretaryGeneralName: string;
}

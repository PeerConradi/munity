import { Committee } from "./committee.model";
import { Delegation } from "./delegation.model";

export class Conference {
  public conferenceId: string;

  public name: string;

  public fullName: string;

  public abbreviation: string;

  public startDate: Date;

  public endDate: Date;

  public creationDate: Date;

  public committees: Committee[];
}

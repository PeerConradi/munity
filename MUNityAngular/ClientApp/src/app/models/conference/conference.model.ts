import { Committee } from "./committee.model";
import { Delegation } from "./delegation.model";
import * as r from "./roles";

export class Conference {
  public conferenceId: string;

  public name: string;

  public fullName: string;

  public abbreviation: string;

  public startDate: Date;

  public endDate: Date;

  public creationDate: Date;

  public committees: Committee[];

  public roles: r.Roles.AbstractRole[];
}

export class ConferenceInfo {
  public conferenceId: string;

  public name: string;

  public fullName: string;

  public abbreviation: string;

  public startDate: Date;

  public endDate: Date;
}
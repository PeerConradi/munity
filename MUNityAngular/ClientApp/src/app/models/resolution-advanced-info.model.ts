import { ResolutionInformation } from "./resolution-information.model";

export class ResolutionAdvancedInfo extends ResolutionInformation {
  public OnlineCode: string;

  public PublicRead: boolean;

  public PublicWrite: boolean;

  public PublicAmendment: boolean;

  public CreationDate: Date;

  public LastChangedDate: Date;

}

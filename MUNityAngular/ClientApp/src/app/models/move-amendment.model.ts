import { AbstractAmendment } from "./abstract-amendment.model";

export class MoveAmendment extends AbstractAmendment {
  NewPosition: string;
  //The Id of the Operative Section that is created for the Move location of this amendment.
  NewSectionID: string;
}

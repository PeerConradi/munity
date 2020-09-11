import { AbstractAmendment } from "./abstract-amendment.model";

export class AddAmendment extends AbstractAmendment {
  TargetPosition: number;

  NewText: string;

  TargetResolutionID: string;
}

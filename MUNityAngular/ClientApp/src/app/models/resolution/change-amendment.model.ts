import { AbstractAmendment } from "./abstract-amendment.model";

export class ChangeAmendment extends AbstractAmendment {
  newText: string;

  /**
   *
   */
  constructor() {
    super();
    this.type = "ChangeAmendment";
    this.newText = '';
  }
}

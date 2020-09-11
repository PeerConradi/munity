import { OperativeSection } from './operative-section.model';
import { Preamble } from './preamble.model';
import { DeleteAmendment } from './delete-amendment.model';
import { AbstractAmendment } from './abstract-amendment.model';
import { ChangeAmendment } from './change-amendment.model';
import { MoveAmendment } from './move-amendment.model';
import { AddAmendment } from './add-amendment.model';
import { ResolutionHeader } from "./resolution-header.model";

export class Resolution {
  resolutionId: string;
  date: Date;
  header: ResolutionHeader;

  preamble: Preamble;

  operativeSection: OperativeSection;

}

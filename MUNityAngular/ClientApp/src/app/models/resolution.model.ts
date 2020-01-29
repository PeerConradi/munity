import { OperativeSection } from './operative-section.model';
import { Preamble } from './preamble.model';
import { DeleteAmendment } from './delete-amendment.model';
import { AbstractAmendment } from './abstract-amendment.model';

export class Resolution {
  ID: string;
  Topic: string;
  Name: string;
  OperativeSections: OperativeSection[];
  Preamble: Preamble;
  lastSaved: Date;
  CommitteeName: string;
  SubmitterName: string;
  SupporterNames: string[] = [];
  DeleteAmendments: DeleteAmendment[] = [];
}
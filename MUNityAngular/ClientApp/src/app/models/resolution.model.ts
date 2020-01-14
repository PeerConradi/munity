import { OperativeSection } from './operative-section.model';
import { Preamble } from './preamble.model';

export class Resolution {
  ID: string;
  Topic: string;
  Name: string;
  OperativeSections: OperativeSection[];
  Preamble: Preamble;
  lastSaved: Date;
}

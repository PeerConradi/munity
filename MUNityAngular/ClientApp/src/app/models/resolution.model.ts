import { OperativeSection } from './operative-section.model';
import { Preamble } from './preamble.model';

export class Resolution {
  ID: string;
  OperativeSections: OperativeSection[];
  Preamble: Preamble;
  lastSaved: Date;
}

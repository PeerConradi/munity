import { Notice } from "./notice.model";

export class PreambleParagraph {
  Text: string;
  ID: string;

  ResolutionID: string;
  Notices: Notice[] = [];
}

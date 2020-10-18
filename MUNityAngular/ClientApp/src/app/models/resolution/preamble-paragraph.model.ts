import { Notice } from "../notice.model";

export class PreambleParagraph {

  preambleParagraphId: string;
  text: string;

  notices: Notice[] = [];

  differsFromServer: boolean = false;
}

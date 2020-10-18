import { Notice } from "../notice.model";

export class OperativeParagraph {
  operativeParagraphId: string;

  name: string;

  isLocked: boolean;

  isVirtual: boolean;

  text: string;

  visible: boolean;

  children: OperativeParagraph[] = [];

  notices: Notice[] = [];

  // internal for front End
  differsFromServer: boolean = false;
}

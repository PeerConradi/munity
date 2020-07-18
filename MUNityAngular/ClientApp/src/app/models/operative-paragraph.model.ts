import { Notice } from "./notice.model";

export class OperativeParagraph {
  operativeParagraphId: string;

  name: string;

  isLocked: string;

  isVirtual: string;

  text: string;

  visible: boolean;

  children: OperativeParagraph[] = [];

  notices: Notice[] = [];
}

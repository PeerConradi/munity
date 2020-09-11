import { Notice } from "../notice.model";
import { OperativeParagraph } from "./operative-paragraph.model";
import { ChangeAmendment } from "./change-amendment.model";
import { AddAmendment } from "./add-amendment.model";
import { MoveAmendment } from "./move-amendment.model";
import { DeleteAmendment } from "./delete-amendment.model";

export class OperativeSection {

  operativeSectionId: string;

  paragraphs: OperativeParagraph[] = [];

  changeAmendments: ChangeAmendment[] = [];

  addAmendments: AddAmendment[] = [];

  moveAmendments: MoveAmendment[] = [];

  deleteAmendments: DeleteAmendment[] = [];
  
}

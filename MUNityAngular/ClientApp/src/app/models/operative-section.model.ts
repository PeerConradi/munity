export class OperativeSection {
  ID: string;
  Name: string;
  IsLocked: boolean;
  IsVirtual: boolean;
  Text: string;
  Path: string;
  ParentID: string;
  ResolutionID: string;
  AmendmentParagraph: boolean;
  Children: string[] = [];
  DeleteAmendmentCount: number = 0;
  ChangeAmendmentCount: number = 0;
  MoveAmendmentCount: number = 0;

}

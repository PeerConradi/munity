import { NoticeTag } from "./notice-tag.model";

export class Notice {
  Id: string;
  AuthorName: string;
  CreationDate: Date;
  Title: string;
  Text: string;
  Tags: NoticeTag[];
}

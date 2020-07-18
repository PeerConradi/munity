import { NoticeTag } from "./notice-tag.model";

export class Notice {
  id: string;
  authorName: string;
  creationDate: Date;
  title: string;
  text: string;
  tags: NoticeTag[] = [];
  readBy: string[] = [];
}

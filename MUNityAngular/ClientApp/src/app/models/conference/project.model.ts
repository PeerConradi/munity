import { Conference, ConferenceInfo } from "./conference.model";

export class Project {
    public projectId: string;

    public projectName: string;

    public projectAbbreviation: string;

    public conferences: Conference[];
}


export class ProjectInfo {
    public projectId: string;

    public projectName: string;

    public projectAbbreviation: string;

    public conferences: ConferenceInfo[];
}

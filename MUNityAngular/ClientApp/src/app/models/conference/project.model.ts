import { Conference, ConferenceInfo } from "./conference.model";

export class Project {
    public projectId: string;

    public projectName: string;

    public projectAbbreviation: string;

    public projectOrganisationId: string;

    public conferences: Conference[];
}
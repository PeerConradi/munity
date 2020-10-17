import { Project } from "./conference/project.model";

export class Organisation {
    public organisationId: string;

    public organisationName: string;

    public organisationAbbreviation: string;

    // Will not be given when calling the default GetOrganisation Model.

    public roles: OrganisationRole[];

    public member: OrganisationMember[];

    public projects: Project[];
}

export class OrganisationRole {
    public OrganisationRoleId: number;

    public roleName: string;

    public organisationId: string;

    public canCreateProject: boolean;
}

export class OrganisationMember {
    public organisationMemberId: number;

    public username: string;

    public organisationId: string;

    public roleId: number;
}
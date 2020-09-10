export class AbstractRole {
    roleId: number;

    roleName: string;

    iconName: string;

    applicationState: EApplicationStates;

    applicationValue: string;

    allowMultipleParticipations: boolean;
}

export enum EApplicationStates {
    CLOSED,
    DIRECT_APPLICATION,
    CATEGORY_APPLICATION,
    REGISTRATION,
    DELEGATION_APPLICATION,
    COMMITTEE_APPLICATION,
    CLOSED_TO_PUBLIC,
    CUSTOM
}

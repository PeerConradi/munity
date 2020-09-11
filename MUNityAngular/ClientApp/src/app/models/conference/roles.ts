namespace Roles {
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

    export enum EPressCategories {
        PRINT,
        TV,
        ONLINE
    }

    export class DelegateRole extends AbstractRole {
        public isDelegationLeader: boolean;

        public title: string;

        public delegationId: string;

        public committeeId: string;

        public delegateStateId: string;
    }

    export class NgoRole extends AbstractRole {
        public group: number;

        public ngoName: string;

        public leader: boolean;
    }

    export class PressRole extends AbstractRole {
        public pressCategory: EPressCategories;
    }

    export class SecretaryGeneralRole extends AbstractRole {
        public title: string;
    }

    export class TeamRole extends AbstractRole {
        public parentTeamRoleId: number;

        public teamRoleGroup: number;
    }

    export class VisitorRole extends AbstractRole {
        public organisation: string;
    }
}
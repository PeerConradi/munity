export namespace Roles {
    export class AbstractRole {
        public roleId: number;

        public roleName: string;

        public roleFullName: string;

        public roleShort: string;

        public iconName: string;

        public applicationState: EApplicationStates;

        public applicationValue: string;

        public allowMultipleParticipations: boolean;

        public roleType: string;
    }

    export enum EApplicationStates {
        CLOSED = 0,
        DIRECT_APPLICATION = 1,
        CATEGORY_APPLICATION = 2,
        REGISTRATION = 3,
        DELEGATION_APPLICATION = 4,
        COMMITTEE_APPLICATION = 5,
        CLOSED_TO_PUBLIC = 6,
        CUSTOM = 7
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

        constructor() {
            super();
            this.roleType = 'DelegateRole';
        }
    }

    export class NgoRole extends AbstractRole {
        public group: number;

        public ngoName: string;

        public leader: boolean;

        constructor() {
            super();
            this.roleType = 'NgoRole';
        }
    }

    export class PressRole extends AbstractRole {
        public pressCategory: EPressCategories;

        constructor() {
            super();
            this.roleType = 'PressRole';
        }
    }

    export class SecretaryGeneralRole extends AbstractRole {
        public title: string;

        constructor() {
            super();
            this.roleType = 'SecretaryGeneralRole';
        }
    }

    export class TeamRole extends AbstractRole {
        public parentTeamRoleId: number;

        public teamRoleLevel: number;

        public teamRoleGroupId: number;

        constructor() {
            super();
            this.roleType = 'TeamRole';
        }
    }

    export class VisitorRole extends AbstractRole {
        public organisation: string;

        constructor() {
            super();
            this.roleType = 'VisitorRole';
        }
    }

    export class TeamRoleGroup {
        public teamRoleGroupId: number;

        public name: string;

        public fullName: string;

        public abbreviation: string;

      public groupLevel: number;

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.Core;
using MUNityCore.Models.Organization;

namespace MUNityTest.TestEnvironment
{
    class ConferenceEnvironment
    {
        public ConferenceEnvironment(MUNityCore.DataHandlers.EntityFramework.MunCoreContext context)
        {
            context.Organisations.Add(TestOrganization);
            context.Projects.Add(TestProject);
            context.Conferences.Add(TestConference);
            context.Committees.Add(TestCommitteeGeneralAssembly);
            context.Committees.Add(TestCommitteeSecurityCouncil);
            context.Users.Add(TestUserMax);
            context.Users.Add(TestUserMike);
            context.Users.Add(TestUserMillie);
            context.Users.Add(TestUserFinn);
            context.RoleAuths.Add(RoleAuthLeader);
            context.RoleAuths.Add(RoleAuthPesant);
            context.TeamRoles.Add(TestRoleProjectLeader);
            context.TeamRoles.Add(TestRoleSecretaryLeader);
            context.SaveChanges();
        }

        private Organization _organization;
        private Project _project;
        private Conference _conference;
        private Committee _committeeGeneralAssembly;
        private Committee _committeeSecurityCouncil;
        
        public Organization TestOrganization
        {
            get
            {
                if (_organization == null)
                {
                    _organization = new Organization
                    {
                        OrganizationName = "United Nations", OrganizationAbbreviation = "UN"
                    };
                }
                return _organization;
            }
        }

        public Project TestProject
        {
            get
            {
                if (_project == null)
                {
                    _project = new Project
                    {
                        ProjectName = "Model United Nations",
                        ProjectAbbreviation = "MUN",
                        ProjectOrganization = TestOrganization
                    };
                }

                return _project;
            }
        }

        public Conference TestConference
        {
            get
            {
                if (_conference == null)
                {
                    _conference = new Conference
                    {
                        Name = "Model United Nations 2021",
                        Abbreviation = "MUN 2021",
                        FullName = "Official Model United Nations 2021",
                        StartDate = new DateTime(2021, 01, 01),
                        EndDate = new DateTime(2021, 12, 31),
                        ConferenceProject = TestProject
                    };
                }

                return _conference;
            }
        }

        public Committee TestCommitteeGeneralAssembly
        {
            get
            {
                if (_committeeGeneralAssembly == null)
                {
                    _committeeGeneralAssembly = new Committee
                    {
                        Conference = _conference,
                        Name = "Generalversammlung",
                        FullName = "die Generalversammlung",
                        CommitteeShort = "GV"
                    };
                }

                return _committeeGeneralAssembly;
            }
        }


        public Committee TestCommitteeSecurityCouncil
        {
            get
            {
                if (_committeeSecurityCouncil == null)
                {
                    _committeeSecurityCouncil = new Committee
                    {
                        Conference = _conference,
                        Name = "Sicherheitsrat",
                        FullName = "der Sicherheitsrat",
                        CommitteeShort = "SR"
                    };
                }

                return _committeeSecurityCouncil;
            }
        }

        private User _userMax;
        public User TestUserMax
        {
            get
            {
                if (_userMax == null)
                {
                    _userMax = new User
                    {
                        Username = "maxmustermann",
                        Title = "",
                        Forename = "Max",
                        Lastname = "Mustermann",
                        Mail = "max-mustermann@mail.com"
                    };
                }

                return _userMax;
            }
        }

        private User _userMike;
        public User TestUserMike
        {
            get
            {
                if (_userMike == null)
                {
                    _userMike = new User
                    {
                        Username = "mikelitoris",
                        Title = "",
                        Forename = "Mike",
                        Lastname = "Litoris",
                        Mail = "mike@litoris.com"
                    };
                }

                return _userMike;
            }
        }

        private User _userMillie;
        public User TestUserMillie
        {
            get
            {
                if (_userMillie == null)
                {
                    _userMillie = new User
                    {
                        Username = "mbb",
                        Title = "",
                        Forename = "Millie Bobby",
                        Lastname = "Brown",
                        Mail = "millie-brown@mail.com"
                    };
                }

                return _userMillie;
            }
        }

        private User _userFinn;
        public User TestUserFinn
        {
            get
            {
                if (_userFinn == null)
                {
                    _userFinn = new User
                    {
                        Username = "f.wolfhard",
                        Title = "",
                        Forename = "Finn",
                        Lastname = "Wolfhard",
                        Mail = "finn@mail.com"
                    };
                }

                return _userFinn;
            }
        }

        private RoleAuth _roleAuthLeader;

        public RoleAuth RoleAuthLeader
        {
            get
            {
                if (_roleAuthLeader == null)
                {
                    _roleAuthLeader = new RoleAuth
                    {
                        Conference = TestConference,
                        CanEditConferenceSettings = true,
                        CanEditParticipations = true,
                        CanSeeApplications = true
                    };
                }

                return _roleAuthLeader;
            }
        }

        private RoleAuth _roleAuthPesant;

        public RoleAuth RoleAuthPesant
        {
            get
            {
                if (_roleAuthPesant == null)
                {
                    _roleAuthPesant = new RoleAuth
                    {
                        Conference = TestConference,
                        CanEditConferenceSettings = false,
                        CanEditParticipations = false,
                        CanSeeApplications = false
                    };
                }

                return _roleAuthPesant;
            }
        }

        private TeamRole _teamRoleProjectLeader;

        public TeamRole TestRoleProjectLeader
        {
            get
            {
                if (_teamRoleProjectLeader == null)
                {
                    _teamRoleProjectLeader = new TeamRole
                    {
                        RoleName = "Projektleitung",
                        Conference = TestConference,
                        RoleAuth = RoleAuthLeader,
                        ApplicationState = EApplicationStates.Closed
                    };
                }

                return _teamRoleProjectLeader;
            }
        }

        private TeamRole _teamRoleSecretaryLeader;
        public TeamRole TestRoleSecretaryLeader
        {
            get
            {
                if (_teamRoleSecretaryLeader == null)
                {
                    _teamRoleSecretaryLeader = new TeamRole
                    {
                        Conference = TestConference, ParentTeamRole = TestRoleProjectLeader
                    };
                }

                return _teamRoleSecretaryLeader;
            }
        }

        private PressRole _pressRole;

        public PressRole TestPressRole
        {
            get
            {
                if (_pressRole == null)
                {
                    _pressRole = new PressRole
                    {
                        RoleName = "Zeitungs-Journalist",
                        PressCategory = PressRole.EPressCategories.Print,
                        Conference = TestConference,
                        RoleAuth = RoleAuthPesant
                    };
                }

                return _pressRole;
            }
        }

    }
}

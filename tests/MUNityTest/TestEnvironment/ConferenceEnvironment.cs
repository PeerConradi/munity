using System;
using System.Collections.Generic;
using System.Text;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Conference.Roles;
using MUNityCore.Models.User;
using MUNityCore.Models.Organization;

namespace MUNityTest.TestEnvironment
{
    class ConferenceEnvironment
    {
        public ConferenceEnvironment(MUNityCore.DataHandlers.EntityFramework.MunityContext context)
        {
            context.Organizations.Add(TestOrganization);
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
                        OrganizationName = "United Nations", OrganizationShort = "UN"
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
                        ProjectShort = "MUN",
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
                        ConferenceShort = "MUN 2021",
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

        private MunityUser _userMax;
        public MunityUser TestUserMax
        {
            get
            {
                if (_userMax == null)
                {
                    _userMax = new MunityUser
                    {
                        UserName = "maxmustermann",
                        Title = "",
                        Forename = "Max",
                        Lastname = "Mustermann",
                        NormalizedEmail = "max-mustermann@mail.com"
                    };
                }

                return _userMax;
            }
        }

        private MunityUser _userMike;
        public MunityUser TestUserMike
        {
            get
            {
                if (_userMike == null)
                {
                    _userMike = new MunityUser
                    {
                        UserName = "mikelitoris",
                        Title = "",
                        Forename = "Mike",
                        Lastname = "Litoris",
                        NormalizedEmail = "mike@litoris.com"
                    };
                }

                return _userMike;
            }
        }

        private MunityUser _userMillie;
        public MunityUser TestUserMillie
        {
            get
            {
                if (_userMillie == null)
                {
                    _userMillie = new MunityUser
                    {
                        UserName = "mbb",
                        Title = "",
                        Forename = "Millie Bobby",
                        Lastname = "Brown",
                        NormalizedEmail = "millie-brown@mail.com"
                    };
                }

                return _userMillie;
            }
        }

        private MunityUser _userFinn;
        public MunityUser TestUserFinn
        {
            get
            {
                if (_userFinn == null)
                {
                    _userFinn = new MunityUser
                    {
                        UserName = "f.wolfhard",
                        Title = "",
                        Forename = "Finn",
                        Lastname = "Wolfhard",
                        NormalizedEmail = "finn@mail.com"
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

        private ConferenceTeamRole _teamRoleProjectLeader;

        public ConferenceTeamRole TestRoleProjectLeader
        {
            get
            {
                if (_teamRoleProjectLeader == null)
                {
                    _teamRoleProjectLeader = new ConferenceTeamRole
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

        private ConferenceTeamRole _teamRoleSecretaryLeader;
        public ConferenceTeamRole TestRoleSecretaryLeader
        {
            get
            {
                if (_teamRoleSecretaryLeader == null)
                {
                    _teamRoleSecretaryLeader = new ConferenceTeamRole
                    {
                        Conference = TestConference, ParentTeamRole = TestRoleProjectLeader
                    };
                }

                return _teamRoleSecretaryLeader;
            }
        }

        private ConferencePressRole _pressRole;

        public ConferencePressRole TestPressRole
        {
            get
            {
                if (_pressRole == null)
                {
                    _pressRole = new ConferencePressRole
                    {
                        RoleName = "Zeitungs-Journalist",
                        PressCategory = ConferencePressRole.EPressCategories.Print,
                        Conference = TestConference,
                        RoleAuth = RoleAuthPesant
                    };
                }

                return _pressRole;
            }
        }

    }
}

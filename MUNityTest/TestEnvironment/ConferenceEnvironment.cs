using System;
using System.Collections.Generic;
using System.Text;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Organisation;

namespace MUNityTest.TestEnvironment
{
    class ConferenceEnvironment
    {
        private static Organisation _organisation;
        public static Project _project;
        private static Conference _conference;
        

        public static Organisation TestOrganisation
        {
            get
            {
                if (_organisation == null)
                {
                    _organisation = new Organisation();
                    _organisation.OrganisationName = "United Nations";
                    _organisation.OrganisationAbbreviation = "UN";
                }
                return _organisation;
            }
        }

        public static Project TestProject
        {
            get
            {
                if (_project == null)
                {
                    _project = new Project();
                    _project.ProjectName = "Model United Nations";
                    _project.ProjectAbbreviation = "MUN";
                    _project.ProjectOrganisation = TestOrganisation;
                }

                return _project;
            }
        }

        public static Conference TestConference
        {
            get
            {
                if (_conference == null)
                {
                    _conference = new Conference();
                    _conference.Name = "Model United Nations 2021";
                    _conference.Abbreviation = "MUN 2021";
                    _conference.FullName = "Official Model United Nations 2021";
                    _conference.StartDate = new DateTime(2021,01,01);
                    _conference.EndDate = new DateTime(2021,12,31);
                    _conference.ConferenceProject = TestProject;
                }

                return _conference;
            }
        }


    }
}

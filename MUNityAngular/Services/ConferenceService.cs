using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using MySql.Data.MySqlClient;
using MUNityAngular.Models.User;
using MUNityAngular.DataHandlers.EntityFramework;
using System.Text.RegularExpressions;
using MUNityAngular.DataHandlers.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Organisation;
using MUNityAngular.Util.Extenstions;

namespace MUNityAngular.Services
{
    public class ConferenceService : IConferenceService
    {
        private ConferenceContext _context;


        

        public Organisation CreateOrganisation(string name, string abbreviation)
        {
            var organisation = new Organisation();

            organisation.OrganisationId = Guid.NewGuid().ToString();
            if (!_context.Organisations.Any(n => n.OrganisationId == abbreviation))
                organisation.OrganisationId = abbreviation;

            organisation.OrganisationName = name;
            organisation.OrganisationAbbreviation = abbreviation;
            _context.Organisations.Add(organisation);
            _context.SaveChangesAsync();
            return organisation;
        }

        public Task<Organisation> GetOrganisation(string id)
        {
            return _context.Organisations.FirstOrDefaultAsync(n => n.OrganisationId == id);
        }

        public Project CreateProject(string name, string abbreviation, Organisation organisation)
        {
            var project = new Project
            {
                ProjectName = name,
                ProjectAbbreviation = abbreviation,
                ProjectOrganisation = organisation
            };

            if (!_context.Projects.Any(n => n.ProjectId == abbreviation))
                project.ProjectId = abbreviation;

            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public Task<Project> GetProject(string id)
        {
            return _context.Projects.FirstOrDefaultAsync(n => n.ProjectId == id);
        }

        public Conference CreateConference(string name, string fullname, string abbreviation, Project project)
        {
            var conference = new Conference();
            conference.Name = name;
            conference.FullName = fullname;
            conference.Abbreviation = abbreviation;
            if (!_context.Conferences.Any(n => n.ConferenceId == abbreviation))
                conference.ConferenceId = abbreviation;
            conference.ConferenceProject = project;

            _context.Conferences.Add(conference);
            _context.SaveChanges();

            return conference;
        }

        public Task<Conference> GetConference(string id)
        {
            return _context.Conferences.Include(n => n.Committees).FirstOrDefaultAsync(n => n.ConferenceId == id);
        }

        public Committee CreateCommittee(Conference conference, string name, string fullname, string abbreviation)
        {
            if (conference == null)
                throw new ArgumentException("The conference can not be null, make sure you give a valid conference when creating a committee.");

            var committee = new Committee();
            committee.Name = name;
            committee.FullName = fullname;
            committee.Abbreviation = abbreviation;

            

            string customid = conference.ConferenceId + "-" + abbreviation.ToUrlValid();
            if (!_context.Committees.Any(n => n.CommitteeId == customid))
                committee.CommitteeId = customid;


            committee.Conference = conference;
            _context.Committees.Add(committee);

            _context.SaveChanges();

            return committee;

        }

        public Task<Committee> GetCommittee(string id)
        {
            return _context.Committees.FirstOrDefaultAsync(n => n.CommitteeId == id);
        }

        public ConferenceService(ConferenceContext context)
        {
            this._context = context;
            Console.WriteLine("Conference-Service Started!");
        }
    }
}

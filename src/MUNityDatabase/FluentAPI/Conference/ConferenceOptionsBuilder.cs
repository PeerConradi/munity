using System;
using System.Linq;
using MUNity.Database.Context;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.User;

namespace MUNity.Database.FluentAPI
{
    public class ConferenceOptionsBuilder
    {
        private Conference _conference;

        private readonly MunityContext _context;

        public Conference Conference
        {
            get
            {
                this._conference.CreationDate = DateTime.Now;
                return _conference;
            }
        }

        public ConferenceOptionsBuilder WithName(string name)
        {
            this._conference.Name = name;
            return this;
        }

        public ConferenceOptionsBuilder WithFullName(string fullName)
        {
            this._conference.FullName = fullName;
            return this;
        }

        public ConferenceOptionsBuilder WithShort(string shortName)
        {
            this._conference.ConferenceShort = shortName;
            return this;
        }

        public ConferenceOptionsBuilder WithStartDate(DateTime startDate)
        {
            this._conference.StartDate = startDate;
            return this;
        }

        public ConferenceOptionsBuilder WithEndDate(DateTime endDate)
        {
            this._conference.EndDate = endDate;
            return this;
        }

        public ConferenceOptionsBuilder WithBasePrice(decimal price)
        {
            this._conference.GeneralParticipationCost = price;
            return this;
        }

        public ConferenceOptionsBuilder ByUser(MunityUser user)
        {
            this._conference.CreationUser = user;
            return this;
        }

        public ConferenceOptionsBuilder WithProject(string projectId)
        {
            var project = _context.Projects.FirstOrDefault(n => n.ProjectId == projectId);
            if (project == null)
                throw new NullReferenceException($"The given Project {projectId} was not found. Make sure it exists.");

            Conference.ConferenceProject = project;
            return this;
        }

        public ConferenceOptionsBuilder WithCommittee(Action<CommitteeOptionsBuilder> builder)
        {
            var options = new CommitteeOptionsBuilder();
            builder(options);
            this._conference.Committees.Add(options.Committee);
            return this;
        }

        public ConferenceOptionsBuilder(MunityContext context, Project project = null)
        {
            this._conference = new Conference();
            this._context = context;
            this._conference.ConferenceProject = project;
        }
    }
}

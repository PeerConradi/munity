﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;
using MUNityAngular.Models.Organisation;

namespace MUNityAngular.Services
{
    public interface IConferenceService
    {
        Organisation CreateOrganisation(string name, string abbreviation);

        Task<Organisation> GetOrganisation(string id);

        Project CreateProject(string name, string abbreviation, Organisation organisation);

        Task<Project> GetProject(string id);

        Conference CreateConference(string name, string fullname, string abbreviation, Project project);

        Task<Conference> GetConference(string id);

        Committee CreateCommittee(Conference conference, string name, string fullname, string abbreviation);

        Task<Committee> GetCommittee(string id);


    }
}
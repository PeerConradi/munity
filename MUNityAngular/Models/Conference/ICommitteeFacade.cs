﻿namespace MUNityAngular.Models.Conference
{
    public interface ICommitteeFacade
    {
        public string CommitteeId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public string Article { get; set; }

        public string ResolutlyCommitteeId { get; }
    }
}
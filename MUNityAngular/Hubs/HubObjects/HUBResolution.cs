using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Util.Extenstions;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBResolution is a one on one copy of the ResolutionModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
    public class HUBResolution
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Topic { get; set; }

        public string AgendaItem { get; set; }

        public string Session { get; set; }

        public string SubmitterName { get; set; }

        public string CommitteeName { get; set; }

        public DateTime Date { get; set; }

        public HUBPreamble Preamble { get; set; }

        public List<HUBChangeAmendment> ChangeAmendments { get; set; }

        public List<HUBDeleteAmendment> DeleteAmendments { get; set; }

        public List<HUBMoveAmendment> MoveAmendments { get; set; }

        public List<HUBAddAmendment> AddAmendmentsSave { get; set; }

        public List<HUBOperativeParagraph> OperativeSections { get; set; }

        public List<string> SupporterNames { get; set; }

        public HUBResolution(ResolutionModel resolution)
        {
            this.ID = resolution.ID;
            this.Name = resolution.Name;
            this.FullName = resolution.FullName;
            this.Topic = resolution.Topic;
            this.AgendaItem = resolution.AgendaItem;
            this.Session = resolution.Session;
            this.Date = resolution.Date;
            this.Preamble = new HUBPreamble(resolution.Preamble);
            ChangeAmendments = resolution.ChangeAmendments.ToHubAmendments();
            this.DeleteAmendments = resolution.DeleteAmendments.ToHubAmendments();
            this.MoveAmendments = resolution.MoveAmendments.ToHubAmendments();
            this.AddAmendmentsSave = resolution.AddAmendmentsSave.ToHubAmendments();
            this.OperativeSections = resolution.OperativeSections.ToHubParagraphs();
            this.SubmitterName = SubmitterName;
            this.SupporterNames = resolution.SupporterNames;
            this.CommitteeName = resolution.CommitteeName;
        }

        public HUBResolution()
        {
            ChangeAmendments = new List<HUBChangeAmendment>();
            DeleteAmendments = new List<HUBDeleteAmendment>();
            MoveAmendments = new List<HUBMoveAmendment>();
            AddAmendmentsSave = new List<HUBAddAmendment>();
            OperativeSections = new List<HUBOperativeParagraph>();
            Preamble = new HUBPreamble();
        }
    }
}

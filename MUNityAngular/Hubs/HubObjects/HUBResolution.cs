using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBResolution
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Topic { get; set; }
        public string AgendaItem { get; set; }

        public string Session { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<Models.ChangeAmendmentModel> ChangeAmendments { get; set; }

        public IEnumerable<Models.DeleteAmendmentModel> DeleteAmendments { get; set; }

        public IEnumerable<Models.MoveAmendment> MoveAmendments { get; set; }

        public IEnumerable<Models.AddAmendmentModel> AddAmendmentsSave { get; set; }

        public List<HUBOperativeParagraph> OperativeSections { get; set; }

        public HUBResolution(Models.ResolutionModel resolution)
        {
            this.ID = resolution.ID;
            this.Name = resolution.Name;
            this.FullName = resolution.FullName;
            this.Topic = resolution.Topic;
            this.AgendaItem = resolution.AgendaItem;
            this.Session = resolution.Session;
            this.Date = resolution.Date;
            this.ChangeAmendments = resolution.ChangeAmendments;
            this.DeleteAmendments = resolution.DeleteAmendments;
            this.MoveAmendments = resolution.MoveAmendments;
            this.AddAmendmentsSave = resolution.AddAmendmentsSave;
            
            this.OperativeSections = new List<HUBOperativeParagraph>();
            foreach(var oa in resolution.OperativeSections)
            {
                OperativeSections.Add(new HUBOperativeParagraph(oa));
            }
        }
    }
}

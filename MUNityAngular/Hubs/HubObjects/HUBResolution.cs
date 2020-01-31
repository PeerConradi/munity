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

        public List<HUBChangeAmendment> ChangeAmendments { get; set; }

        public List<HUBDeleteAmendment> DeleteAmendments { get; set; }

        public List<HUBMoveAmendment> MoveAmendments { get; set; }

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
            ChangeAmendments = new List<HUBChangeAmendment>();
            foreach(var amendment in resolution.ChangeAmendments)
            {
                ChangeAmendments.Add(new HUBChangeAmendment(amendment));
            }
            this.DeleteAmendments = new List<HUBDeleteAmendment>();
            foreach(var amendment in resolution.DeleteAmendments)
            {
                this.DeleteAmendments.Add(new HUBDeleteAmendment(amendment));
            }
            this.MoveAmendments = new List<HUBMoveAmendment>();
            foreach(var amendment in resolution.MoveAmendments)
            {
                this.MoveAmendments.Add(new HUBMoveAmendment(amendment));
            }
            this.AddAmendmentsSave = resolution.AddAmendmentsSave;
            
            this.OperativeSections = new List<HUBOperativeParagraph>();
            foreach(var oa in resolution.OperativeSections)
            {
                OperativeSections.Add(new HUBOperativeParagraph(oa));
            }
        }
    }
}

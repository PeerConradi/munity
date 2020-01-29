﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBOperativeParagraph
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsLocked { get; set; }
        public bool IsVirtual { get; set; }
        public string Text { get; set; }
        public bool Visible { get; set; }
        public string ParentID { get; set; }
        public string ResolutionID { get; set; }
        public bool AmendmentParagraph { get; set; }

        public IEnumerable<string> Children { get; set; }

        public int DeleteAmendmentCount { get; set; }

        public int ChangeAmendmentCount { get; set; }

        public int MoveAmendmentCount { get; set; }

        public HUBOperativeParagraph(Models.OperativeParagraphModel oa)
        {
            this.ID = oa.ID;
            this.Name = oa.Name;
            this.IsLocked = oa.IsLocked;
            this.IsVirtual = oa.IsVirtual;
            this.Text = oa.Text;
            this.Visible = oa.Visible;
            this.ParentID = oa.ParentID;
            this.ResolutionID = oa.ResolutionID;
            this.AmendmentParagraph = oa.AmendmentParagraph;
            this.Children = oa.Children;
            this.Path = oa.Path;
            this.DeleteAmendmentCount = oa.DeleteAmendmentCount;
            this.ChangeAmendmentCount = oa.ChangeAmendmentCount;
            this.MoveAmendmentCount = oa.MoveAmendmentCount;
        }
    }
}

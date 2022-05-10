using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Resolution;

public class ResaOperativeParagraph
{
    public string ResaOperativeParagraphId { get; set; }

    public string Name { get; set; } = "";

    public string Text { get; set; } = "";

    public bool IsLocked { get; set; } = false;

    public bool IsVirtual { get; set; } = false;

    public bool Visible { get; set; } = true;

    public bool Corrected { get; set; } = false;

    public IList<ResaOperativeParagraph> Children { get; set; }

    public string Comment { get; set; } = "";

    public int OrderIndex { get; set; }

    public bool AllowAmendments { get; set; } = true;

    public ResaElement Resolution { get; set; }

    public ResaOperativeParagraph Parent { get; set; }

    public IList<ResaDeleteAmendment> DeleteAmendments { get; set; }
    public IList<ResaChangeAmendment> ChangeAmendments { get; set; }

    public IList<ResaMoveAmendment> MoveAmendments { get; set; }

    public int GetIndex()
    {
        int index = 0;
        if (Parent == null && Resolution != null)
        {
            foreach (var paragraph in Resolution.OperativeParagraphs)
            {
                if (paragraph == this) break;
                if (!paragraph.IsVirtual) index++;
            }
            return index;
        }
        else if (Parent != null)
        {
            foreach (var child in Parent.Children)
            {
                if (child == this) break;
                if (!child.IsVirtual) index++;
            }
            return index;
        }

        return -1;
    }

    public ResaOperativeParagraph()
    {
        ResaOperativeParagraphId = Guid.NewGuid().ToString();
        Children = new List<ResaOperativeParagraph>();
        DeleteAmendments = new List<ResaDeleteAmendment>();
        ChangeAmendments = new List<ResaChangeAmendment>();
        MoveAmendments = new List<ResaMoveAmendment>();
    }

    public override string ToString()
    {
        return Text;
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Base;
using MUNity.Database.Models.Conference;

namespace MUNity.Database.Models.Conference;

public class ConferenceApplicationFormula
{
    public int ConferenceApplicationFormulaId { get; set; }

    public ConferenceApplicationOptions Options { get; set; }

    public ConferenceApplicationFormulaTypes FormulaType { get; set; }

    public string PreContent { get; set; }

    public string PostContent { get; set; }

    public string Title { get; set; }

    /// <summary>
    /// Should the user that applies for this role have a Fore and Lastname set
    /// </summary>
    public bool RequiresName { get; set; }

    /// <summary>
    /// Should the user that applies have an address set in their profile
    /// </summary>
    public bool RequiresAddress { get; set; }

    /// <summary>
    /// Is the user required to input a school into their profile.
    /// </summary>
    public bool RequiresSchool { get; set; }

    public int? MaxWishes { get; set; }

    public ICollection<ConferenceApplicationField> Fields { get; set; }
}

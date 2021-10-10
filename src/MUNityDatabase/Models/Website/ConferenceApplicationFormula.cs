using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;
using MUNityBase;

namespace MUNity.Database.Models.Website
{
    public class ConferenceApplicationFormula
    {
        public int ConferenceApplicationFormulaId { get; set; }

        public Conference.Conference Conference { get; set; }

        public ConferenceApplicationFormulaTypes FormulaType { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ApplicationStartDate { get; set; }

        public DateTime? ApplicationEndDate { get; set; }


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

        public byte? MaxDelegationWishes { get; set; }

        public ICollection<ConferenceApplicationField> Fields { get; set; }
    }

    public class ConferenceApplicationField
    {

        public int ConferenceApplicationFieldId { get; set; }

        public ConferenceApplicationFormula Forumula { get; set; }

        public string FieldName { get; set; }

        public string FieldDescription { get; set; }

        public bool IsRequired { get; set; }

        public ConferenceApplicationFieldTypes FieldType { get; set; }

        public string DefaultValue { get; set; }
    }

    public class ConferenceDelegationApplicationFieldInput
    {
        public long ConferenceDelegationApplicationFieldInputId { get; set; }

        public DelegationApplication Application { get; set; }

        public ConferenceApplicationField Field { get; set; }

        public string Value { get; set; }

        public string ValueSecondary { get; set; }
    }
}

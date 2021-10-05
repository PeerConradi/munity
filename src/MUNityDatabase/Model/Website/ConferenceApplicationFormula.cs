using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference;
using MUNityBase;

namespace MUNity.Database.Model.Website
{
    public class ConferenceApplicationFormula
    {
        public int ConferenceApplicationFormulaId { get; set; }

        public Models.Conference.Conference Conference { get; set; }

        public ConferenceApplicationFormulaTypes FormulaType { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ApplicationStartDate { get; set; }

        public DateTime? ApplicationEndDate { get; set; }


        public string PreContent { get; set; }

        public string PostContent { get; set; }

        public string Title { get; set; }

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
    }
}

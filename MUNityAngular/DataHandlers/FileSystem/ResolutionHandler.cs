using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.FileSystem
{
    public class ResolutionHandler
    {

        

        public static Models.ResolutionModel ExampleResolution
        {
            get
            {
                var resolution = new Models.ResolutionModel("test_resolution");
                //Settings
                resolution.Name = "Example";
                resolution.FullName = "Example Resolution";
                resolution.Topic = "Example";
                resolution.AgendaItem = "3a";
                resolution.Session = "4";
                resolution.Conference = Database.ConferenceHandler.TestConference;
                resolution.ResolutelyCommittee = Database.CommitteeHandler.TestCommittee;
                resolution.Supporters.Add(Database.DelegationHandler.AllDefaultDelegations()[0]);
                resolution.Supporters.Add(Database.DelegationHandler.AllDefaultDelegations()[1]);
                resolution.Submitter = Database.DelegationHandler.AllDefaultDelegations()[2];
                resolution.Date = new DateTime(1995, 11, 7, 22, 30, 00);

                //Preamble
                var preambleOne = resolution.Preamble.AddParagraph("Präambel Paragraph 1");
                preambleOne.ID = "preamble_paragraph_one";
                var preambleTwo = resolution.Preamble.AddParagraph("Präambel Paragraph 2");
                preambleTwo.ID = "preamble_paragraph_two";

                var secionOne = resolution.AddOperativeParagraph("operative Paragraph 1");
                secionOne.ID = "operative_paraphraph_one";
                var sectionTwo = resolution.AddOperativeParagraph("operative Parahraph 2");
                sectionTwo.ID = "operative_paragraph_two";

                var changeAmendment = new Models.ChangeAmendmentModel();
                changeAmendment.TargetSection = secionOne;
                changeAmendment.SubmitTime = new DateTime(1995, 11, 7, 22, 32, 40);
                changeAmendment.SubmitterID = Database.DelegationHandler.AllDefaultDelegations()[0].ID;
                changeAmendment.NewText = "Das ist der neue Text";

                var deleteAmendment = new Models.DeleteAmendmentModel();
                deleteAmendment.TargetSection = secionOne;
                deleteAmendment.SubmitterID = Database.DelegationHandler.AllDefaultDelegations()[0].ID;
                deleteAmendment.SubmitTime = new DateTime(1995, 11, 7, 22, 31, 32);

                var moveAmendment = new Models.MoveAmendment();
                moveAmendment.TargetSection = secionOne;
                moveAmendment.NewPosition = 1;
                moveAmendment.SubmitterID = Database.DelegationHandler.AllDefaultDelegations()[0].ID;
                moveAmendment.SubmitTime = new DateTime(1995, 11, 7, 22, 32, 30);

                var addAmendment = new Models.AddAmendmentModel();
                addAmendment.TargetResolution = resolution;
                addAmendment.TargetPosition = 0;
                addAmendment.SubmitterID = Database.DelegationHandler.AllDefaultDelegations()[0].ID;
                addAmendment.SubmitTime = new DateTime(1995, 11, 7, 22, 33, 44);
                addAmendment.NewText = "Text welcher in dem neuen Absatz stehen soll.";
                
                return resolution;
            }
        }
    }
}

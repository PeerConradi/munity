using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class ConferenceHandler
    {
        private static Models.ConferenceModel _testConference;

        public static Models.ConferenceModel GetConference(string id)
        {
            if (id == TestConference.ID)
                return TestConference;
            return null;

        }

        public static Models.ConferenceModel TestConference
        {
            get
            {
                if (_testConference == null)
                {
                    _testConference = new Models.ConferenceModel();
                    _testConference.ID = "TESTCONFERENCE";
                    _testConference.Name = "Test";
                    _testConference.FullName = "Test Conference";
                    _testConference.AddCommittee(CommitteeHandler.TestCommittee);
                }

                return _testConference;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MUNityTest
{
    public class ConnectionInfo
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public CMunityMongoDatabaseSettings MunityMongoDatabaseSettings { get; set; }

        public class CMunityMongoDatabaseSettings
        {
            public string ConnectionString { get; set; }

            public string DatabaseName { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class PrincipalInfo
        {
            public String Name
            {
                get;
                set;
            }

            public String Telephone
            {
                get;
                set;
            }

            public String Email
            {
                get;
                set;
            }

            public Dictionary<String, String> Others
            {
                get;
                set;
            }

            public PrincipalInfo()
            {
                Others = new Dictionary<string, string>();
            }
        }
    };
};

using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class PrincipalInfo
    {
        private String name;
        private String telephone;
        private String email;
        private Dictionary<String, String> others;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public Dictionary<String, String> Others
        {
            get { return others; }
            set { others = value; }
        }

        public PrincipalInfo()
        {
            others = new Dictionary<string, string>();
        }
    }
}

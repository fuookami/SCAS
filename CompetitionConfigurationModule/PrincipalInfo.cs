using System;

namespace CompetitionConfigurationModule
{
    public class PrincipalInfo
    {
        private String name;
        private String telephone;
        private String email;

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
    }
}

using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterFormID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationRegisterForm-{0}", ID);
        }
    }

    public class OrganizationRegisterFormValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string SID { get; internal set; }
        public string OrgID { get; internal set; }
        public string RegionID { get; internal set; }
    }

    public class OrganizationRegisterForm
        : DomainAggregateRoot<OrganizationRegisterFormValue, OrganizationRegisterFormID>
    {
        public string SID { get; }

        public Organization Org { get; }
        public Region RegisteredRegion { get; }

        public OrganizationRegisterFormInfo Info { get; }
        public OrganizationRegisterFormExaminiation Examination { get; }

        public bool Examined { get { return Examination != null; } }
        public bool Approved { get { return Examination.Approved; } }
    }
}

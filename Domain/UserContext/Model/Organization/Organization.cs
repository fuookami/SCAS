using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class OrganizationID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("Organization-{0}", ID);
        }
    }

    public struct OrganizationValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string SID { get; internal set; }
    }

    public partial class Organization
        : DomainAggregateRoot<OrganizationValue, OrganizationID>
    {
        // 在当前域下的识别码，由系统使用者给定
        public string SID { get; }
        
        // 组织信息
        public OrganizationInfo Info { get; }

        internal Organization(string sid, string name)
        {
            SID = sid;
            Info = new OrganizationInfo(id, name);
        }

        internal Organization(OrganizationID id, string sid, OrganizationInfo info)
            : base(id)
        {
            SID = sid;
            Info = info;
        }

        public override OrganizationValue ToValue()
        {
            return new OrganizationValue
            {
                ID = this.ID, 
                SID = this.SID
            };
        }
    }
}

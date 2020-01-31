using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class OrganizationInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationInfo-{0}", ID);
        }
    }

    public struct OrganizationInfoValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string OrgID { get; internal set; }

        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }

    public class OrganizationInfo
        : DomainAggregateChild<OrganizationInfoValue, OrganizationInfoID, OrganizationID>
    {
        // 组织系统识别码
        internal string OrgID { get { return pid.ID; } }

        // 组织名
        public string Name { get; }
        // 描述
        public string Description { get; internal set; }

        internal OrganizationInfo(OrganizationID pid, string name)
            : base(pid)
        {
            Name = name;
        }

        internal OrganizationInfo(OrganizationID pid, OrganizationInfoID id, string name, string description = null)
            : base(pid, id)
        {
            Name = name;
            Description = description;
        }

        public override OrganizationInfoValue ToValue()
        {
            return new OrganizationInfoValue
            {
                ID = this.ID, 
                OrgID = this.OrgID, 
                Name = this.Name, 
                Description = this.Description
            };
        }
    }
}

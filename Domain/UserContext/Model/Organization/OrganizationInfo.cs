using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

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

    public class OrganizationInfoValue
        : DomainEntityValueBase
    {
        [NotNull] public string OrgID { get; internal set; }

        [NotNull] public string Name { get; internal set; }
        [NotNull] public string Description { get; internal set; }
    }

    public class OrganizationInfo
        : DomainAggregateChild<OrganizationInfoValue, OrganizationInfoID, OrganizationID>
    {
        // 组织系统识别码
        [NotNull] public string OrgID => pid.ID;

        // 组织名
        [DisallowNull] public string Name { get; internal set; }
        // 描述
        [DisallowNull] public string Description { get; internal set; }

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
            return base.ToValue(new OrganizationInfoValue
            {
                OrgID = this.OrgID,
                Name = this.Name,
                Description = this.Description
            });
        }
    }
}

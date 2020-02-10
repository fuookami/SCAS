using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

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

    public class OrganizationValue
        : DomainAggregateRootValueBase
    {
        [NotNull] public string SID { get; internal set; }
    }

    public partial class Organization
        : DomainAggregateRoot<OrganizationValue, OrganizationID>
    {
        // 在当前域下的识别码，由系统使用者给定
        [DisallowNull] public string SID { get; }

        // 组织信息
        [DisallowNull] public OrganizationInfo Info { get; }

        internal Organization(string sid, string name)
        {
            SID = sid;
            Info = new OrganizationInfo(id, name);
        }

        internal Organization(OrganizationID id, bool archived, string sid, OrganizationInfo info)
            : base(id, archived)
        {
            SID = sid;
            Info = info;
        }

        public new bool Archive()
        {
            return base.Archive();
        }

        public override OrganizationValue ToValue()
        {
            return base.ToValue(new OrganizationValue
            {
                SID = this.SID
            });
        }
    }
}

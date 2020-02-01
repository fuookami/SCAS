using SCAS.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public class RegionInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("RegionInfo-{0}", ID);
        }
    }

    public class RegionInfoValue
        : DomainEntityValueBase
    {
        [NotNull] public string RegionID { get; internal set; }

        [NotNull] public string Name { get; internal set; }
        [NotNull] public string Description { get; internal set; }
    }

    // 域信息
    public class RegionInfo
        : DomainAggregateChild<RegionInfoValue, RegionInfoID, RegionID>
    {
        // 域系统识别码
        [NotNull] public string RegionID { get { return pid.ID; } }

        // 域名
        [DisallowNull] public string Name { get; internal set; }
        // 域描述
        [DisallowNull] public string Description { get; internal set; }

        internal RegionInfo(RegionID pid, string name)
            : base(pid)
        {
            Name = name;
        }

        internal RegionInfo(RegionID pid, RegionInfoID id, string name, string description = null)
            : base(pid, id)
        {
            Name = name;
            Description = description;
        }

        public override RegionInfoValue ToValue()
        {
            return base.ToValue(new RegionInfoValue
            {
                RegionID = this.RegionID,
                Name = this.Name,
                Description = this.Description
            });
        }
    }
}

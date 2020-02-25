using SCAS.Module;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    public class RegionInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("RegionInfo-{0}", ID);
        }

        public RegionInfoID() { }
        internal RegionInfoID(string id) : base(id) { }
    }

    public class RegionInfoValue
        : DomainEntityValueBase
    {
        [NotNull] public string RegionID { get; internal set; }

        [NotNull] public string Name { get; internal set; }
        [NotNull] public string Description { get; internal set; }
        [NotNull] public IReadOnlyList<string> Tags { get; internal set; }
    }

    // 域信息
    public class RegionInfo
        : DomainAggregateChild<RegionInfoValue, RegionInfoID, RegionID>
    {
        // 域系统识别码
        [NotNull] public string RegionID => pid.ID;

        // 域名
        [DisallowNull] public string Name { get; internal set; }
        // 域描述
        [DisallowNull] public string Description { get; internal set; }
        // 域标签
        [DisallowNull] internal List<string> TagsList { get; }
        [NotNull] public IReadOnlyList<string> Tags => TagsList;

        internal RegionInfo(RegionID pid, string name)
            : base(pid)
        {
            Name = name;
            TagsList = new List<string>();
        }

        internal RegionInfo(RegionID pid, RegionInfoID id, string name, string description = null, IReadOnlyList<string> tags = null)
            : base(pid, id)
        {
            Name = name;
            Description = description;
            TagsList = tags?.ToList() ?? new List<string>();
        }

        public RegionInfo Snapshoot()
        {
            return new RegionInfo(pid, id, Name, Description, TagsList.ToList());
        }

        public RegionInfo Refresh(string name = null, string description = null, IReadOnlyCollection<string> tags = null)
        {
            if (name != null)
            {
                Name = name;
            }
            if (description != null)
            {
                Description = description;
            }
            if (tags != null)
            {
                TagsList.Clear();
                TagsList.AddRange(tags);
            }
            return this;
        }

        public override RegionInfoValue ToValue()
        {
            return base.ToValue(new RegionInfoValue
            {
                RegionID = this.RegionID,
                Name = this.Name,
                Description = this.Description,
                Tags = new List<string>(Tags)
            });
        }
    }
}

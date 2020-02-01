using SCAS.Utils;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public class RegionID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("Region-{0}", ID);
        }
    }

    public class RegionValue
        : DomainEntityValueBase
    {
        public string ParentRegionID { get; internal set; }
    };

    // 域
    public abstract class Region
        : DomainAggregateRoot<RegionValue, RegionID>
    {
        // 父域
        public Region ParentRegion { get; }

        // 域信息
        [DisallowNull] public RegionInfo Info { get; }

        public bool BeRoot { get { return ParentRegion == null; } }

        internal Region(string name, Region parentRegion = null)
        {
            ParentRegion = parentRegion;
            Info = new RegionInfo(this.id, name);
        }

        internal Region(RegionID id, RegionInfo info, Region parentRegion = null)
            : base(id)
        {
            ParentRegion = parentRegion;
            Info = info;
        }

        public override RegionValue ToValue()
        {
            return base.ToValue(new RegionValue
            {
                ParentRegionID = this.ParentRegion?.ID
            });
        }
    };
}

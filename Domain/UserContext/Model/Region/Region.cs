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
        public RegionType Type { get; internal set; }
        public string ParentRegionID { get; internal set; }
    };

    // 域
    public class Region
        : DomainAggregateRoot<RegionValue, RegionID>
    {
        // 类型
        RegionType Type { get; }
        // 父域
        public Region ParentRegion { get; }

        // 域信息
        [DisallowNull] public RegionInfo Info { get; }

        // 是否允许个人注册
        public bool IndependentPersonalityAllowed { get { return RegionTypeTrait.IndependentPersonalityAllowed(Type); } }
        // 是否允许组织注册
        public bool OrganizationAllowed { get { return RegionTypeTrait.OrganizationAllowed(Type); } }

        public bool BeRoot { get { return ParentRegion == null; } }

        internal Region(RegionType type, string name, Region parentRegion = null)
        {
            Type = type;
            ParentRegion = parentRegion;
            Info = new RegionInfo(this.id, name);
        }

        internal Region(RegionID id, RegionType type, RegionInfo info, Region parentRegion = null)
            : base(id)
        {
            Type = type;
            ParentRegion = parentRegion;
            Info = info;
        }

        public bool RegisterValid(PersonRegisterForm register)
        {
            return RegionTypeTrait.Allow(Type, register);
        }

        public bool RegisterValid(OrganizationRegisterForm register)
        {
            return RegionTypeTrait.Allow(Type, register);
        }

        public override RegionValue ToValue()
        {
            return base.ToValue(new RegionValue
            {
                Type = this.Type, 
                ParentRegionID = this.ParentRegion?.ID
            });
        }
    };
}

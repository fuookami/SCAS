using SCAS.Utils;
using SCAS.Module;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationRegister-{0}", ID);
        }
    }

    public class OrganizationRegisterValue
        : DomainEntityValueBase
    {
        [NotNull] public string SID { get; internal set; }
        [NotNull] public string OrgID { get; internal set; }
        [NotNull] public string RegionID { get; internal set; }
    }

    // 组织注册表
    public class OrganizationRegister
        : DomainAggregateRoot<OrganizationRegisterValue, OrganizationRegisterID>
    {
        // 在当前域的应用识别码，由系统使用者给定规则
        [DisallowNull] public string SID { get; }

        // 组织
        [DisallowNull] public Organization Org { get; }
        // 注册的域
        [DisallowNull] public Region RegisteredRegion { get; }

        // 注册信息
        [DisallowNull] public OrganizationRegisterInfo Info { get; }

        internal OrganizationRegister(string sid, Organization org, Region region, uint prefixCode)
        {
            SID = sid;
            Org = org;
            RegisteredRegion = region;
            Info = new OrganizationRegisterInfo(this.id, prefixCode);
        }

        internal OrganizationRegister(OrganizationRegisterID id, string sid, Organization org, Region region, OrganizationRegisterInfo info)
            : base(id)
        {
            SID = sid;
            Org = org;
            RegisteredRegion = region;
            Info = info;
        }

        public override OrganizationRegisterValue ToValue()
        {
            return base.ToValue(new OrganizationRegisterValue
            {
                SID = this.SID,
                OrgID = this.Org.ID,
                RegionID = this.RegisteredRegion.ID
            });
        }

        public struct OrganizationRegisters
            : IDomainValue
        {
            private Dictionary<Region, OrganizationRegister> registers;

            // 已注册的域
            public IReadOnlyList<Region> RegisteredRegion { get { return registers.Keys.ToList(); } }

            public Try<uint> PrefixCode(Region region)
            {
                return registers.ContainsKey(region) 
                    ? new Try<uint>(registers[region].Info.PrefixCode)
                    : new Try<uint>();
            }

            internal OrganizationRegisters(IReadOnlyList<OrganizationRegister> registersList)
            {
                registers = registersList.ToDictionary(reg => reg.RegisteredRegion);
            }
        }
    }
}

using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string SId { get; internal set; }
        public string OrgId { get; internal set; }
        public string RegionId { get; internal set; }
    }

    // 组织注册表
    public class OrganizationRegister
        : IDomainAggregate<OrganizationRegisterValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 在当前域的应用识别码，由系统使用者给定规则
        public string SId { get; }
        // 组织系统识别码
        internal string OrgId { get; }

        // 注册的域
        public Region RegisteredRegion { get; }
        // 注册信息
        public OrganizationRegisterInfo Info { get; }

        public OrganizationRegisterValue ToValue()
        {
            return new OrganizationRegisterValue
            {
                Id = this.Id, 
                SId = this.SId, 
                OrgId = this.OrgId, 
                RegionId = this.RegisteredRegion.Id
            };
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

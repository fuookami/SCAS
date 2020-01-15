using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public partial class OrganizationRegister
    {
        public string Id { get; }
        public Organization Org { get; }

        // 注册的域
        public Region RegisteredRegion { get; }
        // 在当前域下的前缀码，用于秩序册的生成，由系统使用者给定，应为两位数字
        public uint Code { get; }

        public OrganizationRegisterInfo Info { get; internal set; }

        internal OrganizationRegister(Organization org, Region region, uint code)
            : this(Guid.NewGuid().ToString("N"), org, region, DateTime.Now, code) {}

        internal OrganizationRegister(string id, Organization org, Region region, DateTime time, uint code)
        {
            Id = id;
            Org = org;
            RegisteredRegion = region;
            Code = code;
        }
    }

    public struct OrganizationRegisterValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string OrgId { get; internal set; }
        public string RegisteredRegionId { get; internal set; }

        public DateTime RegisterdTime { get; internal set; }
        public uint Code { get; internal set; }
    };

    public partial class OrganizationRegister
        : IPersistentType<OrganizationRegisterValue>
    {
        public OrganizationRegisterValue ToValue()
        {
            return new OrganizationRegisterValue
            {
                Id = this.Id,
                OrgId = this.Org.Id,
                RegisteredRegionId = this.RegisteredRegion.Id,
                RegisterdTime = this.RegisterdTime,
                Code = this.Code
            };
        }
    };
}

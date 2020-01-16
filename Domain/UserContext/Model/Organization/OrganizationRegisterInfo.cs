using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterInfoValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string RegisterId { get; internal set; }

        public uint PrefixCode { get; internal set; }
        public DateTime RegisteredTime { get; internal set; }
    }

    public class OrganizationRegisterInfo
        : IDomainEntity<OrganizationRegisterInfoValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 注册表系统识别码
        internal string RegisterId { get; }

        // 在当前域的前缀码，由系统使用者给定规则
        public uint PrefixCode { get; }
        // 注册时间
        public DateTime RegisteredTime { get; }

        public OrganizationRegisterInfoValue ToValue()
        {
            return new OrganizationRegisterInfoValue
            {
                Id = this.Id,
                RegisterId = this.RegisterId,
                PrefixCode = this.PrefixCode, 
                RegisteredTime = this.RegisteredTime
            };
        }
    }
}

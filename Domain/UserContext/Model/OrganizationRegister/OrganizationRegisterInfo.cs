using SCAS.Utils;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationRegisterInfo-{0}", ID);
        }
    }

    public class OrganizationRegisterInfoValue
        : DomainEntityValueBase
    {
        [NotNull] public string RegisterID { get; internal set; }

        public uint PrefixCode { get; internal set; }
        public DateTime RegisteredTime { get; internal set; }
    }

    public class OrganizationRegisterInfo
        : DomainAggregateChild<OrganizationRegisterInfoValue, OrganizationRegisterInfoID, OrganizationRegisterID>
    {
        // 注册表系统识别码
        [NotNull] public string RegisterID { get { return pid.ID; } }

        // 在当前域的前缀码，由系统使用者给定规则
        public uint PrefixCode { get; }
        // 注册时间
        public DateTime RegisteredTime { get; }

        internal OrganizationRegisterInfo(OrganizationRegisterID pid, uint prefixCode)
            : base(pid)
        {
            PrefixCode = prefixCode;
            RegisteredTime = DateTime.Now;
        }

        internal OrganizationRegisterInfo(OrganizationRegisterID pid, OrganizationRegisterInfoID id, uint prefixCode, DateTime registeredTime)
            : base(pid, id)
        {
            PrefixCode = prefixCode;
            RegisteredTime = registeredTime;
        }

        public override OrganizationRegisterInfoValue ToValue()
        {
            return base.ToValue(new OrganizationRegisterInfoValue
            {
                RegisterID = this.RegisterID,
                PrefixCode = this.PrefixCode,
                RegisteredTime = this.RegisteredTime
            });
        }
    }
}

using SCAS.Utils;
using System;

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

    public struct OrganizationRegisterInfoValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string RegisterID { get; internal set; }

        public uint PrefixCode { get; internal set; }
        public DateTime RegisteredTime { get; internal set; }
        public string FormID { get; internal set; }
    }

    public class OrganizationRegisterInfo
        : DomainAggregateChild<OrganizationRegisterInfoValue, OrganizationRegisterInfoID, OrganizationRegisterID>
    {
        // 注册表系统识别码
        internal string RegisterID { get { return pid.ID; } }

        // 在当前域的前缀码，由系统使用者给定规则
        public uint PrefixCode { get; }
        // 注册时间
        public DateTime RegisteredTime { get; }
        // 审批表单
        public PersonRegisterForm Form { get; }

        internal OrganizationRegisterInfo(OrganizationRegisterID pid, uint prefixCode, PersonRegisterForm form)
            : base(pid)
        {
            PrefixCode = prefixCode;
            RegisteredTime = DateTime.Now;
            Form = form;
        }

        internal OrganizationRegisterInfo(OrganizationRegisterID pid, OrganizationRegisterInfoID id, uint prefixCode, DateTime registeredTime, PersonRegisterForm form)
            : base(pid, id)
        {
            PrefixCode = prefixCode;
            RegisteredTime = registeredTime;
            Form = form;
        }

        public override OrganizationRegisterInfoValue ToValue()
        {
            return new OrganizationRegisterInfoValue
            {
                ID = this.ID,
                RegisterID = this.RegisterID,
                PrefixCode = this.PrefixCode, 
                RegisteredTime = this.RegisteredTime,
                FormID = this.Form.ID
            };
        }
    }
}

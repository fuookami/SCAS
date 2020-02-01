using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterFormInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationRegisterFormInfo-{0}", ID);
        }
    }

    public class OrganizationRegisterFormInfoValue
        : RegisterFormInfoValueBase
    {
        public uint PrefixCode { get; internal set; }
    }

    // 组织注册表审批表单信息
    public class OrganizationRegisterFormInfo
        : RegisterFormInfoBase<OrganizationRegisterFormInfoValue, OrganizationRegisterFormInfoID, OrganizationRegisterFormID>
    {
        // 在当前域的前缀码，由系统使用者给定规则
        public uint PrefixCode { get; }

        internal OrganizationRegisterFormInfo(OrganizationRegisterFormID pid, Person initiator, uint prefixCode)
            : base(pid, initiator)
        {
            PrefixCode = prefixCode;
        }

        internal OrganizationRegisterFormInfo(OrganizationRegisterFormID pid, OrganizationRegisterFormInfoID id, Person initiator, uint prefixCode, DateTime initialTime)
            : base(pid, id, initiator, initialTime)
        {
            PrefixCode = prefixCode;
        }

        public override OrganizationRegisterFormInfoValue ToValue()
        {
            return base.ToValue(new OrganizationRegisterFormInfoValue 
            {
                PrefixCode = this.PrefixCode
            });
        }
    }
}

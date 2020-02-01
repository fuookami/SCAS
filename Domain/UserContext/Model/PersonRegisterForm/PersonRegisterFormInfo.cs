using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public class PersonRegisterFormInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonRegisterFormInfo-{0}", ID);
        }
    }

    public class PersonRegisterFormInfoValue
        : RegisterFormInfoValueBase
    {
    }

    // 个人注册表审批表单信息
    public class PersonRegisterFormInfo
        : RegisterFormInfoBase<PersonRegisterFormInfoValue, PersonRegisterFormInfoID, PersonRegisterFormID>
    {
        internal PersonRegisterFormInfo(PersonRegisterFormID pid, Person initiator)
            : base(pid, initiator)
        {
        }

        internal PersonRegisterFormInfo(PersonRegisterFormID pid, PersonRegisterFormInfoID id, Person initiator, DateTime initialTime)
            : base(pid, id, initiator, initialTime)
        {
        }

        public override PersonRegisterFormInfoValue ToValue()
        {
            return base.ToValue(new PersonRegisterFormInfoValue { });
        }
    }
}

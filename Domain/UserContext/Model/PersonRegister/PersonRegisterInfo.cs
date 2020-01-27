using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public class PersonRegisterInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonRegisterInfo-{0}", ID);
        }
    }

    public struct PersonRegisterInfoValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string RegisterID { get; internal set; }

        public DateTime RegisteredTime { get; internal set; }
        public string FormID { get; internal set; }
    }

    public class PersonRegisterInfo
        : DomainAggregateChild<PersonRegisterInfoValue, PersonRegisterInfoID, PersonRegisterID>
    {
        // 注册表系统识别码
        internal string RegisterID { get { return pid.ID; } }

        // 注册时间
        public DateTime RegisteredTime { get; }
        // 审批表单
        public PersonRegisterForm Form { get; }

        internal PersonRegisterInfo(PersonRegisterID pid, PersonRegisterForm form)
            : base(pid)
        {
            RegisteredTime = DateTime.Now;
            Form = form;
        }

        internal PersonRegisterInfo(PersonRegisterID pid, PersonRegisterInfoID id, DateTime registeredTime, PersonRegisterForm form)
            : base(pid, id)
        {
            RegisteredTime = registeredTime;
            Form = form;
        }

        public override PersonRegisterInfoValue ToValue()
        {
            return new PersonRegisterInfoValue
            {
                ID = this.ID,
                RegisterID = this.RegisterID,
                RegisteredTime = this.RegisteredTime, 
                FormID = this.Form.ID
            };
        }
    }
}

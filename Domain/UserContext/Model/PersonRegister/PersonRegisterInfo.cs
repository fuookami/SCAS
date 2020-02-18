using SCAS.Module;
using System;
using System.Diagnostics.CodeAnalysis;

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

    public class PersonRegisterInfoValue
        : DomainEntityValueBase
    {
        [NotNull] public string RegisterID { get; internal set; }

        public DateTime RegisteredTime { get; internal set; }
    }

    public class PersonRegisterInfo
        : DomainAggregateChild<PersonRegisterInfoValue, PersonRegisterInfoID, PersonRegisterID>
    {
        // 注册表系统识别码
        [NotNull] public string RegisterID => pid.ID;

        // 注册时间
        public DateTime RegisteredTime { get; }

        internal PersonRegisterInfo(PersonRegisterID pid)
            : base(pid)
        {
            RegisteredTime = DateTime.Now;
        }

        internal PersonRegisterInfo(PersonRegisterID pid, PersonRegisterInfoID id, DateTime registeredTime)
            : base(pid, id)
        {
            RegisteredTime = registeredTime;
        }

        public override PersonRegisterInfoValue ToValue()
        {
            return base.ToValue(new PersonRegisterInfoValue
            {
                RegisterID = this.RegisterID,
                RegisteredTime = this.RegisteredTime
            });
        }
    }
}

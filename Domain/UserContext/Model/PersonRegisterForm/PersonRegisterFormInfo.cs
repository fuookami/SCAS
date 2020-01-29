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

    public struct PersonRegisterFormInfoValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string FormID { get; internal set; }
        public string InitiatorID { get; internal set; }
        public DateTime InitialTime { get; internal set; }
    }

    // 个人注册表审批表单信息
    public class PersonRegisterFormInfo
        : DomainAggregateChild<PersonRegisterFormInfoValue, PersonRegisterFormInfoID, PersonRegisterFormID>
    {
        // 审批表单系统识别码
        public string FormID { get { return pid.ID; } }

        // 发起者
        public Person Initiator { get; }
        // 发起时间
        public DateTime InitialTime { get; }

        internal PersonRegisterFormInfo(PersonRegisterFormID pid, Person initiator)
            : base(pid)
        {
            Initiator = initiator;
            InitialTime = DateTime.Now;
        }

        internal PersonRegisterFormInfo(PersonRegisterFormID pid, PersonRegisterFormInfoID id, Person initiator, DateTime initialTime)
            : base(pid, id)
        {
            Initiator = initiator;
            InitialTime = initialTime;
        }

        public override PersonRegisterFormInfoValue ToValue()
        {
            return new PersonRegisterFormInfoValue
            { 
                ID = this.ID, 
                FormID = this.FormID, 
                InitiatorID = this.Initiator.ID, 
                InitialTime = this.InitialTime
            };
        }
    }
}

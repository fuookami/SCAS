using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public class PersonRegisterFormExaminationID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonRegisterFormExamination-{0}", ID);
        }
    }

    public struct PersonRegisterFormExaminationValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string FormID { get; internal set; }

        public string ExaminerID { get; internal set; }
        public DateTime ExaminationTime { get; internal set; }

        public bool Approved { get; internal set; }
        public string Annotation { get; internal set; }
    }

    // 个人注册表审批表单审批结果
    public class PersonRegisterFormExamination
        : DomainAggregateChild<PersonRegisterFormExaminationValue, PersonRegisterFormExaminationID, PersonRegisterFormID>
    {
        // 审批表单系统识别码
        public string FormID { get { return pid.ID; } }

        // 审批人
        public Person Examiner { get; }
        // 审批时间
        public DateTime ExaminationTime { get; }

        // 是否通过
        public bool Approved { get; }
        // 审批意见
        public string Annotation { get; }

        internal PersonRegisterFormExamination(PersonRegisterFormID pid, Person examiner, bool approved, string annotation)
            : base(pid)
        {
            Examiner = examiner;
            ExaminationTime = DateTime.Now;
            Approved = approved;
            Annotation = annotation;
        }

        internal PersonRegisterFormExamination(PersonRegisterFormID pid, PersonRegisterFormExaminationID id, Person examiner, DateTime examinationTime, bool approved, string annotation)
            : base(pid, id)
        {
            Examiner = examiner;
            ExaminationTime = examinationTime;
            Approved = approved;
            Annotation = annotation;
        }

        public override PersonRegisterFormExaminationValue ToValue()
        {
            return new PersonRegisterFormExaminationValue
            {
                ID = this.ID, 
                FormID = this.FormID, 
                ExaminerID = this.Examiner.ID, 
                ExaminationTime = this.ExaminationTime, 
                Approved = this.Approved, 
                Annotation = this.Annotation
            };
        }
    }
}

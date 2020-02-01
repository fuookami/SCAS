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

    public class PersonRegisterFormExaminationValue
        : RegisterFormExaminiationValueBase
    {
    }

    // 个人注册表审批表单审批结果
    public class PersonRegisterFormExamination
        : RegisterFormExaminationBase<PersonRegisterFormExaminationValue, PersonRegisterFormExaminationID, PersonRegisterFormID>
    {
        internal PersonRegisterFormExamination(PersonRegisterFormID pid, Person examiner, bool approved, string annotation)
            : base(pid, examiner, approved, annotation)
        {
        }

        internal PersonRegisterFormExamination(PersonRegisterFormID pid, PersonRegisterFormExaminationID id, Person examiner, DateTime examinationTime, bool approved, string annotation)
            : base(pid, id, examiner, examinationTime, approved, annotation)
        {
        }

        public override PersonRegisterFormExaminationValue ToValue()
        {
            return base.ToValue(new PersonRegisterFormExaminationValue { });
        }
    }
}

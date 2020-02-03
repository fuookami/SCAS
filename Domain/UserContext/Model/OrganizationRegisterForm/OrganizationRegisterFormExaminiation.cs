using SCAS.Module;
using System;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterFormExaminationID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationRegisterFormExamination-{0}", ID);
        }
    }

    public class OrganizationRegisterFormExaminationValue
        : RegisterFormExaminiationValueBase
    {
    }

    // 组织注册表审批表单审批结果
    public class OrganizationRegisterFormExamination
         : RegisterFormExaminationBase<OrganizationRegisterFormExaminationValue, OrganizationRegisterFormExaminationID, OrganizationRegisterFormID>
    {
        internal OrganizationRegisterFormExamination(OrganizationRegisterFormID pid, Person examiner, bool approved, string annotation)
            : base(pid, examiner, approved, annotation)
        {
        }

        internal OrganizationRegisterFormExamination(OrganizationRegisterFormID pid, OrganizationRegisterFormExaminationID id, Person examiner, DateTime examinationTime, bool approved, string annotation)
            : base(pid, id, examiner, examinationTime, approved, annotation)
        {
        }

        public override OrganizationRegisterFormExaminationValue ToValue()
        {
            return base.ToValue(new OrganizationRegisterFormExaminationValue { });
        }
    }
}

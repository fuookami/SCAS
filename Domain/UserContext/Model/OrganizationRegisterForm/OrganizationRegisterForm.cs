using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterFormID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("OrganizationRegisterForm-{0}", ID);
        }
    }

    public class OrganizationRegisterFormValue
        : RegisterFormValueBase
    {
        [NotNull] public string OrgID { get; internal set; }
        [NotNull] public string RegionID { get; internal set; }
    }

    // 组织注册表审批表单
    public class OrganizationRegisterForm
        : RegisterFormBase<OrganizationRegisterFormValue, OrganizationRegisterFormID, OrganizationRegister, OrganizationRegisterFormInfo, OrganizationRegisterFormExamination>
    {
        // 注册组织
        public Organization Org { get; }
        // 目标域
        public Region RegisteredRegion { get; }

        internal OrganizationRegisterForm(string sid, Organization org, Region region, Person initiator, uint prefixCode)
            : base(sid)
        {
            Org = org;
            RegisteredRegion = region;
            Info = new OrganizationRegisterFormInfo(this.id, initiator, prefixCode);
        }

        internal OrganizationRegisterForm(OrganizationRegisterFormID id, string sid, Organization org, Region region, OrganizationRegisterFormInfo info, OrganizationRegisterFormExamination examination = null)
            : base(id, sid, info, examination)
        {
            Org = org;
            RegisteredRegion = region;
        }

        public override OrganizationRegister Approve(string sid, Person examiner, string annotation)
        {
            Examination = new OrganizationRegisterFormExamination(this.id, examiner, true, annotation);
            return new OrganizationRegister(sid, Org, RegisteredRegion, Info.PrefixCode);
        }

        public override void Unapprove(Person examiner, string annotation)
        {
            Examination = new OrganizationRegisterFormExamination(id, examiner, false, annotation);
        }

        public override OrganizationRegisterFormValue ToValue()
        {
            return base.ToValue(new OrganizationRegisterFormValue 
            {
                OrgID = this.Org.ID, 
                RegionID = this.RegisteredRegion.ID
            });
        }
    }
}

using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public class PersonRegisterFormID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonRegisterForm-{0}", ID);
        }
    }

    public class PersonRegisterFormValue
        : RegisterFormValueBase
    {
        [NotNull] public string PersonID { get; internal set; }
        [NotNull] public string RegionID { get; internal set; }
        public string OrgID { get; internal set; }
    }

    // 个人注册表审批表单
    public class PersonRegisterForm
        : RegisterFormBase<PersonRegisterFormValue, PersonRegisterFormID, PersonRegister, PersonRegisterFormInfo, PersonRegisterFormExamination>
    {
        // 注册人
        [DisallowNull] public Person Person { get; }
        // 目标域
        [DisallowNull] public Region RegisteredRegion { get; }
        // 目标组织
        public Organization BelongingOrganization { get; }

        internal PersonRegisterForm(string sid, Person person, Region region, Person initiator)
            : base(sid)
        {
            Person = person;
            RegisteredRegion = region;
            Info = new PersonRegisterFormInfo(this.id, initiator);
        }

        internal PersonRegisterForm(string sid, Person person, Region region, Organization org, Person initiator)
            : this(sid, person, region, initiator)
        {
            BelongingOrganization = org;
        }

        internal PersonRegisterForm(PersonRegisterFormID id, string sid, Person person, Region region, Organization org, PersonRegisterFormInfo info, PersonRegisterFormExamination examination = null)
            : base(id, sid, info, examination)
        {
            Person = person;
            RegisteredRegion = region;
            BelongingOrganization = org;
        }

        public override PersonRegister Approve(string sid, Person examiner, string annotation)
        {
            Examination = new PersonRegisterFormExamination(id, examiner, true, annotation);
            Archive();
            return new PersonRegister(sid, Person, RegisteredRegion, BelongingOrganization);
        }

        public override void Unapprove(Person examiner, string annotation)
        {
            Examination = new PersonRegisterFormExamination(id, examiner, false, annotation);
            Archive();
        }

        public override PersonRegisterFormValue ToValue()
        {
            return base.ToValue(new PersonRegisterFormValue
            {
                PersonID = this.Person.ID,
                RegionID = this.RegisteredRegion.ID,
                OrgID = this.BelongingOrganization?.ID
            });
        }
    }
}

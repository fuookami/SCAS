﻿using SCAS.Utils;

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

    public struct PersonRegisterFormValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string SID { get; internal set; }
        public string PersonID { get; internal set; }
        public string RegionID { get; internal set; }
        public string OrgID { get; internal set; }
    }

    // 个人注册表审批表单
    public class PersonRegisterForm
        : DomainAggregateRoot<PersonRegisterFormValue, PersonRegisterFormID>
    {
        // 应用识别码，由系统使用者给定规则（流水号）
        public string SID { get; }

        // 注册人
        public Person Person { get; }
        // 目标域
        public Region RegisteredRegion { get; }
        // 目标组织
        public Organization BelongingOrganization { get; }

        // 审批表单信息
        public PersonRegisterFormInfo Info { get; }
        // 审批结果
        public PersonRegisterFormExamination Examination { get; private set; }

        // 是否已经审批
        public bool Examined { get { return Examination != null; } }
        // 审批是否通过
        public bool Approved { get { return Examination.Approved; } }

        internal PersonRegisterForm(string sid, Person person, Region region, Person initiator)
        {
            SID = sid;
            Person = person;
            RegisteredRegion = region;
            Info = new PersonRegisterFormInfo(id, initiator);
        }

        internal PersonRegisterForm(string sid, Person person, Region region, Organization org, Person initiator)
            : this(sid, person, region, initiator)
        {
            BelongingOrganization = org;
        }

        internal PersonRegisterForm(PersonRegisterFormID id, string sid, Person person, Region region, Organization org, PersonRegisterFormInfo info, PersonRegisterFormExamination examination = null)
            : base(id)
        {
            SID = sid;
            Person = person;
            RegisteredRegion = region;
            BelongingOrganization = org;
            Info = info;
            Examination = examination;
        }

        public override PersonRegisterFormValue ToValue()
        {
            return new PersonRegisterFormValue
            {
                ID = this.ID, 
                SID = this.SID, 
                PersonID = this.Person.ID, 
                RegionID = this.RegisteredRegion.ID, 
                OrgID = this.BelongingOrganization?.ID
            };
        }

        public PersonRegister Approve(string sid, Person examiner, string annotation)
        {
            Examination = new PersonRegisterFormExamination(id, examiner, true, annotation);
            return new PersonRegister(sid, Person, RegisteredRegion, BelongingOrganization, this);
        }

        public void Unapprove(Person examiner, string annotation)
        {
            Examination = new PersonRegisterFormExamination(id, examiner, false, annotation);
        }
    }
}

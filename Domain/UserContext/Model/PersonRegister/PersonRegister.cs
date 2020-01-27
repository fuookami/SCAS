using SCAS.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    public class PersonRegisterID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonRegister-{0}", ID); 
        }
    }

    public struct PersonRegisterValue
    : IPersistentValue
    {
        public string ID { get; internal set; }
        public string SID { get; internal set; }
        public string PersonID { get; internal set; }
        public string OrgID { get; internal set; }
    }

    // 个人注册表
    public class PersonRegister
        : DomainAggregateRoot<PersonRegisterValue, PersonRegisterID>
    {
        // 在当前组织的应用识别码，由系统使用者给定规则
        public string SID { get; }

        // 个人
        public Person Person { get; }
        // 注册的组织
        public Organization Org { get; }

        // 注册信息
        public PersonRegisterInfo Info { get; }

        internal PersonRegister(string sid, Person person, Organization org, PersonRegisterForm form)
        {
            SID = sid;
            Person = person;
            Org = org;
            Info = new PersonRegisterInfo(id, form);
        }

        internal PersonRegister(PersonRegisterID id, string sid, Person person, Organization org, PersonRegisterInfo info)
            : base(id)
        {
            SID = sid;
            Person = person;
            Org = org;
            Info = info;
        }

        public override PersonRegisterValue ToValue()
        {
            return new PersonRegisterValue
            {
                ID = this.ID,
                SID = this.SID,
                PersonID = this.Person.ID,
                OrgID = this.Org.ID
            };
        }
    }

    public struct PersonRegisters
        : IDomainValue
    {
        // 注册列表
        private Dictionary<Organization, PersonRegister> registers;

        // 已注册的组织
        public IReadOnlyList<Organization> RegisteredOrgs { get { return registers.Keys.ToList(); } }

        internal PersonRegisters(IReadOnlyList<PersonRegister> registersList)
        {
            registers = registersList.ToDictionary(reg => reg.Org);
        }
    }
}

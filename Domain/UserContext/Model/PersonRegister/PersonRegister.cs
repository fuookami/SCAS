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
        public string RegionID { get; internal set; }
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
        // 注册的域
        public Region RegisteredRegion { get; }
        // 注册的组织
        public Organization BelongingOrganization { get; }

        // 注册信息
        public PersonRegisterInfo Info { get; }
        // 审批表单
        public PersonRegisterForm Form { get; }

        // 是否有所属的组织
        public bool BelongedAnyOrganization { get { return BelongingOrganization != null; } }

        internal PersonRegister(string sid, Person person, Region region, Organization org, PersonRegisterForm form)
        {
            SID = sid;
            Person = person;
            RegisteredRegion = region;
            BelongingOrganization = org;
            Info = new PersonRegisterInfo(id, form);

        }

        internal PersonRegister(PersonRegisterID id, string sid, Person person, Region region, Organization org, PersonRegisterInfo info)
            : base(id)
        {
            SID = sid;
            Person = person; 
            RegisteredRegion = region;
            BelongingOrganization = org;
            Info = info;
        }

        public override PersonRegisterValue ToValue()
        {
            return new PersonRegisterValue
            {
                ID = this.ID,
                SID = this.SID,
                PersonID = this.Person.ID,
                RegionID = this.RegisteredRegion.ID, 
                OrgID = this.BelongingOrganization?.ID
            };
        }
    }

    public struct PersonRegisters
        : IDomainValue
    {
        // 注册列表
        private Dictionary<Region, PersonRegister> registers;

        // 已注册的域
        public IReadOnlyList<Region> RegisteredRegions { get { return registers.Keys.ToList(); } }

        public bool RegisteredIn(Region region)
        {
            return RegisteredRegions.Contains(region);
        }

        public bool? BePersonalIn(Region region)
        {
            return !registers[region]?.BelongedAnyOrganization;
        }

        public Organization BelongedOrganizationIn(Region region)
        {
            return registers?[region].BelongingOrganization;
        }

        internal PersonRegisters(IReadOnlyList<PersonRegister> registersList)
        {
            registers = registersList.ToDictionary(reg => reg.RegisteredRegion);
        }
    }
}

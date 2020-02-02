using SCAS.Utils;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    public class PersonRegisterValue
        : DomainEntityValueBase
    {
        [NotNull] public string SID { get; internal set; }
        [NotNull] public string PersonID { get; internal set; }
        [NotNull] public string RegionID { get; internal set; }
        [NotNull] public string OrgID { get; internal set; }
    }

    // 个人注册表
    public class PersonRegister
        : DomainAggregateRoot<PersonRegisterValue, PersonRegisterID>
    {
        // 在当前组织的应用识别码，由系统使用者给定规则
        [DisallowNull] public string SID { get; }

        // 个人
        [DisallowNull] public Person Person { get; }
        // 注册的域
        [DisallowNull] public Region RegisteredRegion { get; }
        // 注册的组织
        [DisallowNull] public Organization BelongingOrganization { get; }

        // 注册信息
        [DisallowNull] public PersonRegisterInfo Info { get; }

        // 是否有所属的组织
        public bool BelongedAnyOrganization { get { return BelongingOrganization != null; } }

        internal PersonRegister(string sid, Person person, Region region, Organization org)
        {
            SID = sid;
            Person = person;
            RegisteredRegion = region;
            BelongingOrganization = org;
            Info = new PersonRegisterInfo(id);
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
            return base.ToValue(new PersonRegisterValue
            {
                SID = this.SID,
                PersonID = this.Person.ID,
                RegionID = this.RegisteredRegion.ID, 
                OrgID = this.BelongingOrganization?.ID
            });
        }
    }

    public struct PersonRegisters
        : IDomainValue
    {
        // 注册列表
        private Dictionary<Region, PersonRegister> registers;

        // 已注册的域
        public IReadOnlyList<Region> RegisteredRegions { get { return registers.Keys.ToList(); } }

        // 是否在某个域中注册
        public bool RegisteredIn(Region region)
        {
            return RegisteredRegions.Contains(region);
        }

        // 是否以个人身份注册某个域
        public bool? BePersonalIn(Region region)
        {
            return !registers[region]?.BelongedAnyOrganization;
        }

        // 在某个域中所属的组织
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

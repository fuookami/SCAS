using SCAS.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterValue
    : IPersistentValue
    {
        public string Id { get; internal set; }
        public string SId { get; internal set; }
        public string PersonId { get; internal set; }
        public string OrgId { get; internal set; }
    }

    // 个人注册表
    public partial class PersonRegister
        : IDomainAggregate<PersonRegisterValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 在当前组织的应用识别码，由系统使用者给定规则
        public string SId { get; }
        // 个人系统识别码
        public string PersonId { get; }

        // 注册的组织
        public Organization RegisteredOrg { get; }
        // 注册信息
        public PersonRegisterInfo Info { get; internal set; }

        public PersonRegisterValue ToValue()
        {
            return new PersonRegisterValue
            {
                Id = this.Id,
                SId = this.SId,
                PersonId = this.PersonId,
                OrgId = this.RegisteredOrg.Id
            };
        }
    }

    public struct PersonRegisters
        : IDomainValue
    {
        // 注册列表
        private Dictionary<Organization, PersonRegister> orgRegistersList;

        // 已注册的组织
        public IReadOnlyList<Organization> RegisteredOrgs { get { return orgRegistersList.Keys.ToList(); } }
        // 已注册的域
        public IReadOnlyList<Region> RegisteredRegions { get { return RegisteredOrgs.SelectMany(org => org.RegisteredRegions).Distinct().ToList(); } }

        internal PersonRegisters(IReadOnlyList<PersonRegister> registers)
        {
            orgRegistersList = registers.ToDictionary(reg => reg.RegisteredOrg);
        }
    }
}

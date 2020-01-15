using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    // 个人
    public partial class Person
    {
        // 系统识别码，由系统生成
        public string Id { get; }

        // 在当前域下的识别码，由系统使用者给定，
        public string Sid { get; internal set; }
        // 姓名
        public string Name { get; internal set; }
        
        // 个人信息
        public PersonInfo Info { get; internal set; }
        // 注册列表
        internal Dictionary<Organization, PersonRegister> OrgRegistersList { get; }

        // 已注册的组织
        public IReadOnlyList<Organization> RegisteredOrgs { get { return OrgRegistersList.Keys.ToList(); } }
        // 已注册的域
        public IReadOnlyList<Region> RegisteredRegions { get { return RegisteredOrgs.SelectMany(org => org.RegisteredRegions).Distinct().ToList(); } }
    }

    public struct PersonValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Sid { get; internal set; }
        public string Name { get; internal set; }
    }

    public partial class Person
        : IPersistentType<PersonValue>
    {
        public PersonValue ToValue()
        {
            return new PersonValue
            {
                Id = this.Id,
                Sid = this.Sid,
                Name = this.Name
            };
        }
    }
};

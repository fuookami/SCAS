using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.Basic
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

        // 所属组织
        internal List<Organization> BelongedOrganizationsList { get; }
        public IReadOnlyList<Organization> BelongedOrganizations { get { return BelongedOrganizationsList; } }
        public IReadOnlyList<Region> BelongedRegions { get { return BelongedOrganizationsList.SelectMany(org => org.BelongedRegions).ToList(); } }

        internal Person(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString("N");
            BelongedOrganizationsList = new List<Organization>();
        }
    }

    public struct PersonValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Sid { get; internal set; }
        public string Name { get; internal set; }

        public IReadOnlyList<string> BelongedOrganizations { get; internal set; }
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
                Name = this.Name,
                BelongedOrganizations = this.BelongedOrganizations.Select(org => org.Id).ToList()
            };
        }
    }
};

using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.UserContext
{
    public struct RegionValue
    {
        public string Id { get; internal set; }
    };

    // 域
    public partial class Region
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 域名
        public string Name { get; internal set; }
        // 描述
        public string Description { get; internal set; }

        internal List<Organization> OrganizationsList { get; }
        public IReadOnlyList<Organization> Organizations { get { return OrganizationsList; } }

        internal Region(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString("N");
            OrganizationsList = new List<Organization>();
        }
    };

    public struct RegionValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }

        public IReadOnlyList<string> Organizations { get; internal set; }
    }

    public partial class Region
        : IPersistentType<RegionValue>
    {
        public RegionValue ToValue()
        {
            return new RegionValue
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Organizations = this.Organizations.Select(region => region.Id).ToList()
            };
        }
    }
}

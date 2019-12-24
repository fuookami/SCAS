using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.Basic
{
    public partial class Organization
    {
        // 系统识别码，由系统生成
        public string Id { get; }

        // 在当前域下的识别码，由系统使用者给定
        public string Sid { get; internal set; }
        // 组织名
        public string Name { get; internal set; }
        // 描述
        public string Description { get; internal set; }

        // 在各个域下的前缀码，用于秩序册的生成，由系统使用者给定，应为两位数字
        internal Dictionary<Region, uint> PrefixCodesDict { get; }
        public IReadOnlyList<Region> BelongedRegions { get { return PrefixCodes.Keys.ToList(); } }
        public IReadOnlyDictionary<Region, uint> PrefixCodes { get { return PrefixCodesDict; } }

        internal Organization(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString("N");
            PrefixCodesDict = new Dictionary<Region, uint>();
        }
    }

    public struct OrganizationValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Sid { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }

        public IReadOnlyDictionary<string, uint> PrefixCodes { get; internal set; }
    }

    public partial class Organization
        : IPersistentType<OrganizationValue>
    {
        public OrganizationValue ToValue()
        {
            return new OrganizationValue
            {
                Id = this.Id,
                Sid = this.Sid,
                Name = this.Name,
                Description = this.Description,
                PrefixCodes = this.PrefixCodes.ToDictionary(pair => pair.Key.Id, pair => pair.Value)
            };
        }
    }
};

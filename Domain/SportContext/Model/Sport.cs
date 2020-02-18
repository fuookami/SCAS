using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.Basic
{
    // 运动
    public partial class Sport
    {
        // 系统识别码，由系统生成
        public string Id { get; }

        // 运动名
        public string Name { get; internal set; }
        // 描述
        public string Description { get; internal set; }
        // 项目列表
        internal List<SportItem> ItemsList { get; }
        public IReadOnlyList<SportItem> Items => ItemsList;

        internal Sport(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString("N");
            ItemsList = new List<SportItem>();
        }
    }

    public struct SportValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }

        public IReadOnlyList<string> Items { get; internal set; }
    }

    public partial class Sport
        : IPersistentType<SportValue>
    {
        public SportValue ToValue()
        {
            return new SportValue
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Items = this.Items.Select(item => item.Id).ToList()
            };
        }
    }
}

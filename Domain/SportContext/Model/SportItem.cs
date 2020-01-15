using SCAS.Utils;
using System;

namespace SCAS.Domain.Basic
{
    // 项目
    public partial class SportItem
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 项目名
        public string Name { get; internal set; }
        // 描述
        public string Description { get; internal set; }

        internal SportItem(string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString("N");
        }
    }

    public struct SportItemValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }

    public partial class SportItem
        : IPersistentType<SportItemValue>
    {
        public SportItemValue ToValue()
        {
            return new SportItemValue
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description
            };
        }
    }
}

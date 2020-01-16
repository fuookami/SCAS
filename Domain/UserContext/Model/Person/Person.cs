using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public struct PersonValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string SId { get; internal set; }
    }

    // 个人
    public partial class Person
        : IDomainAggregate<PersonValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 应用识别码，由系统使用者给定规则
        public string SId { get; internal set; }

        // 个人信息
        public PersonInfo Info { get; internal set; }

        public PersonValue ToValue()
        {
            return new PersonValue
            {
                Id = this.Id,
                SId = this.SId
            };
        }
    }
}

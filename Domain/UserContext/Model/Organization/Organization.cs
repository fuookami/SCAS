using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string Sid { get; internal set; }
    }

    public partial class Organization
        : IDomainAggregate<OrganizationValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 在当前域下的识别码，由系统使用者给定
        public string Sid { get; }
        
        // 组织信息
        public OrganizationInfo Info { get; }

        public OrganizationValue ToValue()
        {
            return new OrganizationValue
            {
                Id = this.Id,
                Sid = this.Sid
            };
        }
    }
}

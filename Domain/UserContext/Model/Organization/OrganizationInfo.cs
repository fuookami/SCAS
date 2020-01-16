using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationInfoValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string OrgId { get; internal set; }

        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }

    public class OrganizationInfo
        : IDomainEntity<OrganizationInfoValue>
    {
        // 系统识别码， 由系统生成
        public string Id { get; }
        // 组织系统识别码
        internal string OrgId { get; }

        // 组织名
        public string Name { get; internal set; }
        // 描述
        public string Description { get; internal set; }

        public OrganizationInfoValue ToValue()
        {
            return new OrganizationInfoValue
            {
                Id = this.Id, 
                OrgId = this.OrgId, 
                Name = this.Name, 
                Description = this.Description
            };
        }
    }
}

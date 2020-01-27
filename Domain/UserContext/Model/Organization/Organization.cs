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
        : DomainAggregate<OrganizationValue, OrganizationID>
    {
        // 在当前域下的识别码，由系统使用者给定
        public string SID { get; }
        
        // 组织信息
        public OrganizationInfo Info { get; }

        public override OrganizationValue ToValue()
        {
            return new OrganizationValue
            {
                Id = this.ID.ID, 
                Sid = this.Sid
            };
        }
    }
}

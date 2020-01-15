using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    // 组织注册表信息
    public partial class OrganizationRegisterInfo
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 组织注册表
        public OrganizationRegister OrgRegister { get; }

        // 注册时间
        public DateTime RegisterdTime { get; }

        internal OrganizationRegisterInfo(string id, )
    }

    public struct OrganizationRegisterInfoValue
        : IPersistentValue
    {
        
    }

    public partial class OrganizationRegisterInfo
        : IPersistentType<OrganizationRegisterInfoValue>
    {
        public OrganizationRegisterInfoValue ToValue()
        {
            
        }
    }
}

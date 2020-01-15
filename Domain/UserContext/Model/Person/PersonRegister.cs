using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    // 个人注册表
    public partial class PersonRegister
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 个人系统识别码
        public string PersonId { get; }

        // 注册的组织
        public Organization RegisteredOrg { get; }
        // 注册信息
        public PersonRegisterInfo Info { get; internal set; }
    }

    public struct PersonRegisterValue
        : IPersistentValue
    { 
        public string Id { get; internal set; }
        public string PersonId { get; internal set; }
        public string OrgId { get; internal set; }
    }

    public partial class PersonRegister
        : IPersistentType<PersonRegisterValue>
    {
        public PersonRegisterValue ToValue()
        {
            return new PersonRegisterValue
            {
                Id = this.Id, 
                PersonId = this.PersonId, 
                OrgId = this.RegisteredOrg.Id
            };
        }
    }
}

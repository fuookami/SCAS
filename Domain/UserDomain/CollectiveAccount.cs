using SCAS.Domain.Basic;

namespace SCAS.Domain.User
{
    class CollectiveAccount
        : Account
    {
        // 所属组织
        public Organization BelongedOrganization { get; internal set; }

        // 密码
        public string Password { get; internal set; }
    }
}

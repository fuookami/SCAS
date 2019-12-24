using SCAS.Domain.Basic;
using System.Collections;

namespace SCAS.Domain.User
{
    public class CollectiveAccount
        : Account
    {
        // 所属组织
        public Organization BelongedOrganization { get; internal set; }

        // 可以使用的用户
        internal List<Person> EnabledUsers { get; set; }

        // 密码
        public string Password { get; internal set; }
    }
}

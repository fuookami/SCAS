using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.AccountContext
{
    public class CollectiveAccount
        : Account
    {
        // 所属组织
        public Organization BelongedOrganization { get; internal set; }

        // 密码
        public string Password { get; internal set; }

        // 可使用的用户列表
        internal List<User> EnabledUsersList { get; }
        public IReadOnlyList<User> EnabledUsers { get { return EnabledUsersList; } }

        public override bool Allow(User user)
        {
            return EnabledUsers.Contains(user);
        }
    }
}

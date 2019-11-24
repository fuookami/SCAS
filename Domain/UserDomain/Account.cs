using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.User
{
    // 账号类型
    enum AccountType
    {
        Personal,
        Collective, 
        Public
    }

    // 账号权限
    enum AccountAuthority
    {
        Visitor,
        Applicant,
        ApplicantLeader, 
        Staff, 
        StaffManager,
        StaffLeader, 
        SystemManager, 
        Administrator
    }

    // 账号
    abstract class Account
    {
        // 账号
        public string Id { get; internal set; }

        // 类型
        public AccountType Type { get; internal set; }

        // 权限
        public AccountAuthority Authority { get; internal set; }

        // 可使用的用户列表
        internal List<User> EnabledUsersList { get; set; }
        public IReadOnlyList<User> EnabledUsers { get { return EnabledUsersList; } }

        public bool Allow(User user)
        {
            return EnabledUsers.Contains(user);
        }
    }
}

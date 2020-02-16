using SCAS.Domain.UserContext;

namespace SCAS.Domain.AccountContext
{
    // 账号类型
    public enum AccountType
    {
        Personal,
        Collective,
        Public
    }

    // 账号权限
    public enum AccountAuthority
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
    public abstract class Account
    {
        // 账号
        public string Id { get; internal set; }

        // 类型
        public AccountType Type { get; internal set; }

        // 权限
        public AccountAuthority Authority { get; internal set; }

        public abstract bool Allow(User user);
    }
}

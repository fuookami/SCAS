using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public 

    public class User
    {
        // 使用者
        public Person Person { get; internal set; }

        // 密码
        public string Password { get; internal set; }

        // 关联的账号
        internal List<Account> RelatedAccountsList { get; set; }
        public IReadOnlyList<Account> RelatedAccounts { get { return RelatedAccountsList; }  }
    }
}

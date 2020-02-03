using SCAS.Domain.UserContext;

namespace SCAS.Domain.AccountContext
{
    public class PublicAccount
        : Account
    {
        public override bool Allow(User user)
        {
            return true;
        }
    }
}

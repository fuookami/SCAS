namespace SCAS.Domain.UserContext
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

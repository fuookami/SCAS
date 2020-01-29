namespace SCAS.Domain.UserContext
{
    public class PersonalAccount 
        : Account
    {
        // 所属个人
        public Person BelongedPerson { get; internal set; }

        // 密码
        public string Password { get; internal set; }

        public User EnabledUser { get; internal set; }

        public override bool Allow(User user)
        {
            return EnabledUser == user;
        }
    }
}

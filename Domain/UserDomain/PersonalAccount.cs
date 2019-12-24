using SCAS.Domain.Basic;

namespace SCAS.Domain.User
{
    public class PersonalAccount 
        : Account
    {
        // 所属个人
        public Person BelongedPerson { get; internal set; }

        // 密码
        public string Password { get; internal set; }
    }
}

using SCAS.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public class UserID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("User-{0}", ID);
        }
    }

    public class UserValue
        : DomainEntityValueBase
    {
        [NotNull] public string PersonID { get; internal set; }
        [NotNull] public string Account { get; internal set; }
        [NotNull] public string Password { get; internal set; }
        public bool Available { get; internal set; }
    }

    public class User
        : DomainAggregateRoot<UserValue, UserID>
    {
        private IEncryptor encryptor;

        public Person Person { get; }
        public string Account { get; }
        public string Password { get; }
        public bool Available { get; }

        internal User(Person person, string account, string password, IEncryptor encryptor = null)
        {
            Person = person;
            Account = encryptor?.Decrypt(account) ?? account;
            Password = encryptor?.Decrypt(password) ?? password;
            Available = true;
        }

        internal User(UserID id, Person person, string account, string password, bool available, IEncryptor encryptor = null)
            : base(id)
        {
            Person = person;
            Account = encryptor?.Decrypt(account) ?? account;
            Password = encryptor?.Decrypt(password) ?? password;
            Available = available;
        }

        public override UserValue ToValue()
        {
            return base.ToValue(new UserValue
            {
                PersonID = this.Person.ID, 
                Account = this.encryptor?.Encrypt(Account) ?? Account, 
                Password = this.encryptor?.Encrypt(Password) ?? Password, 
                Available = this.Available
            });
        }
    }
}

using SCAS.Utils;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System;

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

        [DisallowNull] public Person Person { get; }
        [DisallowNull] public string Account { get; }
        [DisallowNull] public string Password { get; }
        public bool Available { get; internal set; }

        internal User(Person person, string account, string password, IEncryptor targetEncryptor = null)
        {
            Person = person;
            encryptor = targetEncryptor;
            Account = Decrypt(account);
            Password = Decrypt(password);
            Available = true;
        }

        internal User(UserID id, Person person, string account, string password, bool available, IEncryptor targetEncryptor = null)
            : base(id)
        {
            Person = person;
            encryptor = targetEncryptor;
            Account = Decrypt(account);
            Password = Decrypt(password);
            Available = available;
        }

        public override UserValue ToValue()
        {
            return base.ToValue(new UserValue
            {
                PersonID = this.Person.ID,
                Account = Encrypt(this.Account), 
                Password = Encrypt(this.Password),
                Available = this.Available
            });
        }

        private string Encrypt(string plaintext)
        {
            return Convert.ToBase64String(this.encryptor?.Encrypt(Encoding.ASCII.GetBytes(plaintext))) ?? plaintext;
        }

        private string Decrypt(string ciphertext)
        {
            return Encoding.ASCII.GetString(encryptor?.Decrypt(Convert.FromBase64String(ciphertext))) ?? ciphertext;
        }
    }
}

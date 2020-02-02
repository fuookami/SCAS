using System;

namespace SCAS.Utils
{
    public interface IEncryptor
    {
        public string Encrypt(string raw);
        public string Decrypt(string raw);
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace SCAS.Utils
{
    public interface IEncryptor
    {
        public byte[] Encrypt(byte[] plaintext);
        public byte[] Decrypt(byte[] ciphertext);
    }

    public class SCASEncryptor
        : IEncryptor
    {
        [DisallowNull] private byte[] publicKey;
        [DisallowNull] private byte[] privateKey;

        private RSACryptoServiceProvider rsa;

        public SCASEncryptor(byte[] targetPublicKey, byte[] targetPrivateKey)
        {
            publicKey = targetPublicKey;
            privateKey = targetPrivateKey;

            rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(publicKey, out _);
            rsa.ImportRSAPrivateKey(privateKey, out _);
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            return rsa.Encrypt(plaintext, false);
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            return rsa.Decrypt(ciphertext, false);
        }
    }
}

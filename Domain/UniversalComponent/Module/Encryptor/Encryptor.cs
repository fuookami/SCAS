namespace SCAS.Module
{
    public interface IEncryptor
    {
        public byte[] Encrypt(byte[] plaintext);
        public byte[] Decrypt(byte[] ciphertext);
    }
}

using System.Security.Cryptography;

namespace SCAS.Module
{
    public class SCASExtractor
        : IExtractor
    {
        private readonly MD5 md5;

        public SCASExtractor()
        {
            md5 = MD5.Create();
        }

        public byte[] Extract(byte[] data)
        {
            return md5.ComputeHash(data);
        }
    }
}

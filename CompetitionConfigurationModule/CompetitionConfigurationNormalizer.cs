using System;

namespace CompetitionConfigurationModule
{
    class CompetitionConfigurationNormalizer
    {
        public enum ErrorCode
        {
            NoError
        }

        private ErrorCode lastErrorCode;
        private String lastError;
        private CompetitionInfo outputData;
        private String binaryData;

        public ErrorCode LastErrorCode
        {
            get { return lastErrorCode; }
        }

        public String LastError
        {
            get { return lastError; }
        }

        public CompetitionInfo Data
        {
            get { return outputData; }
            set
            {
                outputData = value;
                binaryData = "";
            }
        }

        public String Binary
        {
            get { return binaryData; }
        }

        public CompetitionConfigurationNormalizer(CompetitionInfo data)
        {
            lastErrorCode = ErrorCode.NoError;
            outputData = data;
            binaryData = "";
        }

        private bool NormalizeToBinary()
        {
            // to do
            binaryData = "test";
            return true;
        }

        public bool Normalize()
        {
            if (binaryData.Length != 0)
            {
                return true;
            }
            return NormalizeToBinary();
        }

        public bool NormalizeToFile(String url)
        {
            if (binaryData.Length == 0 && !NormalizeToBinary())
            {
                return false;
            }
            // save to file
            return true;
        }

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}

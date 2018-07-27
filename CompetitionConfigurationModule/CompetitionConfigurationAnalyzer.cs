using System;

namespace CompetitionConfigurationModule
{
    class CompetitionConfigurationAnalyzer
    {
        public enum InputType
        {
            File,
            Binary
        }
        
        public enum ErrorCode
        {
            NoError
        }

        private InputType inputType;
        private ErrorCode lastErrorCode;
        private String lastError;
        private CompetitionInfo result;

        public InputType DataInputType
        {
            get { return inputType; }
            set { inputType = value; }
        }

        public ErrorCode LastErrorCode
        {
            get { return lastErrorCode; }
        }

        public String LastError
        {
            get { return lastError; }
        }

        public CompetitionConfigurationAnalyzer(InputType dataInputType = InputType.File)
        {
            inputType = dataInputType;
            lastErrorCode = ErrorCode.NoError;
            result = null;
        }

        public bool Analyze(String data)
        {
            switch (inputType)
            {
                case InputType.Binary:
                    return AnalyzeFromBinary(data);
                case InputType.File:
                    return AnalyzeFromFile(data);
                default:
                    return false;
            }
        }

        private bool AnalyzeFromFile(String url)
        {
            // load from the file
            String data = "1";
            return AnalyzeFromBinary(data);
        }

        private bool AnalyzeFromBinary(String json)
        {
            // to do
            result = new CompetitionInfo();
            return true;
        }

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}

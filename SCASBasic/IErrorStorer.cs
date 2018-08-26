using System;

namespace SCAS
{
    public enum ErrorCode
    {
        None
    };

    public class ErrorStorer
    {
        private ErrorCode lastErrorCode;
        private String lastError;

        public ErrorCode LastErrorCode
        {
            get { return lastErrorCode; }
        }

        public String LastError
        {
            get { return lastError; }
        }

        public ErrorStorer()
        {
            lastErrorCode = ErrorCode.None;
        }

        public void Refresh()
        {
            RefreshError(ErrorCode.None);
        }

        protected void RefreshError(ErrorCode code, String text = "")
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    };
};

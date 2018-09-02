using System;

namespace SCAS
{
    public enum ErrorCode
    {
        None,
        MismatchedFileExtension
    };

    public class ErrorStorer
    {
        public ErrorCode LastErrorCode
        {
            get;
            private set;
        }

        public String LastError
        {
            get;
            private set;
        }

        public ErrorStorer()
        {
            LastErrorCode = ErrorCode.None;
        }

        public void Refresh()
        {
            RefreshError(ErrorCode.None);
        }

        protected void RefreshError(ErrorCode code, String text = "")
        {
            LastErrorCode = code;
            LastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    };
};

using System;
using System.Collections.Generic;

namespace SCAS.Utils
{
    public enum ErrCode
    {
        A,
        B
    };

    public partial class ErrCodeTrait
    {
        public static IReadOnlyDictionary<ErrCode, string> DefaultMessage { get; }
    };

    public class Error
    {
        public ErrCode Code { get; }
        public DateTime Time { get; }
        public string Message { get; }

        public Error(ErrCode code, string message = null)
        {
            Code = code;
            Message = message ?? ErrCodeTrait.DefaultMessage[code];
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("Error:{0}, {1}, {2}", Code.ToString(), Time.ToString(), Message);
        }
    }

    public partial class ErrCodeTrait
    {
        static ErrCodeTrait()
        {
            DefaultMessage = new Dictionary<ErrCode, string>
            {
                { ErrCode.A, "A" }
            };
        }
    };
}

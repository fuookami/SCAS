using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS.Utils
{
    public class Try
    {
        public bool Succeeded { get; }
        public Error Err { get; }

        public static Try Success;

        static Try() {
            Success = new Try();
        }

        public Try()
        {
            Succeeded = true;
        }

        public Try(Error err)
        {
            Succeeded = false;
            Err = err;
        }

        public static implicit operator bool(Try v)
        {
            return v.Succeeded;
        }

        public static Try operator&(Try lhs, Try rhs)
        {
            return lhs.Succeeded ? rhs : lhs;
        }

        public static Try operator|(Try lhs, Try rhs)
        {
            return lhs.Succeeded ? lhs : rhs;
        }

        public static bool operator true(Try v)
        {
            return v.Succeeded;
        }

        public static bool operator false(Try v)
        {
            return !v.Succeeded;
        }
    }

    public class TryEx<T>
        : Try
    {
        public T Value { get; }

        public TryEx(T ret)
            : base()
        {
            Value = ret;
        }

        public TryEx(Error err)
            : base(err)
        {
        }
    }
}

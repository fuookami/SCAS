using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS.Utils
{
    public class Try
    {
        public bool Succeed { get; }
        public Error Err { get; }

        public Try()
        {
            Succeed = true;
        }

        public Try(Error err)
        {
            Succeed = false;
            Err = err;
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

using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS.Utils
{
    public class Try<T>
    {
        public bool Succeed { get; }
        public T Value { get; }

        public Try()
        {
            Succeed = false;
        }

        public Try(T ret)
        {
            Succeed = true;
            Value = ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS.Utils
{
    public interface IPersistentValue
    {
    }

    public interface IPersistentType<out T>
        where T: IPersistentValue
    {
        public T ToValue();
    }
}

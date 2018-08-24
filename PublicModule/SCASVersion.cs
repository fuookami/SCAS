using System;

namespace SCAS
{
    public class Version
    {
        const Int32 MajorVersion = 0;
        const Int32 SubVersion = 1;
        const Int32 ModifyVersion = 6;

        new static String ToString()
        {
            return String.Format("{0}{1:D2}{2:D3}", MajorVersion, SubVersion, ModifyVersion);
        }
    }
}

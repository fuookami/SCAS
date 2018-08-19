using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class GroupInfo
        {
            public const Int32 NoGroup = -1;
            public const Int32 NoLimit = 0;

            private bool enabled;
            private Int32 numberPerGroup;

            public bool Enabled
            {
                get { return enabled; }
                set
                {
                    if (value)
                    {
                        SetEnabled();
                    }
                    else
                    {
                        SetDisbaled();
                    }
                }
            }

            public Int32 NumberPerGroup
            {
                get { return numberPerGroup; }
                set
                {
                    SetEnabled(value);
                }
            }

            public GroupInfo()
            {
                SetDisbaled();
            }

            public void SetEnabled(Int32 number = NoLimit)
            {
                enabled = true;
                if (number <= NoGroup)
                {
                    throw new Exception("分组的数量是个无效值");
                }
                numberPerGroup = number;
            }

            public void SetDisbaled()
            {
                enabled = false;
                numberPerGroup = NoGroup;
            }
        }
    };
};

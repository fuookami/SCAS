using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class GroupInfo
        {
            private SSUtils.EnabledNumberRange _numberPerGroup;

            public bool Enabled
            {
                get
                {
                    return _numberPerGroup.IsEnabled();
                }
                set
                {
                    if (Enabled != value)
                    {
                        if (value)
                        {
                            _numberPerGroup.SetEnabled();
                        }
                        else
                        {
                            _numberPerGroup.SetDisabled();
                        }
                    }
                }
            }

            public SSUtils.NumberRange NumberPerGroup
            {
                get
                {
                    return _numberPerGroup.Range;
                }
                set
                {
                    if (value != null)
                    {
                        SetEnabled(value);
                    }
                    else
                    {
                        SetDisabled();
                    }
                }
            }

            public GroupInfo()
            {
                _numberPerGroup = new SSUtils.EnabledNumberRange();
            }

            public void SetEnabled()
            {
                SetEnabled(new SSUtils.NumberRange());
            }

            public void SetEnabled(UInt32 number)
            {
                SetEnabled(new SSUtils.NumberRange(number, number));
            }

            public void SetEnabled(SSUtils.NumberRange range)
            {
                _numberPerGroup.SetEnabled(range ?? throw new Exception("设置的每组运动员数量范围是个无效值"));
            }

            public void SetDisabled()
            {
                _numberPerGroup.SetDisabled();
            }
        }
    };
};

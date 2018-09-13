using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class EntryValidator
        {
            private bool _enabledInTeamwork;
            private SSUtils.EnabledNumberRange _entryNumberPerAthlete;

            public bool Enabled
            {
                get
                {
                    return _entryNumberPerAthlete.IsEnabled();
                }
                set
                {
                    if (Enabled != value)
                    {
                        if (value)
                        {
                            _entryNumberPerAthlete.SetEnabled();
                        }
                        else
                        {
                            _entryNumberPerAthlete.SetDisabled();
                        }
                    }
                }
            }

            public bool EnabledInTeamwork
            {
                get
                {
                    return _enabledInTeamwork;
                }
                set
                {
                    if (_enabledInTeamwork != value)
                    {
                        if (value)
                        {
                            SetEnabledInTeamwork();
                        }
                        else
                        {
                            SetDisabledInTeamwork();
                        }
                    }
                }
            }

            public SSUtils.NumberRange EntryNumberPerAthlete
            {
                get
                {
                    return _entryNumberPerAthlete.Range;
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

            public EntryValidator()
            {
                _entryNumberPerAthlete = new SSUtils.EnabledNumberRange();
                SetDisabled();
            }

            public void SetEnabled()
            {
                SetEnabled(new SSUtils.NumberRange());
            }

            public void SetEnabled(UInt32 number)
            {
                SetEnabled(new SSUtils.NumberRange(0, number));
            }

            public void SetEnabled(SSUtils.NumberRange range)
            {
                _entryNumberPerAthlete.SetEnabled(range ?? throw new Exception("设置的每个运动员的项目报名数量范围是个无效值"));
            }

            public void SetDisabled()
            {
                SetDisabledInTeamwork();
                _entryNumberPerAthlete.SetDisabled();
            }

            public void SetEnabledInTeamwork()
            {
                if (!Enabled)
                {
                    SetEnabled();
                }
                _enabledInTeamwork = true;
            }

            public void SetDisabledInTeamwork()
            {
                _enabledInTeamwork = false;
            }
        };
    };
};

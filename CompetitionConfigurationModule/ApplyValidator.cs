using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class EntryValidator
        {
            private bool _enabledInTeamwork;
            private SSUtils.EnabledNumberRange _applicationNumberPerAthlete;

            public bool Enabled
            {
                get
                {
                    return _applicationNumberPerAthlete.IsEnabled();
                }
                set
                {
                    if (Enabled != value)
                    {
                        if (value)
                        {
                            _applicationNumberPerAthlete.SetEnabled();
                        }
                        else
                        {
                            _applicationNumberPerAthlete.SetDisabled();
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

            public SSUtils.NumberRange ApplicationNumberPerAthlete
            {
                get
                {
                    return _applicationNumberPerAthlete.Range;
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
                _applicationNumberPerAthlete.SetEnabled(range ?? throw new Exception("设置的每个运动员的项目报名数量范围是个无效值"));
            }

            public void SetDisabled()
            {
                SetDisabledInTeamwork();
                _applicationNumberPerAthlete.SetDisabled();
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

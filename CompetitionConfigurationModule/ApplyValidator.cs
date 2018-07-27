using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class ApplicationValidator
    {
        public const Int32 NotEnabled = -1;
        public const Int32 NoLimit = 0;

        private bool enabled;
        private bool enabledInTeamwork;
        private Int32 maxApplicationNumberPerTeam;

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
                    SetDisabled();
                }
            }
        }

        public bool EnabledInTeamwork
        {
            get { return enabledInTeamwork; }
            set
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

        public Int32 MaxApplicationNumberPerTeam
        {
            get { return maxApplicationNumberPerTeam; }
            set { SetEnabled(value); }
        }

        public ApplicationValidator()
        {
            SetDisabled();
        }

        public void SetEnabled(Int32 maxApplicationNumber = NoLimit)
        {
            if (maxApplicationNumber <= NotEnabled)
            {
                throw new Exception("每个运动员填报项目的上限数量是个无效值");
            }

            if (!enabled)
            {
                enabled = true;
            }
            maxApplicationNumberPerTeam = maxApplicationNumber;
        }

        public void SetDisabled()
        {
            enabled = false;
            SetDisabledInTeamwork();
            maxApplicationNumberPerTeam = NotEnabled;
        }

        public void SetEnabledInTeamwork()
        {
            if (!enabled)
            {
                SetEnabled();
            }
            enabledInTeamwork = true;
        }

        public void SetDisabledInTeamwork()
        {
            enabledInTeamwork = false;
        }
    }
}

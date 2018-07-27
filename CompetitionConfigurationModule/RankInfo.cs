using System;

namespace CompetitionConfigurationModule
{
    public class RankInfo
    {
        private bool enabled;
        private bool forced;
        private AthleteRankPool athleteRanks;
        private AthleteRank defaultRank;

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

        public bool Forced
        {
            get { return forced; }
            set
            {
                if (value)
                {
                    if (athleteRanks.Count == 0)
                    {
                        throw new Exception("要设置运动员强制分级，运动员级别列表不能为空");
                    }
                    SetForced(athleteRanks[0]);
                }
                else
                {
                    SetUnforced();
                }
            }
        }

        public AthleteRankPool AthleteRanks
        {
            get { return athleteRanks; }
        }

        public AthleteRank DefaultRank
        {
            get { return defaultRank; }
            set
            {
                if (!athleteRanks.Contains(value))
                {
                    throw new Exception("不能设置非运动员级别列表的运动员级别数据作为运动员默认级别");
                }
                SetForced(value);
            }
        }

        public RankInfo()
        {
            SetDisabled();
        }

        public void SetEnabled()
        {
            enabled = true;
        }

        public void SetDisabled()
        {
            enabled = false;
            forced = false;
            athleteRanks.Clear();
            defaultRank = null;
        }

        public void SetForced(AthleteRank rank)
        {
            enabled = true;
            forced = true;
            defaultRank = rank;
        }

        public void SetUnforced()
        {
            if (enabled)
            {
                forced = false;
                defaultRank = null;
            }
        }
    }
}

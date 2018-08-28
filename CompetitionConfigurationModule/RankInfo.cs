using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class RankInfo
        {
            private bool _enabled;
            private bool _forced;
            private AthleteRank _defaultRank;

            public bool Enabled
            {
                get
                {
                    return _enabled;
                }
                set
                {
                    if (_enabled != value)
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
            }

            public bool Forced
            {
                get
                {
                    return _forced;
                }
                set
                {
                    if (_forced != value)
                    {
                        if (value)
                        {
                            if (AthleteRanks.Count == 0)
                            {
                                throw new Exception("要设置运动员强制分级，运动员级别列表不能为空");
                            }
                            SetForced(AthleteRanks[0]);
                        }
                        else
                        {
                            SetUnforced();
                        }
                    }
                }
            }

            public AthleteRankPool AthleteRanks
            {
                get;
            }

            public AthleteRank DefaultRank
            {
                get
                {
                    return _defaultRank;
                }
                set
                {
                    if (!AthleteRanks.Contains(value))
                    {
                        throw new Exception("不能设置非运动员级别列表的运动员级别数据作为运动员默认级别");
                    }
                    SetForced(value);
                }
            }

            public RankInfo()
            {
                AthleteRanks = new AthleteRankPool();
                SetDisabled();
            }

            public void SetEnabled()
            {
                _enabled = true;
            }

            public void SetDisabled()
            {
                _enabled = false;
                _forced = false;
                AthleteRanks.Clear();
                _defaultRank = null;
            }

            public void SetForced(AthleteRank rank)
            {
                _enabled = true;
                _forced = true;
                _defaultRank = rank;
            }

            public void SetUnforced()
            {
                if (_enabled)
                {
                    _forced = false;
                    _defaultRank = null;
                }
            }
        };
    };
};

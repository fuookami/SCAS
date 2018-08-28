using System;
using System.Collections.Generic;
using SSUtils;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class TeamworkInfo
        {
            private bool _beTeamwork;
            private bool _beMultiRank;
            private bool _needEveryPerson;

            private NumberRange _rangesOfTeam

            public bool BeTeamwork
            {
                get
                {
                    return _beTeamwork;
                }
                set
                {
                    if (_beTeamwork != value)
                    {
                        if (value)
                        {
                            SetIsTeamwork();
                        }
                        else
                        {
                            SetIsNotTeamwork();
                        }
                    }
                }
            }

            public bool BeMultiRank
            {
                get
                {
                    return _beMultiRank;
                }
                set
                {
                    if (BeTeamwork)
                    {
                        _beMultiRank = value;
                    }
                }
            }

            public bool NeedEveryPerson
            {
                get
                {
                    return _needEveryPerson;
                }
                set
                {
                    if (_needEveryPerson != value)
                    {
                        if (value)
                        {
                            SetNeedEveryPerson();
                        }
                        else
                        {
                            SetNotNeedEveryPerson();
                        }
                    }
                }
            }

            public Dictionary<AthleteCategory, NumberRange> RangesOfCategories
            {
                get;
                private set;
            }

            public NumberRange RangesOfTeam
            {
                get
                {
                    return _rangesOfTeam;
                }
                set
                {
                    if (_needEveryPerson)
                    {
                        _rangesOfTeam = value ?? throw new Exception("设置的队伍人数是个无效值");
                    }
                }
            }

            public TeamworkInfo()
            {
                SetIsNotTeamwork();
            }

            public void SetIsTeamwork()
            {
                if (!_beTeamwork)
                {
                    _beTeamwork = true;
                }
            }

            public void SetIsNotTeamwork()
            {
                SetNotNeedEveryPerson();

                _beTeamwork = false;
                _beMultiRank = false;
            }

            public void SetNeedEveryPerson()
            {
                if (!_needEveryPerson)
                {
                    SetIsTeamwork();
                    _needEveryPerson = true;

                    RangesOfCategories = new Dictionary<AthleteCategory, NumberRange>();
                    RangesOfTeam = new NumberRange();
                }
            }

            public void SetNotNeedEveryPerson()
            {
                _needEveryPerson = false;

                RangesOfCategories = null;
                RangesOfTeam = null;
            }
        };
    };
};

using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class UInt32Range
        {
            public const UInt32 NoLimit = 0;

            public UInt32 Minimun;
            public UInt32 Maximun;

            public UInt32Range(UInt32 min = 0, UInt32 max = 0)
            {
                Minimun = min;
                Maximun = max;
            }

            public bool Valid()
            {
                return Maximun == 0
                    || Minimun <= Maximun;
            }
        }

        public class TeamworkInfo
        {
            private bool beTeamwork;
            private bool beMultiRank;
            private bool needEveryPerson;
            private Dictionary<AthleteCategory, UInt32Range> rangesOfCategories;
            private UInt32Range rangesOfTeam;

            public bool BeTeamwork
            {
                get { return beTeamwork; }
                set
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

            public bool BeMultiRank
            {
                get { return beMultiRank; }
                set
                {
                    if (BeTeamwork)
                    {
                        beMultiRank = value;
                    }
                }
            }

            public bool NeedEveryPerson
            {
                get { return needEveryPerson; }
                set
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

            public Dictionary<AthleteCategory, UInt32Range> RangesOfCategories
            {
                get { return rangesOfCategories; }
                set
                {
                    if (NeedEveryPerson)
                    {
                        rangesOfCategories = (value ?? new Dictionary<AthleteCategory, UInt32Range>());
                    }
                }
            }

            public UInt32Range RangesOfTeam
            {
                get { return rangesOfTeam; }
                set
                {
                    if (NeedEveryPerson)
                    {
                        rangesOfTeam = (value ?? new UInt32Range());
                    }
                }
            }

            public TeamworkInfo()
            {
                SetIsNotTeamwork();
            }

            public void SetIsTeamwork()
            {
                if (!beTeamwork)
                {
                    beTeamwork = true;
                }
            }

            public void SetIsNotTeamwork()
            {
                SetNotNeedEveryPerson();

                beTeamwork = false;
                beMultiRank = false;
            }

            public void SetNeedEveryPerson()
            {
                if (!needEveryPerson)
                {
                    SetIsTeamwork();
                    needEveryPerson = true;

                    rangesOfCategories = new Dictionary<AthleteCategory, UInt32Range>();
                    rangesOfTeam = new UInt32Range();
                }
            }

            public void SetNotNeedEveryPerson()
            {
                needEveryPerson = false;

                rangesOfCategories = null;
                rangesOfTeam = null;
            }
        };
    };
};

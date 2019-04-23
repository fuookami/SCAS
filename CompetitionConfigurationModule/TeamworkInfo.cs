using System;
using System.Collections.Generic;
using SSUtils;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public partial class TeamworkInfo
        {
            public bool BeTeamwork 
            { 
                get { return _beTeamwork; } 
                internal set { SetIsTeamwork(value); } 
            }
            
            public bool BeMultiRank 
            { 
                get { return _beMultiRank; } 
                internal set { _beMultiRank = BeTeamwork ? value : false; } 
            }

            public bool NeedEveryPerson 
            { 
                get { return _needEveryPerson; } 
                internal set { SetNeedEveryPerson(value); } 
            }

            public Dictionary<AthleteCategory, NumberRange> RangesOfCategories 
            { 
                get; 
                private set; 
            }

            public NumberRange RangesOfTeam 
            { 
                get { return _rangesOfTeam; } 
                internal set { _rangesOfTeam = !_needEveryPerson ? null : value ?? throw new Exception("设置的队伍人数是个无效值"); } 
            }

            public Boolean BeInOrder
            { 
                get; 
                private set; 
            }

            public List<AthleteCategory> Order
            { 
                get;
                private set;
            }

            public TeamworkInfo()
            {
                SetIsNotTeamwork();
            }
        };
    };
};

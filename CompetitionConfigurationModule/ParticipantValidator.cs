using System;
using SSUtils;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class ParticipantValidator
        {
            private AthleteCategoryList _categories;
            private AthleteRankList _ranks;

            public AthleteCategoryList Categories
            {
                get
                {
                    return _categories;
                }
                set
                {
                    if (value == null || value.Count == 0)
                    {
                        throw new Exception("不能将运动员类别列表设置为空列表");
                    }
                    _categories = value;
                }
            }

            public AthleteRankList Ranks
            {
                get
                {
                    return _ranks;
                }
                set
                {
                    if (value == null || value.Count == 0)
                    {
                        throw new Exception("不能将运动员级别列表设置为空列表");
                    }
                    _ranks = value;
                }
            }

            public NumberRange NumberPerTeam
            {
                get;
            }

            public bool BePointForEveryRank
            {
                get;
                set;
            }

            public ParticipantValidator()
            {
                _categories = new AthleteCategoryList();
                _ranks = new AthleteRankList();
                NumberPerTeam = new NumberRange();
                BePointForEveryRank = false;
            }
        }
    };
};

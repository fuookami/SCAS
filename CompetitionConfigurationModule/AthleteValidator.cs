using System;

namespace CompetitionConfigurationModule
{
    class AthleteValidator
    {
        public const UInt32 NoLimit = 0;

        private AthleteCategoryList categories;
        private AthleteRankList ranks;
        private UInt32 maxNumberPerTeam;

        public AthleteCategoryList Categories
        {
            get { return categories; }
            set
            {
                if (value.Count == 0)
                {
                    throw new Exception("不能将运动员类别列表设置为空列表");
                }
                categories = value;
            }
        }

        public AthleteRankList Ranks
        {
            get { return ranks; }
            set
            {
                if (value.Count == 0)
                {
                    throw new Exception("不能将运动员级别列表设置为空列表");
                }
                ranks = value;
            }
        }

        public UInt32 MaxNumberOfPeoplePerTeam
        {
            get { return maxNumberPerTeam; }
            set { maxNumberPerTeam = value; }
        }

        public AthleteValidator()
        {
            maxNumberPerTeam = NoLimit;
        }
    }
}

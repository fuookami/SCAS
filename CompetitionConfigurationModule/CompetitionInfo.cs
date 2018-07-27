using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class CompetitionInfo
    {
        private String id;
        private String name;
        private String subName;
        private String version;
        private String identifier;
        private bool isTemplate;

        private ApplicationValidator applicationValidator;
        private PrincipalInfo principalInfo;
        private PointInfo publicPointInfo;
        private List<Date> dates;

        private AthleteCategoryPool athleteCategories;
        private RankInfo rankInfo;
        private TeamCategoryPool teamCategories;
        private TeamInfoPool teamInfos;

        private Dictionary<Date, GameInfoList> gameInfos;
        private Dictionary<String, EventInfo> eventInfos;

        public String Id
        {
            get { return id; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String SubName
        {
            get { return subName; }
            set { subName = value; }
        }

        public String Version
        {
            get { return version; }
            set { version = value; }
        }

        public String Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        public bool IsTemplate
        {
            get { return isTemplate; }
            set { isTemplate = value; }
        }

        public ApplicationValidator CompetitionApplicationValidator
        {
            get { return applicationValidator; }
            set { applicationValidator = value ?? throw new Exception("传入的报名限制信息无效"); }
        }

        public PrincipalInfo CompetitionPrincipalInfo
        {
            get { return principalInfo; }
            set { principalInfo = value ?? throw new Exception("传入的负责人信息无效"); }
        }

        public PointInfo PublicPointInfo
        {
            get { return publicPointInfo; }
            set { publicPointInfo = value ?? throw new Exception("传入的默认积分信息无效"); }
        }

        public List<Date> Dates
        {
            get { return dates; }
            set
            {
                dates = value;
                var newGameInfos = new Dictionary<Date, GameInfoList>();
                foreach (var date in dates)
                {
                    newGameInfos[date] = gameInfos.ContainsKey(date) ? gameInfos[date] : new GameInfoList();
                }
                gameInfos = newGameInfos;
            }
        }

        public AthleteCategoryPool AthleteCategories
        {
            get { return athleteCategories; }
        }

        public RankInfo CompetitionRankInfo
        {
            get { return rankInfo; }
        }

        public TeamCategoryPool TeamCategories
        {
            get { return teamCategories; }
        }

        public TeamInfoPool TeamInfos
        {
            get { return teamInfos; }
        }

        public Dictionary<Date, GameInfoList> GameInfos
        {
            get { return gameInfos; }
        }

        public Dictionary<String, EventInfo> EventInfos
        {
            get { return eventInfos; }
        }

        public CompetitionInfo()
            : this(Guid.NewGuid().ToString("N")) { }

        public CompetitionInfo(String existedId)
        {
            id = existedId;
            isTemplate = false;

            applicationValidator = new ApplicationValidator();
            principalInfo = new PrincipalInfo();
            publicPointInfo = new PointInfo();
            dates = new List<Date>();

            athleteCategories = new AthleteCategoryPool();
            rankInfo = new RankInfo();
            teamCategories = new TeamCategoryPool();
            teamInfos = new TeamInfoPool();

            gameInfos = new Dictionary<Date, GameInfoList>();
            eventInfos = new Dictionary<String, EventInfo>();
        }
    }
}

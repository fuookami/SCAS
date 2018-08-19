using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class CompetitionInfo
        {
            private String id;
            private String name;
            private String subName;
            private String version;
            private String identifier;
            private bool beTemplate;

            private EntryValidator applicationValidator;
            private PrincipalInfo principalInfo;
            private PointInfo publicPointInfo;
            private SessionPool sessions;
            private String field;

            private AthleteCategoryPool athleteCategories;
            private RankInfo rankInfo;
            private TeamCategoryPool teamCategories;
            private TeamInfoPool teamInfos;

            private List<EventInfo> eventInfos;
            private Dictionary<Session, GameInfoList> gameInfos;

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

            public bool BeTemplate
            {
                get { return beTemplate; }
                set { beTemplate = value; }
            }

            public EntryValidator CompetitionApplicationValidator
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

            public SessionPool Sessions
            {
                get { return sessions; }
                set
                {
                    sessions = value;
                    var newGameInfos = new Dictionary<Session, GameInfoList>();
                    foreach (var date in sessions)
                    {
                        foreach (var session in date.Value)
                        {
                            newGameInfos[session] = gameInfos.ContainsKey(session) ? gameInfos[session] : new GameInfoList();
                        }
                    }
                    gameInfos = newGameInfos;
                }
            }

            public String Field
            {
                get { return field; }
                set { field = value; }
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
            public List<EventInfo> EventInfos
            {
                get { return eventInfos; }
            }

            public Dictionary<Session, GameInfoList> GameInfos
            {
                get { return gameInfos; }
            }

            public CompetitionInfo()
                : this(Guid.NewGuid().ToString("N")) { }

            public CompetitionInfo(String existedId)
            {
                id = existedId;
                beTemplate = false;

                applicationValidator = new EntryValidator();
                principalInfo = new PrincipalInfo();
                publicPointInfo = new PointInfo();
                sessions = new SessionPool();

                athleteCategories = new AthleteCategoryPool();
                rankInfo = new RankInfo();
                teamCategories = new TeamCategoryPool();
                teamInfos = new TeamInfoPool();

                gameInfos = new Dictionary<Session, GameInfoList>();
                eventInfos = new List<EventInfo>();
            }

            public EventInfo GenerateNewEventInfo()
            {
                return new EventInfo(this);
            }
        }
    };
};

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

            private UInt32 displayBeginLine;
            private UInt32 numberOfDisplayLines;
            private List<UInt32> useLines;

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

            public UInt32 DisplayBeginLine
            {
                get { return displayBeginLine; }
            }

            public UInt32 NumberOfDisplayLines
            {
                get { return numberOfDisplayLines; }
            }

            public UInt32 UseBeginLine
            {
                get { return useLines[0]; }
            }

            public UInt32 NumberOfUseLines
            {
                get { return (UInt32)useLines.Count; }
            }

            public List<UInt32> UseLines
            {
                get { return useLines; }
                set
                {
                    UInt32 maxLine = displayBeginLine + numberOfDisplayLines;
                    if (value == null || value.Count == 0)
                    {
                        throw new Exception("传入的使用道次信息是个非法值");
                    }
                    value.Sort();
                    foreach (UInt32 line in value)
                    {
                        if (line > maxLine)
                        {
                            throw new Exception(String.Format("有非法的道次值{0}", line));
                        }
                    }
                    useLines = value;
                }
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

                useLines = new List<UInt32>();
                SetLineConfiguration();

                gameInfos = new Dictionary<Session, GameInfoList>();
                eventInfos = new List<EventInfo>();
            }

            public EventInfo GenerateNewEventInfo()
            {
                return new EventInfo(this);
            }

            public bool SetLineConfiguration(UInt32 useBeginLine = 1, UInt32 numberOfUseLines = 8)
            {
                return SetLineConfiguration(useBeginLine, numberOfUseLines, useBeginLine, numberOfUseLines);
            }

            public bool SetLineConfiguration(UInt32 useBeginLine, UInt32 numberOfUseLines, UInt32 displayBeginLine, UInt32 numberOfDisplayLines)
            {
                if (numberOfUseLines == 0 || numberOfDisplayLines == 0 || (useBeginLine < displayBeginLine)
                    || ((useBeginLine + numberOfUseLines) > (displayBeginLine + numberOfDisplayLines)))
                {
                    return false;
                }

                this.displayBeginLine = displayBeginLine;
                this.numberOfDisplayLines = numberOfDisplayLines;

                useLines.Clear();
                for (UInt32 i = 0; i != numberOfUseLines; ++i)
                {
                    useLines.Add(useBeginLine + i);
                }

                return true;
            }

            public bool SetLineConfiguration(List<UInt32> useLines, UInt32 displayBeginLine, UInt32 numberOfDisplayLines)
            {
                if (useLines == null || useLines.Count == 0 || numberOfDisplayLines == 0)
                {
                    return false;
                }

                this.displayBeginLine = displayBeginLine;
                this.numberOfDisplayLines = numberOfDisplayLines;
                this.UseLines = useLines;

                return true;
            }
        }
    };
};

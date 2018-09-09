using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class CompetitionInfo
        {
            private SessionPool _sessions;

            private List<UInt32> _useLines;

            public String Id
            {
                get;
                internal set;
            }

            public String Name
            {
                get;
                set;
            }

            public String SubName
            {
                get;
                set;
            }

            public String Version
            {
                get;
                set;
            }

            public String Identifier
            {
                get;
                set;
            }

            public UInt32 Order
            {
                get;
                set;
            }

            public bool BeTemplate
            {
                get;
                set;
            }

            public Date EntryClosingDate
            {
                get;
                set;
            }

            public SSUtils.NumberRange NumberOfSubLeader
            {
                get;
            }

            public Boolean CoachOptional
            {
                get;
                set;
            }

            public EntryValidator CompetitionEntryValidator
            {
                get;
            }

            public PrincipalInfo CompetitionPrincipalInfo
            {
                get;
            }

            public PointInfo PublicPointInfo
            {
                get;
            }

            public SessionPool Sessions
            {
                get
                {
                    return _sessions;
                }
                set
                {
                    _sessions = value;
                    RefreshGameInfos();
                }
            }

            public String Field
            {
                get;
                set;
            }

            public AthleteCategoryPool AthleteCategories
            {
                get;
            }

            public RankInfo CompetitionRankInfo
            {
                get;
            }

            public TeamCategoryPool TeamCategories
            {
                get;
            }

            public TeamInfoPool TeamInfos
            {
                get;
            }

            public UInt32 DisplayBeginLine
            {
                get;
                private set;
            }

            public UInt32 NumberOfDisplayLines
            {
                get;
                private set;
            }

            public UInt32 UseBeginLine
            {
                get { return _useLines[0]; }
            }

            public UInt32 NumberOfUseLines
            {
                get { return (UInt32)_useLines.Count; }
            }

            public List<UInt32> UseLines
            {
                get { return _useLines; }
                set
                {
                    UInt32 maxLine = DisplayBeginLine + NumberOfDisplayLines;
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
                    _useLines = value;
                }
            }

            public List<EventInfo> EventInfos
            {
                get;
            }

            public Dictionary<Session, GameInfoList> GameInfos
            {
                get;
                private set;
            }

            public CompetitionInfo()
                : this(Guid.NewGuid().ToString("N")) { }

            public CompetitionInfo(String existedId)
            {
                Id = existedId;
                BeTemplate = false;
                Field = "";
                EntryClosingDate = new Date();
                NumberOfSubLeader = new SSUtils.NumberRange(1, 2);
                CoachOptional = false;

                CompetitionEntryValidator = new EntryValidator();
                CompetitionPrincipalInfo = new PrincipalInfo();
                PublicPointInfo = new PointInfo();
                _sessions = new SessionPool();

                AthleteCategories = new AthleteCategoryPool();
                CompetitionRankInfo = new RankInfo();
                TeamCategories = new TeamCategoryPool();
                TeamInfos = new TeamInfoPool();

                _useLines = new List<UInt32>();
                SetLineConfiguration();

                GameInfos = new Dictionary<Session, GameInfoList>();
                EventInfos = new List<EventInfo>();
            }

            public EventInfo GenerateNewEventInfo()
            {
                return new EventInfo(this);
            }

            public void RefreshGameInfos()
            {
                var newGameInfos = new Dictionary<Session, GameInfoList>();
                foreach (var date in _sessions)
                {
                    foreach (var session in date.Value)
                    {
                        newGameInfos[session] = GameInfos.ContainsKey(session) ? GameInfos[session] : new GameInfoList();
                    }
                }
                GameInfos = newGameInfos;
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

                DisplayBeginLine = displayBeginLine;
                NumberOfDisplayLines = numberOfDisplayLines;

                UseLines.Clear();
                for (UInt32 i = 0; i != numberOfUseLines; ++i)
                {
                    UseLines.Add(useBeginLine + i);
                }

                return true;
            }

            public bool SetLineConfiguration(List<UInt32> useLines, UInt32 displayBeginLine, UInt32 numberOfDisplayLines)
            {
                if (useLines == null || useLines.Count == 0 || numberOfDisplayLines == 0)
                {
                    return false;
                }

                DisplayBeginLine = displayBeginLine;
                NumberOfDisplayLines = numberOfDisplayLines;
                UseLines = useLines;

                return true;
            }
        }
    };
};

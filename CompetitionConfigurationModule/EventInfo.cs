using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class EventInfo
        {
            private String id;
            private String name;

            private GradeInfo gradeInfo;
            private TeamworkInfo teamworkInfo;
            private AthleteValidator athleteValidator;
            private PointInfo pointInfo;
            private TeamInfoList enabledTeams;

            private CompetitionInfo competitionInfo;
            private GameInfoPool gameInfos;

            public String Id
            {
                get { return id; }
            }

            public String Name
            {
                get { return name; }
                set { name = value; }
            }

            public GradeInfo EventGradeInfo
            {
                get { return gradeInfo; }
                set { gradeInfo = value ?? throw new Exception("设置的积分类型是个无效值"); ; }
            }

            public TeamworkInfo EventTeamworkInfo
            {
                get { return teamworkInfo; }
                set { teamworkInfo = value ?? throw new Exception("设置的团体项目信息是个无效值"); ; }
            }

            public AthleteValidator EventAthleteValidator
            {
                get { return athleteValidator; }
                set { athleteValidator = value ?? throw new Exception("设置的运动员报名限制信息是个无效值"); ; }
            }

            public PointInfo EventPointInfo
            {
                get { return pointInfo; }
                set { pointInfo = value ?? throw new Exception("设置的积分信息是个无效值"); ; }
            }

            public TeamInfoList EnabledTeams
            {
                get { return enabledTeams; }
            }

            public CompetitionInfo Competition
            {
                get { return competitionInfo; }
            }

            public GameInfoPool GameInfos
            {
                get { return gameInfos; }
                set
                {
                    if (value.Count == 0)
                    {
                        throw new Exception("不能将比赛列表设置为空列表");
                    }
                    gameInfos = value;
                }
            }

            public EventInfo(CompetitionInfo competition = null)
                : this(Guid.NewGuid().ToString("N"), competition) { }

            public EventInfo(String existedId, CompetitionInfo competition = null)
            {
                id = existedId;
                gradeInfo = new GradeInfo();
                teamworkInfo = new TeamworkInfo();
                athleteValidator = new AthleteValidator();
                pointInfo = competition == null ? new PointInfo() : (PointInfo)competition.PublicPointInfo.Clone();
                enabledTeams = new TeamInfoList();

                competitionInfo = competition;
                if (competition != null)
                {
                    competition.EventInfos.Add(this);
                }
                gameInfos = new GameInfoPool(this);
            }

            public bool IsTemplate()
            {
                return competitionInfo == null;
            }
        };
    };
};

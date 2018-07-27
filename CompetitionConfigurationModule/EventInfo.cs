using System;

namespace CompetitionConfigurationModule
{
    public class EventInfo
    {
        public enum EventType
        {
            Dual,
            Ranking
        }

        private String id;
        private String name;
        private EventType type;

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

        public EventType Type
        {
            get { return type; }
            set { type = value; }
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

        public AthleteValidator EvenetAthleteValidator
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

        public EventInfo(CompetitionInfo competition)
            : this(competition, Guid.NewGuid().ToString("N")) { }

        public EventInfo(CompetitionInfo competition, String existedId)
        {
            if (competition == null)
            {
                throw new Exception("设置的父竞赛是个无效值");
            }

            id = existedId;
            gradeInfo = new GradeInfo();
            teamworkInfo = new TeamworkInfo();
            athleteValidator = new AthleteValidator();
            pointInfo = new PointInfo();
            enabledTeams = new TeamInfoList();

            competitionInfo = competition;
            gameInfos = new GameInfoPool(this);
        }
    }
}

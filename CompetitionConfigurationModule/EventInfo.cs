using System;

namespace CompetitionConfigurationModule
{
    class EventInfo
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
            set { gradeInfo = value; }
        }

        public TeamworkInfo EventTeamworkInfo
        {
            get { return teamworkInfo; }
            set { teamworkInfo = value; }
        }

        public AthleteValidator EvenetAthleteValidator
        {
            get { return athleteValidator; }
            set { athleteValidator = value; }
        }

        public PointInfo EventPointInfo
        {
            get { return pointInfo; }
            set { pointInfo = value; }
        }

        public EventInfo()
            : this(Guid.NewGuid().ToString("N")) { }

        public EventInfo(String existedId)
        {
            id = existedId;
            gradeInfo = new GradeInfo();
            teamworkInfo = new TeamworkInfo();
            athleteValidator = new AthleteValidator();
            pointInfo = new PointInfo();
        }
    }
}

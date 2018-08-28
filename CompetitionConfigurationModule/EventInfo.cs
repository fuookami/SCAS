using System;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class EventInfo
        {
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

            public GradeInfo EventGradeInfo
            {
                get;
            }

            public TeamworkInfo EventTeamworkInfo
            {
                get;
            }

            public ParticipantValidator EventParticipantValidator
            {
                get;
            }

            public PointInfo EventPointInfo
            {
                get;
            }

            public TeamInfoList EnabledTeams
            {
                get;
            }

            public CompetitionInfo Competition
            {
                get;
            }

            public GameInfoPool GameInfos
            {
                get;
            }

            public EventInfo(CompetitionInfo competition = null)
                : this(Guid.NewGuid().ToString("N"), competition) { }

            public EventInfo(String existedId, CompetitionInfo competition = null)
            {
                Id = existedId;
                EventGradeInfo = new GradeInfo();
                EventTeamworkInfo = new TeamworkInfo();
                EventParticipantValidator = new ParticipantValidator();
                EventPointInfo = competition == null ? new PointInfo() : (PointInfo)competition.PublicPointInfo.Clone();
                EnabledTeams = new TeamInfoList();

                Competition = competition;
                if (competition != null)
                {
                    competition.EventInfos.Add(this);
                }
                GameInfos = new GameInfoPool(this);
            }

            public bool IsTemplate()
            {
                return Competition == null;
            }
        };
    };
};

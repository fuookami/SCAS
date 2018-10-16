using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Participant
        {
            public String Id
            {
                get;
                internal set;
            }

            public Team ParticipantTeam
            {
                get;
            }

            public List<Athlete> Athletes
            {
                get;
                private set;
            }

            public Dictionary<Athlete, SSUtils.Order> OrdersOfAthletes
            {
                get;
                private set;
            }

            public Grade ParticipantGrade
            {
                get;
                set;
            }
            
            public TimeSpan BestGrade
            {
                get;
                set;
            }

            public Participant(Team team, List<Athlete> athletesInOrder, String existedId = null)
            {
                Id = existedId ?? Guid.NewGuid().ToString("N");
                ParticipantTeam = team;
                ParticipantGrade = new Grade(this);
                BestGrade = TimeSpan.Zero;
                Set(athletesInOrder);
            }

            public void Set(List<Athlete> athletesInOrder)
            {
                Athletes = athletesInOrder ?? throw new Exception("设置的参赛运动员为无效值");

                OrdersOfAthletes = new Dictionary<Athlete, SSUtils.Order>();
                for (Int32 i = 0, j = athletesInOrder.Count; i != j; ++i)
                {
                    OrdersOfAthletes.Add(athletesInOrder[i], new SSUtils.Order(i));
                }
            }
        }
    };
};

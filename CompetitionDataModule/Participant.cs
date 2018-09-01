using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Participant
        {
            public Team ParticipantTeam
            {
                get;
            }

            public List<Athlete> Athletes
            {
                get;
                private set;
            }

            public Map<Athlete, SSUtils.Order> OrdersOfAthletes
            {
                get;
                private set;
            }

            public Grade ParticipantGrade
            {
                get;
                set;
            }

            public Participant(Team team, List<Athlete> athletesInOrder)
            {
                ParticipantTeam = team;
                Set(athletesInOrder);
            }

            public void Set(List<Athlete> athletesInOrder)
            {
                Athletes = athletesInOrder ?? throw new Exception("设置的参赛运动员为无效值");

                OrdersOfAthletes = new Map<Athlete, SSUtils.Order>();
                for (Int32 i = 0, j = athletesInOrder.Count; i != j; ++i)
                {
                    OrdersOfAthletes.Add(athletesInOrder[i], new SSUtils.Order(i));
                }
            }
        }
    };
};

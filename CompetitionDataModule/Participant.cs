using System;
using System.Collections.Generic;
using SSUtils;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Participant
        {
            private Line _parent;

            public String Id
            {
                get;
                internal set;
            }

            public String Name
            {
                get;
                internal set;
            }

            public Team ParticipantTeam
            {
                get;
            }

            public Order OrderInTeam
            {
                get;
                internal set;
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

            public Line Parent
            {
                get
                {
                    return _parent;
                }
                internal set
                {
                    _parent = value ?? throw new Exception("安排的道次是个无效值");
                }
            }

            public Participant(Team team, List<Athlete> athletesInOrder, String existedId = null)
            {
                _parent = null;
                Id = existedId ?? Guid.NewGuid().ToString("N");
                Name = "";
                OrderInTeam = new Order();
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

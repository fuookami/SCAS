using System;
using System.Collections.Generic;
using System.Linq;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Leader
        {
            public String Sid
            {
                get;
                internal set;
            }

            public String Name
            {
                get;
                internal set;
            }

            public String Telephone
            {
                get;
                internal set;
            }

            public String EMail
            {
                get;
                internal set;
            }
        }

        public class Team
        {
            public TeamInfo Conf
            {
                get;
                private set;
            }

            public Leader TeamLeader
            {
                get;
            }

            public List<Leader> TeamCoaches
            {
                get;
            }

            public List<Leader> TeamSubLeaders
            {
                get;
            }

            public AthletePool Athletes
            {
                get;
            }

            public Dictionary<Event, List<Point>> Points
            {
                get;
            }

            public UInt32 TotalPoints
            {
                get
                {
                    return (UInt32)Points.Sum((ele1) => ele1.Value.Sum((ele2) => ele2.PointValue));
                }
            }

            public Team(TeamInfo conf)
            {
                Conf = conf;
                Athletes = new AthletePool(this, String.Format("{0:D2}", Conf.Order.Value));
                Points = new Dictionary<Event, List<Point>>();
                TeamLeader = new Leader();
                TeamCoaches = new List<Leader>();
                TeamSubLeaders = new List<Leader>();
            }
        }
    };
};

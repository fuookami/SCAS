using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Team
        {
            public TeamInfo Conf
            {
                get;
                private set;
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
                    //! to do: 
                    return 0;
                }
            }

            public Team(TeamInfo conf)
            {
                Conf = conf;
                Athletes = new AthletePool(this, String.Format("{0:D2}", Conf.Order.Value));
                Points = new Dictionary<Event, List<Point>>();
            }
        }
    };
};

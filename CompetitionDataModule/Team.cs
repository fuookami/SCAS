﻿using System;
using System.Collections.Generic;
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
                    //! to do: 
                    return 0;
                }
            }

            public Team(TeamInfo conf)
            {
                Conf = conf;
                Athletes = new AthletePool(this, String.Format("{0:D2}", Conf.Order.Value));
                Points = new Dictionary<Event, List<Point>>();
                TeamLeader = new Leader();
                TeamSubLeaders = new List<Leader>();
            }
        }
    };
};

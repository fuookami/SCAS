using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Event
        {
            public EventInfo Conf
            {
                get;
                private set;
            }

            public Grade MatchRecord
            {
                get;
                set;
            }

            public List<Game> Games
            {
                get;
                internal set;
            }

            public PointPool Points
            {
                get;
            }

            public Event(EventInfo conf, Grade matchRecord)
            {
                Conf = conf;
                MatchRecord = matchRecord;
                Games = new List<Game>();
                Points = new PointPool(conf.EventPointInfo, this);
            }
        }
    };
};

using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Competition
        {
            public CompetitionInfo Conf
            {
                get;
                private set;
            }

            public Dictionary<Session, FieldInfo> FieldInfos
            {
                get;
                internal set;
            }

            public List<Event> Events
            {
                get;
                internal set;
            }

            public Dictionary<Session, Game> Games
            {
                get;
                internal set;
            }

            public List<Team> Teams
            {
                get;
                internal set;
            }

            public Competition(CompetitionInfo conf)
            {
                Conf = conf;
                FieldInfos = new Dictionary<Session, FieldInfo>();
                Events = new List<Event>();
                Games = new Dictionary<Session, Game>();
                Teams = new List<Team>();
            }
        }
    };
};

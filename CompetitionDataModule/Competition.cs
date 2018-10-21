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

            public Dictionary<Session, List<Game>> Games
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
                Games = new Dictionary<Session, List<Game>>();
                Teams = new List<Team>();

                foreach (var dateSessions in conf.Sessions)
                {
                    foreach (var session in dateSessions.Value)
                    {
                        FieldInfos.Add(session, new FieldInfo(session));
                        Games.Add(session, new List<Game>());
                    }
                }
            }
        }
    };
};

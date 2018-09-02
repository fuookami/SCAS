using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace EntryBlank
    {
        public class Athlete
        {
            public String Sid
            {
                get;
            }

            public String Name
            {
                get;
            }

            public String Type
            {
                get;
            }

            public String Rank
            {
                get;
                set;
            }

            public bool Optional
            {
                get;
                internal set;
            }

            internal Athlete(bool optional = false)
            {
                Optional = optional;
            }

            internal Athlete(String sid, String name, String type, bool optional = false, String rank = "")
            {
                Sid = sid;
                Name = name;
                Type = type;
                Optional = optional;
                Rank = rank;
            }
        }

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

            public bool Optional
            {
                get;
                internal set;
            }

            internal Leader(bool optional = false)
            {
                Optional = optional;
            }

            internal Leader(String sid, String name, String telephone, String email, bool optional = false)
            {
                Sid = sid;
                Name = name;
                Telephone = telephone;
                EMail = email;
                Optional = optional;
            }
        }

        public class Entry
        {
            public EventInfo Conf
            {
                get;
            }

            public List<KeyValuePair<String, List<Athlete>>> Athletes
            {
                get;
            }

            internal Entry(EventInfo conf)
            {
                Conf = conf;
                Athletes = new List<KeyValuePair<String, List<Athlete>>>();
            }
        }

        public class EntryBlank
        {
            public CompetitionInfo Conf
            {
                get;
            }

            public TeamInfo Team
            {
                get;
                internal set;
            }

            public Leader TeamLeader
            {
                get;
            }

            public List<Leader> TeamSubLeader
            {
                get;
            }

            public Leader TeamCoach
            {
                get;
            }

            public List<Entry> Entries
            {
                get;
            }

            public List<Athlete> Athletes
            {
                get;
            }

            internal EntryBlank(CompetitionInfo conf)
            {
                Conf = conf;
                Team = null;
                TeamLeader = new Leader();
                TeamCoach = new Leader();
                TeamSubLeader = new List<Leader>();
                Athletes = new List<Athlete>();
                Entries = new List<Entry>();
            }

            internal EntryBlank(CompetitionInfo conf, String teamId)
            {
                Conf = conf;
                Team = conf.TeamInfos.Find((element) => element.Id == teamId) ?? throw new Exception("没有该ID对应的队伍");
                TeamLeader = new Leader();
                TeamCoach = new Leader();
                TeamSubLeader = new List<Leader>();
                Athletes = new List<Athlete>();
                Entries = new List<Entry>();
            }
        };
    };
}

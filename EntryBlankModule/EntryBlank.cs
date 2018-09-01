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

            public Athlete(String sid, String name, String type, String rank = "")
            {
                Sid = sid;
                Name = name;
                Type = type;
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
        }

        public class Entry
        {
            public String EventName
            {
                get;
            }

            public List<Athlete> Athletes
            {
                get;
            }

            public Leader TeamLeader
            {
                get;
            }

            public List<Leader> TeamSubLeader
            {
                get;
            }

            public Entry(String eventName)
            {
                EventName = eventName;
                Athletes = new List<Athlete>();
                TeamLeader = new Leader();
                TeamSubLeader = new List<Leader>();
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

            public List<Athlete> Athletes
            {
                get;
            }

            public List<Entry> Entries
            {
                get;
            }

            public EntryBlank(CompetitionInfo conf)
            {
                Conf = conf;
                Team = null;
                Athletes = new List<Athlete>();
                Entries = new List<Entry>();
            }

            public EntryBlank(CompetitionInfo conf, String teamId)
            {
                Conf = conf;
                TeamInfo temp = conf.TeamInfos.Find((element) => element.Id == teamId);
                Team = temp ?? throw new Exception("没有该ID对应的队伍");
                Athletes = new List<Athlete>();
                Entries = new List<Entry>();
            }
        };
    };
}

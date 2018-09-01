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

        public class Entry
        {
            public String EventName
            {
                get;
            }

            public List<List<Athlete>> Athletes
            {
                get;
            }

            public Entry(String eventName)
            {
                EventName = eventName;
                Athletes = new List<List<Athlete>>();
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
                
            }
        };
    };
}

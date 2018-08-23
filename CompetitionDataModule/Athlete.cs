using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Athlete : IComparable
        {
            private String name;
            private String id;
            private String code;
            private String sid;
            private AthleteCategory category;
            private AthleteRank rank;

            public String Name
            {
                get { return name; }
                set { name = value; }
            }

            public String Id
            {
                get { return id; }
            }

            public String Sid
            {
                get { return sid; }
                set { sid = value; }
            }

            public String Code
            {
                get { return code; }
                set { code = value; }
            }

            public AthleteCategory Category
            {
                get { return category; }
                set
                {
                    category = value ?? throw new Exception("设置的运动员类别是个无效值");
                }
            }

            public AthleteRank Rank
            {
                get { return rank; }
                set
                {
                    rank = value ?? throw new Exception("设置的运动员级别是个无效值");
                }
            }

            public int CompareTo(object obj)
            {
                Athlete rhs = (Athlete)obj;
                int ret = category.CompareTo(rhs.Category);
                if (ret != 0)
                {
                    return ret;
                }

                ret = rank.CompareTo(rhs.rank);
                return ret != 0 ? ret : name.CompareTo(rhs.Name);
            }
        }

        public class AthletePool : Dictionary<String, Athlete>
        {
            private String prefixCode;

            static AthletePool()
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
            }

            public void TidyUpCodes()
            {
                Dictionary<AthleteCategory, List<Athlete>> athletes = new Dictionary<AthleteCategory, List<Athlete>>();
                foreach (var athlete in Values)
                {
                    if (!athletes.ContainsKey(athlete.Category))
                    {
                        athletes.Add(athlete.Category, new List<Athlete>());
                    }
                    athletes[athlete.Category].Add(athlete);
                }

                Clear();
                foreach (var athleteList in athletes.Values)
                {
                    athleteList.Sort();
                    foreach (var athlete in athleteList)
                    {
                        athlete.Code = NextCode();
                        Add(athlete.Code, athlete);
                    }
                }
            }

            private String NextCode()
            {
                for (UInt32 nextOrder = 1; nextOrder != UInt32.MaxValue; ++nextOrder)
                {
                    String code = String.Format("{0}{1:D2}", prefixCode, nextOrder);
                    foreach (Athlete athlete in Values)
                    {
                        if (athlete.Code != code)
                        {
                            return code;
                        }
                    }
                }
                return null;
            }
        }
    };
};

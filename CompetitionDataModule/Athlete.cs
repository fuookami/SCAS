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
            private AthleteCategory _category;
            private AthleteRank _rank;

            public String Name
            {
                get;
                set;
            }

            public String Id
            {
                get;
                internal set;
            }

            public String Sid
            {
                get;
                set;
            }

            public String Code
            {
                get;
                internal set;
            }

            public AthleteCategory Category
            {
                get { return _category; }
                set
                {
                    _category = value ?? throw new Exception("设置的运动员类别是个无效值");
                }
            }

            public AthleteRank Rank
            {
                get { return _rank; }
                set
                {
                    _rank = value ?? throw new Exception("设置的运动员级别是个无效值");
                }
            }

            public Team BelogingTeam
            {
                get;
            }

            public Dictionary<Event, Dictionary<Game, Grade>> Grades
            {
                get;
            }

            public Dictionary<Event, Point> Points
            {
                get;
            }

            public Athlete(Team team)
                : this(team, Guid.NewGuid().ToString("N"))
            {
            }

            public Athlete(Team team, String existedId, String code = null)
            {
                Id = existedId;
                Code = code;
                BelogingTeam = team;
            }

            public int CompareTo(object obj)
            {
                Athlete rhs = (Athlete)obj;
                int ret = Category.CompareTo(rhs.Category);
                if (ret != 0)
                {
                    return ret;
                }

                ret = Rank.CompareTo(rhs.Rank);
                return ret != 0 ? ret : Name.CompareTo(rhs.Name);
            }
        }

        public class AthletePool : Dictionary<String, Athlete>
        {
            private Team _team;
            private String _prefixCode;

            static AthletePool()
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
            }

            public AthletePool(Team team, String prefixCode)
            {
                this._team = team;
                this._prefixCode = prefixCode;
            }

            public Athlete GenerateNewAthlete(String existedId = null)
            {
                return new Athlete(this._team);
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
                        athlete.Code = NextCode() ?? throw new Exception("运动员的序号已经满额，无法再分配");
                        Add(athlete.Code, athlete);
                    }
                }
            }

            private String NextCode()
            {
                for (UInt32 nextOrder = 1; nextOrder != UInt32.MaxValue; ++nextOrder)
                {
                    String code = String.Format("{0}{1:D2}", _prefixCode, nextOrder);
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

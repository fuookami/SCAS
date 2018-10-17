using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
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

            public String Name
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
                Grades = new Dictionary<Event, Dictionary<Game, Grade>>();
                Points = new Dictionary<Event, Point>();
            }

            public int CompareTo(object obj)
            {
                Athlete rhs = (Athlete)obj;
                if (Code == null || rhs.Code == null)
                {
                    if (Code == null && rhs.Code == null)
                    {
                        return 0;
                    }
                    else if (rhs.Code == null)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                int ret = Code.CompareTo(rhs.Code);
                if (ret != 0)
                {
                    return ret;
                }

                ret = Category.CompareTo(rhs.Category);
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
                var ret = new Athlete(this._team);
                if (existedId == null)
                {
                    existedId = Guid.NewGuid().ToString("N");
                }
                ret.Id = existedId;
                Add(existedId, ret);
                return ret;
            }

            public void TidyUpCodes()
            {
                Dictionary<AthleteCategory, Int32> index = new Dictionary<AthleteCategory, int>();
                List<Tuple<AthleteCategory, List<Athlete>>> athletes = new List<Tuple<AthleteCategory, List<Athlete>>>();

                foreach (var athlete in Values)
                {
                    if (!index.ContainsKey(athlete.Category))
                    {
                        index.Add(athlete.Category, athletes.Count);
                        athletes.Add(new Tuple<AthleteCategory, List<Athlete>>(athlete.Category, new List<Athlete>()));
                    }
                    athletes[index[athlete.Category]].Item2.Add(athlete);
                }

                athletes.Sort((lhs, rhs) => lhs.Item1.CompareTo(rhs.Item1));
                foreach (var athleteList in athletes)
                {
                    athleteList.Item2.Sort();
                    foreach (var athlete in athleteList.Item2)
                    {
                        if (athlete.Code == null)
                        {
                            athlete.Code = NextCode() ?? throw new Exception("运动员的序号已经满额，无法再分配");
                        }
                    }
                }
            }

            private String NextCode()
            {
                for (UInt32 nextOrder = 1; nextOrder != UInt32.MaxValue; ++nextOrder)
                {
                    String code = String.Format("{0}{1:D2}", _prefixCode, nextOrder);
                    if (Values.Count == 0)
                    {
                        return code;
                    }
                    else
                    {
                        if (!Values.Any((ele) => ele.Code == code))
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

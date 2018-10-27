using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS
{
    namespace CompetitionData
    {
        public class GradeBase : IComparable
        {
            public enum Code : Int32
            {
                None = 0,
                Normal = 1,
                MR = 3,
                DSQ = 4,
                DNS = 8
            };

            public Code GradeCode
            {
                get;
                private set;
            }

            public TimeSpan Time
            {
                get;
                private set;
            }

            protected GradeBase()
                : this(Code.None, TimeSpan.Zero)
            {
            }

            protected GradeBase(Code code, TimeSpan time)
            {
                GradeCode = code;
                Time = time;
            }

            public void Set(Code code, TimeSpan time = new TimeSpan())
            {
                GradeCode = code;
                if (HasTime(code))
                {
                    if (time.TotalMilliseconds == 0)
                    {
                        throw new Exception("");
                    }
                    Time = time;
                }
                else
                {
                    Time = new TimeSpan();
                }
            }

            public void Set(Tuple<Code, TimeSpan> value)
            {
                Set(value.Item1, value.Item2);
            }

            public bool Valid()
            {
                return Valid(GradeCode);
            }

            public static bool Valid(Code code)
            {
                return code != Code.None;
            }

            public bool HasTime()
            {
                return Valid() && HasTime(GradeCode);
            }

            public static bool HasTime(Code code)
            {
                return code == Code.Normal || code == Code.MR;
            }

            public String ToFormatString()
            {
                return ToFormatString(GradeCode, Time);
            }

            public static String ToFormatString(Code code, TimeSpan time)
            {
                if (!Valid(code))
                {
                    return "";
                }
                else if (HasTime(code))
                {
                    return String.Format("{0}", FormatTime(time));
                }
                else
                {
                    return code.ToString();
                }
            }

            public static String ToDisplayFormatString(Code code, TimeSpan time)
            {
                if (!Valid(code))
                {
                    return "";
                }
                else if (HasTime(code))
                {
                    return String.Format("{0}{1}", FormatTime(time), code == Code.MR ? "(MR)" : "");
                }
                else
                {
                    return code.ToString();
                }
            }

            public static String FormatTime(TimeSpan time)
            {
                return time.TotalMinutes < 1 ? String.Format("{0:D}.{1:D2}", time.Seconds, time.Milliseconds / 10)
                    : time.TotalHours >= 1 ? String.Format("{0:D}:{1:D}:{2:D2}.{3:D2}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds / 10)
                    : String.Format("{0:D}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, time.Milliseconds / 10);
            }

            public override bool Equals(object obj)
            {
                GradeBase rhs = (GradeBase)obj;
                if (GradeCode != rhs.GradeCode)
                {
                    return false;
                }
                else
                {
                    return Time.Equals(rhs.Time);
                }
            }

            public Int32 CompareTo(Object obj)
            {
                GradeBase rhs = (GradeBase)obj;
                if (!Valid() && !rhs.Valid())
                {
                    return 0;
                }
                else if (Valid() && !rhs.Valid())
                {
                    return -1;
                }
                else if (!Valid() && rhs.Valid())
                {
                    return 1;
                }
                else
                {
                    if (HasTime() && rhs.HasTime())
                    {
                        return Time.CompareTo(rhs.Time);
                    }
                    else if (HasTime() && !rhs.HasTime())
                    {
                        return -1;
                    }
                    else if (!HasTime() && rhs.HasTime())
                    {
                        return 1;
                    }
                    else
                    {
                        if (GradeCode == rhs.GradeCode)
                        {
                            return 0;
                        }
                        else if (GradeCode == Code.DSQ)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
        }

        public class RecordGrade : GradeBase
        {
            public Tuple<String, String, UInt32> Competition
            {
                get;
            }

            public Tuple<String, String> Event
            {
                get;
            }

            public Tuple<String, String> Game
            {
                get;
            }

            public Tuple<String, UInt32, UInt32> Participant
            {
                get;
            }

            public List<Tuple<String, String, String>> Athletes
            {
                get;
            }

            public RecordGrade(Tuple<String, String, UInt32> competitionIdNameOrderPair, Tuple<String, String> eventIdNamePair, Tuple<String, String> gameIdNamePair, Tuple<String, UInt32, UInt32> participant, Tuple<String, String, String> athlete, Code code, TimeSpan time)
                : base(code, time)
            {
                Competition = competitionIdNameOrderPair;
                Event = eventIdNamePair;
                Game = gameIdNamePair;
                Participant = participant;
                Athletes = new List<Tuple<String, String, String>>
                {
                    athlete
                };
            }

            public RecordGrade(Tuple<String, String, UInt32> competitionIdNameOrderPair, Tuple<String, String> eventIdNamePair, Tuple<String, String> gameIdNamePair, Tuple<String, UInt32, UInt32> participant, List<Tuple<String, String, String>> athletes, Code code, TimeSpan time)
                : base(code, time)
            {
                Competition = competitionIdNameOrderPair;
                Event = eventIdNamePair;
                Game = gameIdNamePair;
                Participant = participant;
                Athletes = athletes;
            }
        };

        public class Grade : GradeBase
        {
            public static readonly Grade NoGrade = new Grade(null);

            public Participant GradeParticipator
            {
                get;
            }

            public Grade(Participant participant, Code code = Code.None, TimeSpan time = new TimeSpan())
                : base(code, time)
            {
                GradeParticipator = participant;
            }
        };
    };
};

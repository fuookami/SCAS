﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS
{
    namespace CompetitionData
    {
        public class GradeBase
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
                return GradeCode != Code.None;
            }

            static public bool HasTime(Code code)
            {
                return code == Code.Normal || code == Code.MR;
            }

            public String ToFormatString()
            {
                if (!Valid())
                {
                    return "";
                }
                else if (HasTime(GradeCode))
                {
                    return String.Format("{0}{1}", FormatTime(Time), GradeCode == Code.MR ? "(MR)" : "");
                }
                else
                {
                    return GradeCode.ToString();
                }
            }

            private static String FormatTime(TimeSpan time)
            {
                return time.TotalMinutes < 1 ? String.Format("{0:D2}.{1:D2}", time.Seconds, time.Milliseconds / 100)
                    : time.TotalHours >= 1 ? String.Format("{0:D}:{1:D}:{2:D2}.{3:D2}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds / 100)
                    : String.Format("{0:D}:{1:D2}.{2:D2}", time.Minutes, time.Seconds, time.Milliseconds / 100);
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

using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Grade
        {
            public enum Code
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

            public Participant GradeParticipator
            {
                get;
            }

            public Grade(Participant participator)
            {
                GradeCode = Code.None;
                Time = new TimeSpan();
                GradeParticipator = participator;
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

            static private bool HasTime(Code code)
            {
                return code == Code.Normal || code == Code.MR;
            }
        }
    };
};

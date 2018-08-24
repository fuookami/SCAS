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

            private Code code;
            private TimeSpan time;

            private Participator participator;

            public Code GradeCode
            {
                get { return code; }
            }

            public TimeSpan Time
            {
                get { return time; }
            }

            public Participator GradeParticipator
            {
                get { return participator; }
            }

            public Grade(Participator participator)
            {
                code = Code.None;
                time = new TimeSpan();
                this.participator = participator;
            }

            public void Set(Code code, TimeSpan time = new TimeSpan())
            {
                this.code = code;
                if (HasTime(code))
                {
                    if (time.TotalMilliseconds == 0)
                    {
                        throw new Exception("");
                    }
                    this.time = time;
                }
            }

            static private bool HasTime(Code code)
            {
                return code == Code.Normal || code == Code.MR;
            }
        }
    };
};

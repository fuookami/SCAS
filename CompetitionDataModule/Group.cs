using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Line
        {
            public SSUtils.Order Order
            {
                get;
            }

            public Participant LineParticipant
            {
                get;
            }

            public Grade ParticipatorGrade
            {
                get;
                set;
            }

            public Line(UInt32 order, Participant participant)
            {
                Order = new SSUtils.Order((Int32)order);
                LineParticipant = participant;
                ParticipatorGrade = new Grade(participant);
            }
        }

        public class Group
        {
            public List<Line> Lines
            {
                get;
            }

            public Group()
            {
                Lines = new List<Line>();
            }
        }
    };
};

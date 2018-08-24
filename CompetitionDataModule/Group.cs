using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionData
    {
        public struct Line
        {
            public UInt32 Order;
            public Participator LineParticipator;
            public Grade ParticipatorGrade;

            public Line(UInt32 order, Participator participator)
            {
                Order = order;
                LineParticipator = participator;
                ParticipatorGrade = new Grade(participator);
            }
        }

        public class Group
        {
            List<Line> lines;
        }
    };
};

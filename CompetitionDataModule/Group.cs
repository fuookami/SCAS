using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionData
    {
        public struct Line
        {
            public UInt32 Order;
            public Athlete LineAthlete;
            public Grade AthleteGrade;

            public Line(UInt32 order, Athlete athlete)
            {
                Order = order;
                LineAthlete = athlete;
                AthleteGrade = new Grade();
            }
        }

        public class Group
        {
            List<Line> lines;
        }
    };
};

using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Point
        {
            private Team team;
            private Participant participant;
        }

        public class PointPool
        {
            private PointInfo conf;
            private List<Point> points;
        }
    };
};

using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Team
        {
            TeamInfo conf;

            AthletePool athletes;
            Dictionary<Event, List<Point>> points;
        }
    };
};

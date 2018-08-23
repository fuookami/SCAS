using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Competition
        {
            CompetitionInfo conf;

            List<Event> events;
            Dictionary<Session, Game> games;
        }
    };
};

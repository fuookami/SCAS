using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class RegulationInfo
        {
            public List<String> Organizers
            {
                get;
            }

            public List<String> Undertakers
            {
                get;
            }

            public List<String> Coorganizers
            {
                get;
            }

            public List<Tuple<String, String>> Plans
            {
                get;
            }

            public List<String> Contestants
            {
                get;
            }

            public RegulationInfo()
            {
                Organizers = new List<String>();
                Undertakers = new List<String>();
                Coorganizers = new List<String>();
                Plans = new List<Tuple<String, String>>();
                Contestants = new List<String>();
            }
        };
    }
}

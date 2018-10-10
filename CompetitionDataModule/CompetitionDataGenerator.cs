using System;
using System.Collections.Generic;
using System.Linq;
using SCAS.CompetitionConfiguration;
using SCAS.EntryBlank;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Generator : ErrorStorer
        {
            private CompetitionInfo _conf;

            public CompetitionInfo Conf
            {
                get
                {
                    return _conf;
                }
            }

            public List<Blank> EntryBlanks
            {
                get;
            }

            public Competition Result
            {
                get;
                private set;
            }

            public Generator(CompetitionInfo conf)
            {
                _conf = conf;
                EntryBlanks = new List<Blank>();
                Result = null;
            }

            public bool Generate()
            {
                return true;
            }
        }
    };
};

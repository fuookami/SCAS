using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Game
        {
            public GameInfo Conf
            {
                get;
                private set;
            }

            public List<Group> Groups
            {
                get;
                internal set;
            }

            public Event Parent
            {
                get;
            }

            public Game(Event parent, GameInfo conf)
            {
                Parent = parent;
                Conf = conf;
                Groups = new List<Group>();
            }
        }
    };
};

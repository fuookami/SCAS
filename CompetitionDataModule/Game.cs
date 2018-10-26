using System;
using System.Collections.Generic;
using System.Linq;
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

            public List<Tuple<SSUtils.Order, List<Line>>> OrderInGame
            {
                get
                {
                    List<Line> lineWithGrade = new List<Line>();
                    foreach (var group in Groups)
                    {
                        lineWithGrade.AddRange(group.Lines.FindAll((line) => line.ParticipantGrade.Valid()));
                    }
                    return Line.GroupByGrade(lineWithGrade);
                }
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

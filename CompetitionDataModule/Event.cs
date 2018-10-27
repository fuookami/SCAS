using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Event
        {
            public EventInfo Conf
            {
                get;
                private set;
            }

            public RecordGrade MatchRecord
            {
                get;
                set;
            }

            public List<Game> Games
            {
                get;
                internal set;
            }

            public Game LastGame
            {
                get
                {
                    return Games[Games.Count - 1];
                }
            }

            public PointPool Points
            {
                get;
            }

            public Competition Parent
            {
                get;
            }

            public Event(Competition parent, EventInfo conf, RecordGrade matchRecord = null)
            {
                Parent = parent;
                Conf = conf;
                MatchRecord = matchRecord;
                Games = new List<Game>();
                Points = new PointPool(conf.EventPointInfo, this);
            }

            public Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> GetOrderInEvent()
            {
                return Conf.EventTeamworkInfo.BeTeamwork ? LastGame.GetOrderInGameOfTeamworkGame()
                    : LastGame.GetOrderInGameOfPersonalGame();
            }
        }
    };
};

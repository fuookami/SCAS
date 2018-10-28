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

            public List<Tuple<String, List<Tuple<SSUtils.Order, List<Line>>>>> GetOrderInEvent()
            {
                var temp = Conf.EventTeamworkInfo.BeTeamwork ? LastGame.GetOrderInGameOfTeamworkGame()
                    : LastGame.GetOrderInGameOfPersonalGame();
                var ret = new List<Tuple<String, List<Tuple<SSUtils.Order, List<Line>>>>>();
                foreach (var pair in temp)
                {
                    ret.Add(new Tuple<String, List<Tuple<SSUtils.Order, List<Line>>>>(pair.Key, pair.Value));
                }
                return ret;
            }

            public void RefreshPoints()
            {
                foreach (var team in Parent.Teams)
                {
                    foreach (var athlete in team.Athletes)
                    {
                        athlete.Value.Points.Remove(this);
                    }
                    team.Points.Remove(this);
                }

                var lists = GetOrderInEvent();
                foreach (var list in lists)
                {
                    list.Item2.RemoveAll((ele) => !ele.Item1.Valid());

                    var j = Conf.EventPointInfo.Points.Count;
                    for (Int32 i = 0; i != j; ++i)
                    {
                        var thisOrder = list.Item2[i];
                        var lines = thisOrder.Item2;
                        if (thisOrder.Item1.Value > Conf.EventPointInfo.Points.Count)
                        {
                            break;
                        }
                        foreach (var line in lines)
                        {
                            var newPoint = new Point(line.LineParticipant, Conf.EventPointInfo, thisOrder.Item1, line.ParticipantGrade.GradeCode == GradeBase.Code.MR);

                            foreach (var athlete in line.LineParticipant.Athletes)
                            {
                                athlete.Points.Add(this, newPoint);
                            }
                            if (!line.LineParticipant.ParticipantTeam.Points.ContainsKey(this))
                            {
                                line.LineParticipant.ParticipantTeam.Points.Add(this, new List<Point>());
                            }
                            line.LineParticipant.ParticipantTeam.Points[this].Add(newPoint);
                            Points.Add(newPoint);
                        }
                    }
                }
            }
        }
    };
};

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
                    var j = Conf.EventPointInfo.Points.Count;
                    if (j < list.Value.Count)
                    {
                        j = list.Value.Count;
                    }
                    for (Int32 i = 0; i != j; ++i)
                    {
                        var thisOrder = list.Value[i];
                        if (!thisOrder.Item1.Valid())
                        {
                            break;
                        }
                        var lines = thisOrder.Item2;
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

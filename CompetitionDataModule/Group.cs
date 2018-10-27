using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Line
        {
            public SSUtils.Order Order
            {
                get;
            }

            public Participant LineParticipant
            {
                get;
            }

            public Grade ParticipantGrade
            {
                get
                {
                    return LineParticipant != null 
                        ? LineParticipant.ParticipantGrade
                        : Grade.NoGrade;
                }
            }
            
            public Group Parent
            {
                get;
            }

            public static List<Tuple<SSUtils.Order, List<Line>>> GroupByGrade(List<Line> lines)
            {
                if (lines == null || lines.Count == 0)
                {
                    return new List<Tuple<SSUtils.Order, List<Line>>>();
                }
                else
                {
                    lines.Sort((lhs, rhs) => lhs.ParticipantGrade.CompareTo(rhs.ParticipantGrade));
                    List<Tuple<SSUtils.Order, List<Line>>> ret = new List<Tuple<SSUtils.Order, List<Line>>>();
                    if (lines[0].ParticipantGrade.HasTime())
                    {
                        ret.Add(new Tuple<SSUtils.Order, List<Line>>(new SSUtils.Order(1), new List<Line> { lines[0] }));
                    }
                    else
                    {
                        ret.Add(new Tuple<SSUtils.Order, List<Line>>(new SSUtils.Order(SSUtils.Order.NotSet), new List<Line> { lines[0] }));
                    }

                    for (Int32 index = 0, i = 1, j = lines.Count; i != j; ++i)
                    {
                        var line = lines[i];
                        if (line.ParticipantGrade.Equals(ret[index].Item2[ret[index].Item2.Count - 1].ParticipantGrade))
                        {
                            ret[index].Item2.Add(line);
                        }
                        else
                        {
                            if (line.ParticipantGrade.HasTime())
                            {
                                ++index;
                                ret.Add(new Tuple<SSUtils.Order, List<Line>>(new SSUtils.Order(index + 1), new List<Line> { line }));
                            }
                            else
                            {
                                ++index;
                                ret.Add(new Tuple<SSUtils.Order, List<Line>>(new SSUtils.Order(SSUtils.Order.NotSet), new List<Line> { line }));
                            }
                        }
                    }
                    return ret;
                }
            }

            public Line(Group parent, UInt32 order, Participant participant)
            {
                Parent = parent;
                Order = new SSUtils.Order((Int32)order);
                LineParticipant = participant;
                if (participant != null)
                {
                    participant.Parent = this;
                }
            }

            public bool IsNewMatchGrade()
            {
                return ParticipantGrade.HasTime() ? IsNewMatchGrade(ParticipantGrade.Time) : false;
            }

            public bool IsNewMatchGrade(TimeSpan time)
            {
                var eventData = Parent.Parent.Parent;
                if (eventData.MatchRecord != null)
                {
                    if (eventData.Conf.EventGradeInfo.GradeBetterType == CompetitionConfiguration.GradeInfo.BetterType.Smaller)
                    {
                        return time < eventData.MatchRecord.Time;
                    }
                    else if (eventData.Conf.EventGradeInfo.GradeBetterType == CompetitionConfiguration.GradeInfo.BetterType.Bigger)
                    {
                        return time > eventData.MatchRecord.Time;
                    }
                }
                return false;
            }
        }

        public class Group
        {
            public List<Line> Lines
            {
                get;
            }

            public Game Parent
            {
                get;
            }

            public List<Tuple<SSUtils.Order, List<Line>>> OrderInGroup
            {
                get
                {
                    return Line.GroupByGrade(Lines.FindAll((ele) => ele.ParticipantGrade.Valid()));
                }
            }

            public Group(Game parent)
            {
                Parent = parent;
                Lines = new List<Line>();
            }

            public Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> GetOrderInGroupOfPersonalGame()
            {
                var ranks = new Dictionary<String, List<Line>>();
                foreach (var line in Lines)
                {
                    if (line.LineParticipant != null)
                    {
                        var rank = line.LineParticipant.Athletes[0].Rank;
                        var key = rank == null ? "" : rank.Name;
                        if (ranks.ContainsKey(key))
                        {
                            ranks.Add(key, new List<Line>());
                        }
                        ranks[key].Add(line);
                    }
                }

                Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> ret = new Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>>();
                foreach (var rankPair in ranks)
                {
                    ret.Add(rankPair.Key, Line.GroupByGrade(rankPair.Value));
                }
                return ret;
            }

            public Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> GetOrderInGroupOfTeamworkGame()
            {
                var ranks = new Dictionary<String, List<Line>>();
                foreach (var line in Lines)
                {
                    if (line.LineParticipant != null)
                    {
                        var rank = line.LineParticipant.ParticipantTeam.Conf.Category;
                        var key = rank == null ? "" : rank.Name;
                        if (ranks.ContainsKey(key))
                        {
                            ranks.Add(key, new List<Line>());
                        }
                        ranks[key].Add(line);
                    }
                }

                Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> ret = new Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>>();
                foreach (var rankPair in ranks)
                {
                    ret.Add(rankPair.Key, Line.GroupByGrade(rankPair.Value));
                }
                return ret;
            }
        }
    };
};

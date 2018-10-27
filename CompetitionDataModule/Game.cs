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

            public Game(Event parent, GameInfo conf)
            {
                Parent = parent;
                Conf = conf;
                Groups = new List<Group>();
            }

            public Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> GetOrderInGameOfPersonalGame()
            {
                var ranks = new Dictionary<String, List<Line>>();
                foreach (var group in Groups)
                {
                    foreach (var line in group.Lines)
                    {
                        if (line.LineParticipant != null)
                        {
                            var rank = line.LineParticipant.Athletes[0].Rank;
                            var key = rank == null ? "" : rank.Name;
                            if (!ranks.ContainsKey(key))
                            {
                                ranks.Add(key, new List<Line>());
                            }
                            if (!line.ParticipantGrade.Valid())
                            {
                                line.ParticipantGrade.Set(GradeBase.Code.DNS);
                            }
                            ranks[key].Add(line);
                        }
                    }
                }

                Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> ret = new Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>>();
                foreach (var rankPair in ranks)
                {
                    ret.Add(rankPair.Key, Line.GroupByGrade(rankPair.Value));
                }
                return ret;
            }

            public Dictionary<String, List<Tuple<SSUtils.Order, List<Line>>>> GetOrderInGameOfTeamworkGame()
            {
                var ranks = new Dictionary<String, List<Line>>();
                foreach (var group in Groups)
                {
                    foreach (var line in group.Lines)
                    {
                        if (line.LineParticipant != null)
                        {
                            var rank = line.LineParticipant.ParticipantTeam.Conf.Category;
                            var key = rank == null ? "" : rank.Name;
                            if (!ranks.ContainsKey(key))
                            {
                                ranks.Add(key, new List<Line>());
                            }
                            if (!line.ParticipantGrade.Valid())
                            {
                                line.ParticipantGrade.Set(GradeBase.Code.DNS);
                            }
                            ranks[key].Add(line);
                        }
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

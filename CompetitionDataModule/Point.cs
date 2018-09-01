using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Point
        {
            public Team PointTeam
            {
                get;
            }

            public Participant PointParticipant
            {
                get;
            }

            public SSUtils.Order Ranking
            {
                get;
            }

            public UInt32 PointValue
            {
                get;
            }

            public bool BreakRecord
            {
                get;
            }

            internal Point(Participant participant, PointInfo conf, SSUtils.Order ranking, bool breakRecord = false)
            {
                if (ranking.Valid())
                {
                    throw new Exception("传入的名次值是个无效值");
                }
                else if (conf.Points.Count <= ranking.Value)
                {
                    Ranking = new SSUtils.Order();
                    PointValue = 0;
                }
                else
                {
                    Ranking = ranking;
                    PointValue = (UInt32)(conf.Points[(Int32)ranking.Value] * conf.PointRate);
                    if (conf.BreakRecordPointRateEnabled && breakRecord)
                    {
                        PointValue = (UInt32)(PointValue * conf.BreakRecordPointRate);
                    }
                    BreakRecord = breakRecord;
                }

                PointTeam = participant.ParticipantTeam;
                PointParticipant = participant;
            }
        }

        public class PointPool : List<Point>
        {
            private Map<Participant, SSUtils.Order> _orders;

            public PointInfo Conf
            {
                get;
            }

            public Event PointEvent
            {
                get;
            }

            public PointPool(PointInfo conf, Event pointEvent)
            {
                _orders = new Map<Participant, SSUtils.Order>();
                Conf = conf;
                PointEvent = pointEvent;
            }

            public Point GenerateNewPoint(Participant participant, SSUtils.Order ranking, bool breakRecord)
            {
                if (_orders.Left.ContainsKey(participant))
                {
                    return null;
                }
                if (_orders.Right.ContainsKey(ranking))
                {
                    return null;
                }

                _orders.Add(participant, ranking);
                Point point = new Point(participant, Conf, ranking, breakRecord);
                Add(point);
                return point;
            }

            public new void Remove(Point point)
            {
                Participant participant = point.PointParticipant;
                base.Remove(point);
                _orders.Remove(participant);
            }

            public void Remove(Participant participant)
            {
                base.Remove(Find((element) => element.PointParticipant == participant));
                _orders.Remove(participant);
            }

            public void Remove(SSUtils.Order ranking)
            {
                if (ranking.Valid())
                {
                    base.Remove(Find((element) => element.Ranking.Equals(ranking)));
                    _orders.Remove(ranking);
                }
            }

            public new void Sort()
            {
                Sort((lhs, rhs) => lhs.Ranking.CompareTo(rhs.Ranking));
            }
        }
    };
};

using System;
using System.Collections.Generic;
using System.Linq;

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
                lines.Sort((lhs, rhs) => lhs.ParticipantGrade.CompareTo(rhs.ParticipantGrade));
                var groups = lines.GroupBy((ele) => ele.ParticipantGrade)
                    .Select((group) => group.ToList()).ToList();
                
                List<Tuple<SSUtils.Order, List<Line>>> ret = new List<Tuple<SSUtils.Order, List<Line>>>();
                Int32 index = 1;
                foreach (var group in groups)
                {
                    if (group[0].ParticipantGrade.HasTime())
                    {
                        ret.Add(new Tuple<SSUtils.Order, List<Line>>(new SSUtils.Order(index), group));
                        ++index;
                    }
                    else
                    {
                        ret.Add(new Tuple<SSUtils.Order, List<Line>>(new SSUtils.Order(), group));
                    }
                }
                return ret;
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
        }
    };
};

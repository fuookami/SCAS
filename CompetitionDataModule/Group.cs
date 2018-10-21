using System;
using System.Collections.Generic;

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
                    return LineParticipant.ParticipantGrade;
                }
            }
            
            public Group Parent
            {
                get;
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

            public Group(Game parent)
            {
                Parent = parent;
                Lines = new List<Line>();
            }
        }
    };
};

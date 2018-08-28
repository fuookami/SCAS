using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class AthleteRank : IComparable
        {
            public String Id
            {
                get;
                internal set;
            }

            public SSUtils.Order Order
            {
                get;
                internal set;
            }

            public String Name
            {
                get;
                set;
            }

            public AthleteRank()
                : this(Guid.NewGuid().ToString("N"), new SSUtils.Order())
            {
            }

            public AthleteRank(String existedId, UInt32 distributiveOrder)
                : this(existedId, new SSUtils.Order((Int32)distributiveOrder))
            {
            }

            public AthleteRank(String existedId, SSUtils.Order distributiveOrder = new SSUtils.Order())
            {
                Id = existedId;
                Order = distributiveOrder;
            }

            public int CompareTo(object obj)
            {
                AthleteRank rhs = (AthleteRank)obj;
                return Order.CompareTo(rhs.Order);
            }
        }

        public class AthleteRankList : List<AthleteRank>
        {
            new public void Sort()
            {
                Sort((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));
            }
        }

        public class AthleteRankPool : AthleteRankList
        {
            public AthleteRank GenerateNewRank(String existedId = null)
            {
                UInt32 order = 0;
                for (; order != UInt32.MaxValue; ++order)
                {
                    if (this.Find((ele) => ele.Order.Equals(order)) == null)
                    {
                        break;
                    }
                }
                if (order == UInt32.MaxValue)
                {
                    throw new Exception("运动员级别的序号已经满额，无法再分配");
                }
                var element = new AthleteRank(existedId ?? Guid.NewGuid().ToString("N"), order);
                this.Add(element);
                return element;
            }
        }
    };
};

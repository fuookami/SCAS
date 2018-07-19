using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    class AthleteRank
    {
        private String id;
        private UInt32 order;
        private String name;

        public String Id
        {
            get { return id; }
        }

        public UInt32 Order
        {
            get { return order; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public AthleteRank(UInt32 distributiveOrder = 0)
            : this(Guid.NewGuid().ToString("N"), distributiveOrder) { }

        public AthleteRank(String existedId, UInt32 distributiveOrder = 0)
        {
            id = existedId;
            order = distributiveOrder;
        }
    }

    class AthleteRankList : List<AthleteRank>
    {
        new public void Sort()
        {
            Sort((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));
        }
    }

    class AthleteRankPool : AthleteRankList
    {
        public AthleteRank GenerateNewRank()
        {
            UInt32 order = 0;
            for (; order != UInt32.MaxValue; ++order)
            {
                if (this.Find((ele) => ele.Order == order) == null)
                {
                    break;
                }
            }
            if (order == UInt32.MaxValue)
            {
                throw new Exception("运动员级别的序号已经满额，无法再分配");
            }
            var element = new AthleteRank(order);
            this.Add(element);
            return element;
        }
    }
}

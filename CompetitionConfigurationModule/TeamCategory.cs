using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class TeamCategory : IComparable
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

            public TeamCategory(UInt32 distributiveOrder)
                : this(new SSUtils.Order((Int32)distributiveOrder))
            {
            }

            public TeamCategory(SSUtils.Order distributiveOrder = new SSUtils.Order())
                : this(Guid.NewGuid().ToString("N"), distributiveOrder)
            {
            }

            public TeamCategory(String existedId, UInt32 distributiveOrder)
                : this(existedId, new SSUtils.Order((Int32)distributiveOrder))
            {
            }

            public TeamCategory(String existedId, SSUtils.Order distributiveOrder = new SSUtils.Order())
            {
                Id = existedId;
                Order = distributiveOrder;
            }

            public int CompareTo(object obj)
            {
                var rhs = (TeamCategory)obj;
                return Order.CompareTo(rhs.Order);
            }
        }

        public class TeamCategoryList : List<TeamCategory>
        {
            new public void Sort()
            {
                Sort((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));
            }
        }

        public class TeamCategoryPool : TeamCategoryList
        {
            public TeamCategory GenerateNewCategory(String existedId = null)
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
                    throw new Exception("队伍类别的序号已经满额，无法再分配");
                }
                var element = new TeamCategory(existedId ?? Guid.NewGuid().ToString("N"), order);
                Add(element);
                return element;
            }
        };
    };
};

using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class TeamCategory : IComparable
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

            public TeamCategory(UInt32 distributiveOrder = 0)
                : this(Guid.NewGuid().ToString("N"), distributiveOrder) { }

            public TeamCategory(String existedId, UInt32 distributiveOrder = 0)
            {
                id = existedId;
                order = distributiveOrder;
            }

            public int CompareTo(object obj)
            {
                return (Int32)order - (Int32)((TeamCategory)obj).order;
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
                    if (this.Find((ele) => ele.Order == order) == null)
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

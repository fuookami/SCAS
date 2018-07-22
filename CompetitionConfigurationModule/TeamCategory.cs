using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    class TeamCategory
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
    }

    class TeamCategoryList : List<TeamCategory>
    {
        new public void Sort()
        {
            Sort((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));
        }
    }

    class TeamCategoryPool : TeamCategoryList
    {
        public TeamCategory GenerateNewCategory()
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
            var element = new TeamCategory(order);
            Add(element);
            return element;
        }
    }
}

using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class AthleteCategory : IComparable
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

            public AthleteCategory(UInt32 distributiveOrder = 0)
                : this(Guid.NewGuid().ToString("N"), distributiveOrder) { }

            public AthleteCategory(String existedId, UInt32 distributiveOrder = 0)
            {
                id = existedId;
                order = distributiveOrder;
            }

            public int CompareTo(object obj)
            {
                return (Int32)order - (Int32)((AthleteCategory)obj).order;
            }
        }

        public class AthleteCategoryList : List<AthleteCategory>
        {
            new public void Sort()
            {
                Sort((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));
            }
        }

        public class AthleteCategoryPool : AthleteCategoryList
        {
            public bool Check()
            {
                return CheckOrderIsNotSame() && CheckNameIsNotSame();
            }

            public bool CheckOrderIsNotSame()
            {
                HashSet<UInt32> orders = new HashSet<UInt32>();

                foreach (var data in this)
                {
                    if (orders.Contains(data.Order))
                    {
                        return false;
                    }
                    orders.Add(data.Order);
                }

                return true;
            }

            public bool CheckNameIsNotSame()
            {
                HashSet<String> names = new HashSet<String>();

                foreach (var data in this)
                {
                    if (names.Contains(data.Name))
                    {
                        return false;
                    }
                    names.Add(data.Name);
                }

                return true;
            }

            public AthleteCategory GenerateNewCategory(String existedId = null)
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
                    throw new Exception("运动员类别的序号已经满额，无法再分配");
                }
                var element = new AthleteCategory(existedId ?? Guid.NewGuid().ToString("N"), order);
                this.Add(element);
                return element;
            }
        };
    };
};

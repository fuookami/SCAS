using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class AthleteCategory : IComparable
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

            public String SidKey
            {
                get;
                set;
            }

            public AthleteCategory()
                : this(Guid.NewGuid().ToString("N"), new SSUtils.Order())
            {
            }

            public AthleteCategory(String existedId, UInt32 distributiveOrder)
                : this(existedId, new SSUtils.Order((Int32)distributiveOrder))
            {
            }

            public AthleteCategory(String existedId, SSUtils.Order distributiveOrder = new SSUtils.Order())
            {
                Id = existedId;
                Order = distributiveOrder;
            }

            public int CompareTo(object obj)
            {
                AthleteCategory rhs = (AthleteCategory)obj;
                return Order.CompareTo(rhs.Order);
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
                HashSet<SSUtils.Order> orders = new HashSet<SSUtils.Order>();

                foreach (var data in this)
                {
                    if (orders.Contains(data.Order))
                    {
                        return false;
                    }
                    if (data.Order.Valid())
                    {
                        orders.Add(data.Order);
                    }
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
                    if (this.Find((ele) => ele.Order.Equals(order)) == null)
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

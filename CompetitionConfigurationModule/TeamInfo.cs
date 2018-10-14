using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class TeamInfo
        {
            private TeamCategory _category;

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

            public String ShortName
            {
                get;
                set;
            }

            public String Name
            {
                get;
                set;
            }

            public TeamCategory Category
            {
                get
                {
                    return _category;
                }
                set
                {
                    _category = value ?? throw new Exception("设置的队伍类型是个无效值");
                }
            }

            public TeamInfo(TeamCategory teamCategory, UInt32 distributiveOrder)
                : this(teamCategory, new SSUtils.Order((Int32)(distributiveOrder)))
            {
            }

            public TeamInfo(TeamCategory teamCategory, SSUtils.Order distributiveOrder = new SSUtils.Order())
                : this(teamCategory, Guid.NewGuid().ToString("N"), distributiveOrder)
            {
            }

            public TeamInfo(TeamCategory teamCategory, String existedId, SSUtils.Order distributiveOrder = new SSUtils.Order())
            {
                Id = existedId;
                Order = distributiveOrder;
                _category = teamCategory;
            }
        }

        public class TeamInfoList : List<TeamInfo>
        {
            new public void Sort()
            {
                Sort((lhs, rhs) => lhs.Order.CompareTo(rhs.Order));
            }

            public bool CheckCategoryIsSame()
            {
                if (this.Count == 0)
                {
                    return true;
                }

                TeamCategory category = this[0].Category;
                for (Int32 i = 1, j = this.Count; i != j; ++i)
                {
                    if (this[i].Category != category)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public class TeamInfoPool : TeamInfoList
        {
            public TeamInfo GenerateNewInfo(TeamCategory teamCategory, Int32 existedOrder = -1, String existedId = null)
            {
                UInt32 order = 0;
                if (Find((ele) => ele.Order.Equals(existedOrder)) == null)
                {
                    order = (UInt32)existedOrder;
                }
                else
                {
                    for (; order != UInt32.MaxValue; ++order)
                    {
                        if (Find((ele) => ele.Order.Equals(order)) == null)
                        {
                            break;
                        }
                    }
                    if (order == UInt32.MaxValue)
                    {
                        throw new Exception("队伍信息的序号已经满额，无法再分配");
                    }
                }
                var element = new TeamInfo(teamCategory, existedId ?? Guid.NewGuid().ToString("N"), new SSUtils.Order((Int32)order));
                Add(element);
                return element;
            }
        };
    };
};

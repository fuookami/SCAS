﻿using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class TeamInfo
    {
        private String id;
        private UInt32 order;
        private String shortName;
        private String name;
        private TeamCategory category;

        public String Id
        {
            get { return id; }
        }

        public UInt32 Order
        {
            get { return order; }
        }

        public String ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public TeamCategory Category
        {
            get { return category; }
            set { category = value ?? throw new Exception("设置的队伍类型是个无效值"); }
        }

        public TeamInfo(TeamCategory teamCategory, UInt32 distributiveOrder = 0)
            : this(teamCategory, Guid.NewGuid().ToString("N"), distributiveOrder) { }

        public TeamInfo(TeamCategory teamCategory, String existedId, UInt32 distributiveOrder = 0)
        {
            id = existedId;
            order = distributiveOrder;
            category = teamCategory;
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
        public TeamInfo GenerateNewInfo(TeamCategory teamCategory, String existedId = null)
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
                throw new Exception("队伍信息的序号已经满额，无法再分配");
            }
            var element = new TeamInfo(teamCategory, existedId ?? Guid.NewGuid().ToString("N"), order);
            Add(element);
            return element;
        }
    }
}
using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    class GameInfo
    {
        public const UInt32 NoLimit = 0;

        private String id;
        private String name;

        private UInt32 numberOfTeam;
        private Date date;

        private Int32 orderInEvent;
        private Int32 orderInDate;

        private TimeSpan planIntervalTime;
        private TimeSpan planTimePerGroup;

        private GroupInfo groupInfo;

        private EventInfo eventInfo;

        public String Id
        {
            get { return id; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public UInt32 NumberOfTeam
        {
            get { return NumberOfTeam; }
            set { numberOfTeam = value; }
        }

        public Date GameDate
        {
            get { return date; }
            set { date = value; }
        }

        public Int32 OrderInDate
        {
            get { return orderInDate; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("不能把比赛在当天的所有比赛中的序号设置为负数");
                }
                orderInDate = value;
            }
        }

        public Int32 OrderInEvent
        {
            get { return orderInEvent; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("不能把比赛在项目中的序号设置为负数");
                }
                orderInEvent = value;
            }
        }

        public TimeSpan PlanIntervalTime
        {
            get { return planIntervalTime; }
            set
            {
                if (value.Seconds <= 0)
                {
                    throw new Exception("不能将比赛间计划间隔时间设置为负数");
                }
                planIntervalTime = value;
            }
        }

        public TimeSpan PlanTimePerGroup
        {
            get { return planTimePerGroup; }
            set
            {
                if (value.Seconds <= 0)
                {
                    throw new Exception("不能将每组比赛计划时间设置为负数");
                }
                planTimePerGroup = value;
            }
        }

        public GroupInfo EventGroupInfo
        {
            get { return groupInfo; }
            set { groupInfo = value ?? throw new Exception("设置的比赛分组信息是个无效值"); }
        }

        public EventInfo Event
        {
            get { return eventInfo; }
        }

        protected GameInfo(EventInfo _event)
            : this(_event, Guid.NewGuid().ToString("N")) { }

        protected GameInfo(EventInfo _event, String existedId)
        {
            groupInfo = new GroupInfo();
            eventInfo = _event;
        }
    }

    class DuelGameInfo : GameInfo
    {
        public enum DuelGameType
        {
            Group,              // 小组赛
            QuarterFinal,       // 复赛
            SemiFinal,          // 半决赛
            ThridPlaceMatch,    // 三四名决赛（仅在对抗性运动中出现）
            Fianls				// 决赛
        };

        public enum DuelGamePattern
        {
            Elimination,        // 淘汰制 / 竞标赛制
            Circulation			// 循环制（仅在对抗性运动中出现）
        };

        private DuelGameType type;
        private DuelGamePattern pattern;

        public DuelGameType Type
        {
            get { return type; }
            set { type = value; }
        }

        public DuelGamePattern Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        public DuelGameInfo(EventInfo _event)
            : base(_event) { }

        public DuelGameInfo(EventInfo _event, String existedId)
            : base(_event, existedId) { }
    }

    class RankingGameInfo : GameInfo
    {
        public enum RankingGameType
        {
            Group,              // 小组赛
            QuarterFinal,       // 复赛
            SemiFinal,          // 半决赛
            Fianls				// 决赛
        };

        public enum RankingGamePattern
        {
            Elimination,        // 淘汰制 / 竞标赛制
            Ranking,			// 名次制（仅在非对抗性运动中出现）
        };

        private RankingGameType type;
        private RankingGamePattern pattern;

        public RankingGameType Type
        {
            get { return type; }
            set { type = value; }
        }

        public RankingGamePattern Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        public RankingGameInfo(EventInfo _event)
            : base(_event) { }

        public RankingGameInfo(EventInfo _event, String existedId)
            : base(_event, existedId) { }
    }

    class GameInfoList : List<GameInfo>
    {
        public new void Sort()
        {
            this.Sort((lhs, rhs) => lhs.OrderInDate.CompareTo(rhs.OrderInDate));
        }

        public virtual bool CheckOrderIsContinuous(Int32 order = -1)
        {
            for (Int32 i = 0, j = this.Count; i != j; ++i)
            {
                if (this[i].OrderInDate != i)
                {
                    return false;
                }
            }
            return order == -1 
                || (this.Count == 0 && order == 0) 
                || this[this.Count - 1].OrderInDate + 1 == order;
        }

        public bool CheckDateIsSame()
        {
            if (this.Count == 0)
            {
                return true;
            }
            return CheckDateIsSame(this[0].GameDate);
        }

        public bool CheckDateIsSame(Date date)
        {
            if (this.Count == 0)
            {
                return true;
            }
            for (Int32 i = 0, j = this.Count; i != j; ++i)
            {
                if (this[i].GameDate != date)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual Int32 GetNextOrder()
        {
            return this.Count == 0 ? 0 
                : CheckOrderIsContinuous() ? this[this.Count - 1].OrderInDate + 1 
                : -1;
        }
    }

    class GameInfoPool : GameInfoList
    {
        private EventInfo eventInfo;

        public new void Sort()
        {
            this.Sort((lhs, rhs) => lhs.OrderInEvent.CompareTo(rhs.OrderInEvent));
        }

        public GameInfoPool(EventInfo parentEventInfo)
        {
            eventInfo = parentEventInfo;
        }

        public override bool CheckOrderIsContinuous(Int32 order = -1)
        {
            for (Int32 i = 0, j = this.Count; i != j; ++i)
            {
                if (this[i].OrderInEvent != i)
                {
                    return false;
                }
            }
            return order == -1 
                || (this.Count == 0 && order == 0) 
                || (this[this.Count - 1].OrderInEvent + 1 == order);
        }

        public override Int32 GetNextOrder()
        {
            return this.Count == 0 ? 0
                : CheckOrderIsContinuous() ? this[this.Count - 1].OrderInEvent + 1
                : -1;
        }

        public bool CheckNumberOfTeamIsDecreaseProgressively()
        {
            if (this.Count == 0)
            {
                return true;
            }

            for (Int32 i = this[0].NumberOfTeam == GameInfo.NoLimit ? 1 : 0, 
                j = this.Count - 1; i != j; ++i)
            {
                if (this[i].NumberOfTeam >= this[i + 1].NumberOfTeam)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckEventIsSame()
        {
            if (this.Count == 0)
            {
                return true;
            }
            for (Int32 i = 0, j = this.Count; i != j; ++i)
            {
                if (this[i].Event != eventInfo)
                {
                    return false;
                }
            }
            return true;
        }

        public GameInfo GenerateNewGameInfo()
        {
            GameInfo ret = null;
            if (eventInfo.Type == EventInfo.EventType.Dual)
            {
                ret = new DuelGameInfo(eventInfo)
                {
                    OrderInEvent = GetNextOrder()
                };
            }
            else if (eventInfo.Type == EventInfo.EventType.Ranking)
            {
                ret = new RankingGameInfo(eventInfo)
                {
                    OrderInEvent = GetNextOrder()
                };
            }
            else
            {
                throw new Exception("不存在的项目类型");
            }
            this.Add(ret);
            return ret;
        }
    }
}

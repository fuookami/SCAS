using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class GameInfo
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
            Ranking,			// 名次制
        };

        public const UInt32 NoLimit = 0;

        private String id;
        private String name;

        private RankingGameType type;
        private RankingGamePattern pattern;

        private UInt32 numberOfParticipants;
        private Session session;

        private Int32 orderInEvent;
        private Int32 orderInSession;

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

        public UInt32 NumberOfParticipants
        {
            get { return numberOfParticipants; }
            set { numberOfParticipants = value; }
        }

        public Session GameSession
        {
            get { return session; }
            set { session = value; }
        }

        public Int32 OrderInSession
        {
            get { return orderInSession; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("不能把比赛在当天的所有比赛中的序号设置为负数");
                }
                orderInSession = value;
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

        public GameInfo(EventInfo _event)
            : this(_event, Guid.NewGuid().ToString("N")) { }

        public GameInfo(EventInfo _event, String existedId)
        {
            id = existedId;
            groupInfo = new GroupInfo();
            eventInfo = _event;
        }
    }

    public class GameInfoList : List<GameInfo>
    {
        public new void Sort()
        {
            this.Sort((lhs, rhs) => lhs.OrderInSession.CompareTo(rhs.OrderInSession));
        }

        public virtual bool CheckOrderIsContinuous(Int32 order = -1)
        {
            for (Int32 i = 0, j = this.Count; i != j; ++i)
            {
                if (this[i].OrderInSession != i)
                {
                    return false;
                }
            }
            return order == -1 
                || (this.Count == 0 && order == 0) 
                || this[this.Count - 1].OrderInSession + 1 == order;
        }

        public bool CheckSessionIsSame(Session session)
        {
            if (this.Count == 0)
            {
                return true;
            }
            for (Int32 i = 0, j = this.Count; i != j; ++i)
            {
                if (this[i].GameSession == session)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual Int32 GetNextOrder()
        {
            return this.Count == 0 ? 0 
                : CheckOrderIsContinuous() ? this[this.Count - 1].OrderInSession + 1 
                : -1;
        }
    }

    public class GameInfoPool : GameInfoList
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

            for (Int32 i = this[0].NumberOfParticipants == GameInfo.NoLimit ? 1 : 0, 
                j = this.Count - 1; i != j; ++i)
            {
                if (this[i].NumberOfParticipants >= this[i + 1].NumberOfParticipants)
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

        public GameInfo GenerateNewGameInfo(String existedId = null)
        {
            GameInfo ret = null;
            ret = new GameInfo(eventInfo, existedId ?? Guid.NewGuid().ToString("N"))
            {
                OrderInEvent = GetNextOrder()
            };
            this.Add(ret);
            return ret;
        }
    }
}

using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class GameInfo
        {
            public enum GameType
            {
                Preliminary,        // 小组赛/预赛
                QuarterFinal,       // 复赛
                SemiFinal,          // 半决赛
                Finals              // 决赛
            };

            public enum GamePattern
            {
                Elimination,        // 淘汰制 / 竞标赛制（单组内算名次）
                Ranking,            // 名次制（所有组放在一起算名次）
            };

            private SSUtils.NumberRange _numberOfParticipants;
            
            private TimeSpan _planIntervalTime;
            private TimeSpan _planTimePerGroup;

            public String Id
            {
                get;
                internal set;
            }

            public String Name
            {
                get;
                set;
            }

            public GameType Type
            {
                get;
                set;
            }

            public GamePattern Pattern
            {
                get;
                set;
            }

            public UInt32 NumberOfParticipants
            {
                get
                {
                    return _numberOfParticipants.Maximum;
                }
                set
                {
                    _numberOfParticipants.Set(value, value);
                }
            }

            public Session GameSession
            {
                get;
                set;
            }

            public SSUtils.Order OrderInEvent
            {
                get;
                set;
            }

            public SSUtils.Order OrderInSession
            {
                get;
                set;
            }

            public TimeSpan PlanIntervalTime
            {
                get
                {
                    return _planIntervalTime;
                }
                set
                {
                    if (value.TotalSeconds <= 0)
                    {
                        throw new Exception("不能将比赛间计划间隔时间设置为负数");
                    }
                    _planIntervalTime = value;
                }
            }

            public TimeSpan PlanTimePerGroup
            {
                get
                {
                    return _planTimePerGroup;
                }
                set
                {
                    if (value.TotalSeconds <= 0)
                    {
                        throw new Exception("不能将每组比赛计划时间设置为负数");
                    }
                    _planTimePerGroup = value;
                }
            }

            public GroupInfo GameGroupInfo
            {
                get;
            }

            public EventInfo Event
            {
                get;
            }

            public GameInfo(EventInfo _event)
                : this(_event, Guid.NewGuid().ToString("N")) { }

            public GameInfo(EventInfo eventInfo, String existedId)
            {
                Id = existedId;
                _numberOfParticipants = new SSUtils.NumberRange();
                OrderInEvent = new SSUtils.Order();
                OrderInSession = new SSUtils.Order();
                GameGroupInfo = new GroupInfo();
                Event = eventInfo;
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
                    || this[this.Count - 1].OrderInSession.Value + 1 == order;
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
                    : CheckOrderIsContinuous() ? this[this.Count - 1].OrderInSession.Value + 1
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
                    || (this[this.Count - 1].OrderInEvent.Value + 1 == order);
            }

            public override Int32 GetNextOrder()
            {
                return this.Count == 0 ? 0
                    : CheckOrderIsContinuous() ? this[this.Count - 1].OrderInEvent.Value + 1
                    : -1;
            }

            public bool CheckNumberOfTeamIsDecreaseProgressively()
            {
                if (this.Count == 0)
                {
                    return true;
                }

                for (Int32 i = this[0].NumberOfParticipants == SSUtils.NumberRange.NoLimit ? 1 : 0,
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
                    OrderInEvent = new SSUtils.Order(GetNextOrder())
                };
                this.Add(ret);
                return ret;
            }
        }
    };
};

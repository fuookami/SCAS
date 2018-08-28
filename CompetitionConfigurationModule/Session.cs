using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class Session
        {
            public Date SessionDate
            {
                get;
            }

            public String Name
            {
                get;
                set;
            }

            public String FullName
            {
                get;
                set;
            }

            public SSUtils.Order OrderInDate
            {
                get;
                set;
            }

            internal Session(Date sessionDate, UInt32 order)
            {
                SessionDate = sessionDate;
                OrderInDate = new SSUtils.Order((Int32)order);
            }

            internal Session(Date sessionDate, SSUtils.Order order = new SSUtils.Order())
            {
                SessionDate = sessionDate;
                OrderInDate = order;
            }
        }

        public class SessionPool : Dictionary<Date, List<Session>>
        {
            public bool CheckOrderIsContinuous()
            {
                foreach (var datePair in this)
                {
                    if (datePair.Value == null)
                    {
                        return false;
                    }
                    if (datePair.Value.Count == 0)
                    {
                        return true;
                    }

                    for (Int32 i = 0, j = datePair.Value.Count; i != j; ++i)
                    {
                        if (datePair.Value[i].SessionDate != datePair.Key
                            || datePair.Value[i].OrderInDate != i)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            public bool ContainsDate(Date date)
            {
                return ContainsKey(date) && this[date] != null;
            }

            public void AddDate(Date date)
            {
                if (!ContainsDate(date))
                {
                    this[date] = new List<Session>();
                }
            }

            public Session GenerateNewSession(Date date)
            {
                AddDate(date);
                Session ret = new Session(date, GetNextOrder(date));
                this[date].Add(ret);
                return ret;
            }

            public UInt32 GetNextOrder(Date date)
            {
                return ContainsDate(date) ? (UInt32)this[date].Count : 0;
            }
        }
    };
};

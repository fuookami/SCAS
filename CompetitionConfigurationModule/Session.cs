using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    public class Session
    {
        private Date date;
        private String name;
        private String fullName;
        private UInt32 orderInDate;

        public Date SessionDate
        {
            get { return date; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public UInt32 OrderInDate
        {
            get { return orderInDate; }
            set { orderInDate = value; }
        }

        public Session(Date sessionDate, UInt32 order)
        {
            date = sessionDate;
            orderInDate = order;
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
}

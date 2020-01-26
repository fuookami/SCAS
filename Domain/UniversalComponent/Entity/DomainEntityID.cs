using System;

namespace SCAS.Utils
{
    public abstract class DomainEntityID
         : IComparable
    {
        public string ID { get; }

        public DomainEntityID()
        {
            ID = Guid.NewGuid().ToString("N");
        }

        public DomainEntityID(string id)
        {
            ID = id;
        }

        public int CompareTo(object obj)
        {
            if (obj is DomainEntityID rhs)
            {
                return string.Compare(ID, rhs.ID);
            }
            return 1;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DomainEntityID rhs)
            {
                return ID.Equals(rhs);
            }
            return false;
        }

        public new abstract string ToString();
    }
}

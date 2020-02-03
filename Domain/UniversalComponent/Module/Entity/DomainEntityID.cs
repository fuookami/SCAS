using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public abstract class DomainEntityID
         : IComparable
    {
        [DisallowNull] public string ID { get; }

        protected DomainEntityID()
        {
            ID = Guid.NewGuid().ToString("N");
        }

        protected DomainEntityID(string id)
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

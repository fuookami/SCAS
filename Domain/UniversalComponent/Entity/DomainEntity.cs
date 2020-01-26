using System;

namespace SCAS.Utils
{
    public abstract class DomainEntity<T, U>
        : IComparable, IPersistentType<T> 
        where T : IPersistentValue
        where U : DomainEntityID, new()
    {
        protected U id;

        public string ID { get { return id.ID; } }

        public DomainEntity(U entityID = null)
        {
            id = entityID ?? new U();
        }

        public int CompareTo(object obj)
        {
            if (obj is DomainEntityID rhs)
            {
                return ID.CompareTo(rhs);
            }
            return 1;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ID.Equals(obj);
        }

        public abstract T ToValue();
    }
}

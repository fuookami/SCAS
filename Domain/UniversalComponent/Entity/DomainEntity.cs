using System;

namespace SCAS.Utils
{
    public interface IDomainEntity<out T>
        : IComparable, IPersistentType<T> 
        where T : IPersistentValue
    {
    }

    public abstract class DomainEntity<T, U, P>
        : IDomainEntity<T> 
        where T : IPersistentValue
        where U : DomainEntityID, new()
        where P : DomainEntityID
    {
        public U ID { get; }

        public P PID { get; }

        public DomainEntity(P parentID, U id = null)
        {
            PID = parentID ?? throw new Exception("这不是一个可以为聚合根的类型。");
            ID = id ?? new U();
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

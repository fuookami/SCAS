using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public interface IDomainEntity
        : IComparable
    {
        public string ID { get; }
    }

    public abstract class DomainEntityValueBase
        : IPersistentValue
    {
        [NotNull] public string ID { get; internal set; }
    }

    public abstract class DomainEntity<T, U>
        : IDomainEntity, IPersistentType<T>
        where T : DomainEntityValueBase
        where U : DomainEntityID, new()
    {
        [DisallowNull] protected U id;

        [NotNull] public string ID => id.ID;

        protected DomainEntity(U entityID = null)
        {
            id = entityID ?? new U();
        }

        public int CompareTo(object obj)
        {
            if (obj is DomainEntity<T, U> rhs)
            {
                return id.CompareTo(rhs.id);
            }
            return 1;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DomainEntity<T, U> rhs)
            {
                return ID.Equals(rhs.id);
            }
            return false;
        }

        public abstract T ToValue();

        protected T ToValue(T value)
        {
            value.ID = this.ID;
            return value;
        }
    }
}

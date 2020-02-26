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
        public bool Deleted { get; internal set; }
    }

    public abstract class DomainEntityBase<U>
        : IDomainEntity
        where U : DomainEntityID, new()
    {
        [DisallowNull] protected U id;

        [NotNull] public U RawID => id;
        [NotNull] public string ID => id.ID;

        public bool Deleted { get; private set; }

        protected DomainEntityBase(U entityID = null)
        {
            id = entityID ?? new U();
            Deleted = false;
        }

        public void Delete()
        {
            Deleted = true;
        }

        public int CompareTo(object obj)
        {
            if (obj is DomainEntityBase<U> rhs)
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
            if (obj is DomainEntityBase<U> rhs)
            {
                return ID.Equals(rhs.id);
            }
            return false;
        }
    }

    public abstract class DomainEntity<T, U>
        : DomainEntityBase<U>, IPersistentType<T>
        where T : DomainEntityValueBase
        where U : DomainEntityID, new()
    {
        protected DomainEntity(U entityID = null)
            : base(entityID)
        {
        }

        public abstract T ToValue();

        protected T ToValue(T value)
        {
            value.ID = this.ID;
            value.Deleted = this.Deleted;
            return value;
        }
    }
}

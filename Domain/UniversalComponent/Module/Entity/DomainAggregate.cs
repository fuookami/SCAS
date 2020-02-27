using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public class DomainAggregateRootValueBase
        : DomainEntityValueBase
    {
        public bool Archived { get; internal set; }
        public bool Deleted { get; internal set; }
    }

    public abstract class DomainAggregateRoot<T, U>
        : DomainEntity<T, U>
        where T : DomainAggregateRootValueBase
        where U : DomainEntityID, new()
    {
        public bool Archived { get; private set; }
        public bool Deleted { get; protected set; }

        protected DomainAggregateRoot()
        {
            Archived = false;
            Deleted = false;
        }

        protected DomainAggregateRoot(U entityID, bool archived, bool deleted = true)
            : base(entityID)
        {
            Archived = archived;
            Deleted = deleted;
        }

        protected bool Archive()
        {
            if (Archived)
            {
                return false;
            }
            Archived = true;
            return true;
        }

        public virtual bool Delete()
        {
            if (Deleted)
            {
                return false;
            }
            Deleted = true;
            return true;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.Archived = Archived;
            value.Deleted = Deleted;
            return value;
        }
    }

    public class DomainRecyclableAggregateRootValueBase
        : DomainAggregateRootValueBase
    {
        public bool Recycled { get; internal set; }
    }

    public abstract class DomainRecyclableAggregateRoot<T, U>
        : DomainAggregateRoot<T, U>
        where T : DomainRecyclableAggregateRootValueBase
        where U : DomainEntityID, new()
    {
        public bool Recycled { get; private set; }

        protected DomainRecyclableAggregateRoot()
        {
            Recycled = false;
        }

        protected DomainRecyclableAggregateRoot(U entityID, bool archived, bool recycled, bool deleted = false)
            : base(entityID, archived, deleted)
        {
            Recycled = recycled;
        }

        public bool Recycle()
        {
            if (Recycled || Deleted)
            {
                return false;
            }
            Recycled = true;
            return true;
        }

        public override bool Delete()
        {
            if (Deleted || !Recycled)
            {
                return false;
            }
            Deleted = true;
            return true;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.Recycled = Recycled;
            return value;
        }
    }

    public abstract class DomainAggregateChild<T, U, P>
        : DomainEntity<T, U>
        where T : DomainEntityValueBase
        where U : DomainEntityID, new()
        where P : DomainEntityID
    {
        [DisallowNull] protected P pid;

        public P PID => pid;

        protected DomainAggregateChild(P parentEntityID, U entityID = null)
            : base(entityID)
        {
            pid = parentEntityID;
        }
    }
}

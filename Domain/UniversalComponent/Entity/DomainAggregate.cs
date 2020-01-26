using System;

namespace SCAS.Utils
{
    public abstract class DomainAggregateRoot<T, U>
        : DomainEntity<T, U>
        where T : IPersistentValue
        where U : DomainEntityID, new()
    {
        public DomainAggregateRoot(U entityID = null)
            : base(entityID) {}
    }

    public abstract class DomainAggregateChild<T, U, P>
        : DomainEntity<T, U>
        where T : IPersistentValue
        where U : DomainEntityID, new()
        where P : DomainEntityID
    {
        protected P pid;

        public DomainAggregateChild(P parentEntityID, U entityID = null)
            : base(entityID)
        {
            pid = parentEntityID ?? throw new Exception("非聚合根。");
        }
    }
}

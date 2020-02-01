using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Utils
{
    public abstract class DomainAggregateRoot<T, U>
        : DomainEntity<T, U>
        where T : DomainEntityValueBase
        where U : DomainEntityID, new()
    {
        protected DomainAggregateRoot(U entityID = null)
            : base(entityID) {}
    }

    public abstract class DomainAggregateChild<T, U, P>
        : DomainEntity<T, U>
        where T : DomainEntityValueBase
        where U : DomainEntityID, new()
        where P : DomainEntityID
    {
        [DisallowNull] protected P pid;

        protected DomainAggregateChild(P parentEntityID, U entityID = null)
            : base(entityID)
        {
            pid = parentEntityID;
        }
    }
}

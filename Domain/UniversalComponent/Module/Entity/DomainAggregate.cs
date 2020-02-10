﻿using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public class DomainAggregateRootValueBase
        : DomainEntityValueBase
    {
        public bool Archived { get; internal set; }
    }

    public abstract class DomainAggregateRoot<T, U>
        : DomainEntity<T, U>
        where T : DomainAggregateRootValueBase
        where U : DomainEntityID, new()
    {
        public bool Archived { get; protected set; }

        protected DomainAggregateRoot(U entityID = null)
            : base(entityID)
        {
            Archived = true;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.Archived = Archived;
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

        protected DomainAggregateChild(P parentEntityID, U entityID = null)
            : base(entityID)
        {
            pid = parentEntityID;
        }
    }
}

﻿using SCAS.Utils;

namespace SCAS.Module
{
    public interface IAggregateRepository<T, U>
        where T : DomainEntityBase<U>
        where U : DomainEntityID, new()
    {
        T Get(U id);
        Try Add(T entity);
        Try Save(T entity);
    }
}

using System;

namespace SCAS.Utils
{



    
    public abstract class DomainAggregate<T, U>
        : IDomainEntity<T> where T : IPersistentValue
        where U : DomainEntityID, new()
    {

    }

    public interface IDomainValue
    {
    }
}

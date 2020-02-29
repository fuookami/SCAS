using SCAS.Utils;

namespace SCAS.Module
{
    public interface IEventRepository<T>
        where T : IDomainEvent
    {
        public Try Save(T e);
    }
}

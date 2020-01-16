namespace SCAS.Utils
{
    public interface IDomainEntity<out T>
        : IPersistentType<T> where T : IPersistentValue
    {
    }

    public interface IDomainAggregate<out T>
        : IDomainEntity<T> where T : IPersistentValue
    {
    }

    public interface IDomainValue
    {
    }
}

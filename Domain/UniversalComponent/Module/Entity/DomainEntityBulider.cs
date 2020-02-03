namespace SCAS.Module
{
    public interface IDomainEntityBuilder<out T>
        where T : class, IDomainEntity
    {
        public T Build();
    }
}

namespace SCAS.Module
{
    public interface IPersistentValue
    {
    }

    public interface IPersistentType<out T>
        where T: IPersistentValue
    {
        public T ToValue();
    }
}

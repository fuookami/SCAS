namespace SCAS.Utils
{
    public class Modification<T>
    {
        public T OldValue { get; }
        public T NewValue { get; }

        public Modification(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        static public Modification<T> Make(T oldValue, T newValue)
        {
            return newValue == null ? null
                : new Modification<T>(oldValue, newValue);
        }
    }
}

using System;

namespace SCAS.Utils
{
    public interface IDomainEvent
        : IDomainValue
    {
        public uint Code { get; }
        public string Message { get; }
        public string Digest { get; }
        public string Data { get; }

        public DateTime PostTime { get; }
    }

    public abstract class DomainEventValueBase
        : IPersistentValue
    {
        public uint Code { get; internal set; }
        public string Message { get; internal set; }
        public string Digest { get; internal set; }
        public string Data { get; internal set; }

        public DateTime PostTime { get; internal set; }
    }

    public abstract class DomainEventBase<T>
        : IComparable, IDomainEvent, IPersistentType<T>
        where T : DomainEventValueBase
    {
        public uint Code { get; }
        public string Message { get; }
        public string Digest { get; }
        public string Data { get; }

        public DateTime PostTime { get; }

        protected DomainEventBase(uint code, string message, string data)
        {
            
        }


        public int CompareTo(object obj)
        {
            if (obj is DomainEventBase<T> rhs)
            {
                return Digest.CompareTo(rhs.Digest);
            }
            return 1;
        }

        public override int GetHashCode()
        {
            return Digest.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DomainEventBase<T> rhs)
            {
                return Digest.Equals(rhs.Digest);
            }
            return false;
        }

        public abstract T ToValue();

        protected T ToValue(T value)
        {
            value.Code = this.Code;
            value.Message = this.Message;
            value.Digest = this.Digest;
            value.Data = this.Data;
            value.PostTime = PostTime;
            return value;
        }
    }
}

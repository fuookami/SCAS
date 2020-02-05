using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public interface IDomainEvent
        : IDomainValue
    {
        public uint Code { get; }
        public uint Type { get; }
        public uint Level { get; }
        public uint Priority { get; }
        [NotNull] public string Message { get; }
        [DisallowNull] public string Digest { get; }
        [DisallowNull] public string Data { get; }

        public DateTime PostTime { get; }
    }

    public class DomainEventValue
        : IPersistentValue
    {
        public uint Code { get; internal set; }
        public uint Type { get; internal set; }
        public uint Level { get; internal set; }
        public uint Priority { get; internal set; }
        [NotNull] public string Digest { get; internal set; }
        [NotNull] public string Data { get; internal set; }

        public DateTime PostTime { get; internal set; }
    }

    public abstract class DomainEventBase<T>
        : IComparable, IDomainEvent, IPersistentType<T>
        where T : DomainEventValue
    {
        public uint Code { get; }
        public uint Type { get; }
        public uint Level { get; }
        public uint Priority { get; }
        [NotNull] public abstract string Message { get; }
        [DisallowNull] public string Digest { get; protected set; }
        [DisallowNull] public string Data { get; protected set; }

        public DateTime PostTime { get; }

        protected DomainEventBase(uint code, uint type, uint level, uint priority)
        {
            Code = code;
            Type = type;
            Level = level;
            Priority = priority;
            PostTime = DateTime.Now;
        }

        protected DomainEventBase(uint code, uint type, uint level, uint priority, DateTime postTime)
            : this(code, type, level, priority)
        {
            PostTime = postTime;
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
            value.Type = this.Type;
            value.Level = this.Level;
            value.Priority = this.Priority;
            value.Digest = this.Digest;
            value.Data = this.Data;
            value.PostTime = PostTime;
            return value;
        }
    }
}

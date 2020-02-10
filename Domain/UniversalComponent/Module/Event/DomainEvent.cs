using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SCAS.Module
{
    public class DomainEventID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("DomainEvent-{0}", ID);
        }
    }

    public interface IDomainEvent
        : IDomainEntity
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
        : DomainEntityValueBase
    {
        public string TriggerID { get; internal set; }

        public uint Code { get; internal set; }
        public uint Type { get; internal set; }
        public uint Level { get; internal set; }
        public uint Priority { get; internal set; }
        [NotNull] public string Digest { get; internal set; }
        [NotNull] public string Data { get; internal set; }

        public DateTime PostTime { get; internal set; }
    }

    public abstract class DomainEventBase<T, DTO>
        : DomainEntity<T, DomainEventID>
        where T : DomainEventValue
    {
        public IDomainEvent Trigger { get; }

        public uint Code { get; }
        public uint Type { get; }
        public uint Level { get; }
        public uint Priority { get; }
        public DateTime PostTime { get; }

        [NotNull] public abstract string Message { get; }
        [DisallowNull] public string Digest { get; }
        [DisallowNull] public string Data { get; }
        [DisallowNull] public DTO DataObj { get; }

        protected DomainEventBase(uint code, uint type, uint level, uint priority, DTO obj, IExtractor extractor = null)
        {
            Code = code;
            Type = type;
            Level = level;
            Priority = priority;
            PostTime = DateTime.Now;

            DataObj = obj;
            Data = JsonConvert.SerializeObject(DataObj);
            Digest = Encoding.UTF8.GetString(extractor.Extract(Encoding.UTF8.GetBytes(Data)));
        }

        protected DomainEventBase(IDomainEvent trigger, uint code, uint type, uint level, uint priority, DTO obj, IExtractor extractor = null)
            : this(code, type, level, priority, obj, extractor)
        {
            Trigger = trigger;
        }

        protected DomainEventBase(DomainEventID id, uint code, uint type, uint level, uint priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id)
        {
            Code = code;
            Type = type;
            Level = level;
            Priority = priority;
            PostTime = postTime;

            DataObj = obj;
            Data = JsonConvert.SerializeObject(DataObj);
            Digest = Encoding.UTF8.GetString(extractor.Extract(Encoding.UTF8.GetBytes(Data)));
        }

        protected DomainEventBase(DomainEventID id, IDomainEvent trigger, uint code, uint type, uint level, uint priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : this(id, code, type, level, priority, postTime, obj, extractor)
        {
            Trigger = trigger;
        }

        public new int CompareTo(object obj)
        {
            if (obj is DomainEventBase<T, DTO> rhs)
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
            if (obj is DomainEventBase<T, DTO> rhs)
            {
                return Digest.Equals(rhs.Digest);
            }
            return false;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.TriggerID = this.Trigger.ID;
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

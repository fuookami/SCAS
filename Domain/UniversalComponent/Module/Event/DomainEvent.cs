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
        public SCASModuleCode ModuleCode { get; }
        public uint EventCode { get; }
        public SCASEventType Type { get; }
        public SCASEventLevel Level { get; }
        public SCASEventPriority Priority { get; }
        [NotNull] public string Message { get; }
        [DisallowNull] public string Digest { get; }
        [DisallowNull] public string Data { get; }

        public DateTime PostTime { get; }
    }

    public class DomainEventValue
        : DomainEntityValueBase
    {
        public string TriggerID { get; internal set; }
        public string OperatorID { get; internal set; }

        public uint ModuleCode { get; internal set; }
        public uint EventCode { get; internal set; }
        public uint Type { get; internal set; }
        public uint Level { get; internal set; }
        public uint Priority { get; internal set; }
        [NotNull] public string Digest { get; internal set; }
        [NotNull] public string Data { get; internal set; }

        public DateTime PostTime { get; internal set; }
    }

    public abstract class DomainEventBase<T, DTO>
        : DomainEntity<T, DomainEventID>, IDomainEvent
        where T : DomainEventValue
    {
        public IDomainEvent Trigger { get; }

        public SCASModuleCode ModuleCode { get; }
        public uint EventCode { get; }
        public SCASEventType Type { get; }
        public SCASEventLevel Level { get; }
        public SCASEventPriority Priority { get; }
        public DateTime PostTime { get; }

        [NotNull] public abstract string Message { get; }
        [DisallowNull] public string Digest { get; }
        [DisallowNull] public string Data { get; }
        [DisallowNull] public DTO DataObj { get; }

        protected DomainEventBase(IDomainEvent trigger, SCASModuleCode moduleCode, uint eventCode, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
        {
            Trigger = trigger;

            ModuleCode = moduleCode;
            EventCode = eventCode;
            Type = type;
            Level = level;
            Priority = priority;
            PostTime = DateTime.Now;

            DataObj = obj;
            Data = JsonConvert.SerializeObject(DataObj);
            Digest = Encoding.UTF8.GetString(extractor.Extract(Encoding.UTF8.GetBytes(Data)));
        }

        protected DomainEventBase(DomainEventID id, IDomainEvent trigger, SCASModuleCode moduleCode, uint eventCode, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id)
        {
            Trigger = trigger;

            ModuleCode = moduleCode;
            EventCode = eventCode;
            Type = type;
            Level = level;
            Priority = priority;
            PostTime = postTime;

            DataObj = obj;
            Data = JsonConvert.SerializeObject(DataObj);
            Digest = Encoding.UTF8.GetString(extractor.Extract(Encoding.UTF8.GetBytes(Data)));
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
            value.TriggerID = this.Trigger?.ID;
            value.ModuleCode = (uint)this.ModuleCode;
            value.EventCode = this.EventCode;
            value.Type = (uint)this.Type;
            value.Level = (uint)this.Level;
            value.Priority = (uint)this.Priority;
            value.Digest = this.Digest;
            value.Data = this.Data;
            value.PostTime = PostTime;
            return value;
        }
    }

    public abstract class DomainArtificialEventBase<T, DTO, O>
        : DomainEventBase<T, DTO>
        where T : DomainEventValue
        where O : IDomainEntity
    {
        public O Operator { get; }

        protected DomainArtificialEventBase(IDomainEvent trigger, SCASModuleCode moduleCode, uint eventCode, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
            : base(trigger, moduleCode, eventCode, type, level, priority, obj, extractor)
        {
        }

        protected DomainArtificialEventBase(O op, SCASModuleCode moduleCode, uint eventCode, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
            : this(null, moduleCode, eventCode, type, level, priority, obj, extractor)
        {
            Operator = op;
        }

        protected DomainArtificialEventBase(DomainEventID id, IDomainEvent trigger, SCASModuleCode moduleCode, uint eventCode, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id, trigger, moduleCode, eventCode, type, level, priority, postTime, obj, extractor)
        {
        }

        protected DomainArtificialEventBase(DomainEventID id, O op, SCASModuleCode moduleCode, uint eventCode, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : this(id, null, moduleCode, eventCode, type, level, priority, postTime, obj, extractor)
        {
            Operator = op;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.OperatorID = this.Operator?.ID;
            return value;
        }
    }
}

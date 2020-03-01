using SCAS.Module;
using System;

namespace SCAS.Domain.UserContext
{
    public enum UserContextEvent
    {
        RegionCreated,
        RegionInfoModified,
        RegionArchived,
        RegionDeleted,

        OrganizationCreated,
        OrganizationInfoModified,
        OrganizationRegisterInitiated,
        OrganizationRegisterApproved,
        OrganizationRegisterUnapproved,
        OrganizationRegistered,
        OrganizationRegisterInfoModified,
        OrganizationArchived,
        OrganizationRegisterArchived,
        OrganizationDeleted,
        OrganizationRegisterDeleted,

        PersonCreated,
        PersonInfoModified,
        PersonRegisterInitiated,
        PersonRegisterApproved,
        PersonRegisterUnapproved,
        PersonRegistered,
        PersonRegisterInfoModified,
        PersonArchived,
        PersonRegisterArchived,
        PersonDeleted,
        PersonRegisterDeleted
    };

    public interface IUserContextEvent
        : IDomainEvent
    {
        public DomainEventValue ToValue();
    }

    public abstract class UserContextEventBase<DTO>
        : DomainEventBase<DomainEventValue, DTO>, IUserContextEvent
    {
        protected UserContextEventBase(IDomainEvent trigger, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
            : base(trigger, SCASModuleCode.UserContext, (uint)code, type, level, priority, obj, extractor)
        {
        }

        protected UserContextEventBase(DomainEventID id, IDomainEvent trigger, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id, trigger, SCASModuleCode.UserContext, (uint)code, type, level, priority, postTime, obj, extractor)
        {
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }

    public abstract class UserContextArtificialEventBase<DTO>
        : DomainArtificialEventBase<DomainEventValue, DTO, Person>, IUserContextEvent
    {
        protected UserContextArtificialEventBase(IDomainEvent trigger, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
            : base(trigger, SCASModuleCode.UserContext, (uint)code, type, level, priority, obj, extractor)
        {
        }

        protected UserContextArtificialEventBase(Person op, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
            : base(op, SCASModuleCode.UserContext, (uint)code, type, level, priority, obj, extractor)
        {
        }

        protected UserContextArtificialEventBase(DomainEventID id, IDomainEvent trigger, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id, trigger, SCASModuleCode.UserContext, (uint)code, type, level, priority, postTime, obj, extractor)
        {
        }

        protected UserContextArtificialEventBase(DomainEventID id, Person op, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id, op, SCASModuleCode.UserContext, (uint)code, type, level, priority, postTime, obj, extractor)
        {
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

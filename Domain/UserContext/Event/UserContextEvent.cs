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
        OrganizationRegisterArchived,
        OrganizationArchived,

        PersonCreated,
        PersonInfoModified,
        PersonRegisterInitiated,
        PersonRegisterApproved,
        PersonRegisterUnapproved,
        PersonRegistered,
        PersonRegisterInfoModified,
        PersonRegisterArchived,
        PersonArchived
    };

    public abstract class UserContextEventBase<T, DTO>
        : DomainEventBase<T, DTO>
        where T : DomainEventValue
    {
        protected UserContextEventBase(IDomainEvent trigger, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DTO obj, IExtractor extractor = null)
            : base(trigger, SCASModuleCode.UserContext, (uint)code, type, level, priority, obj, extractor)
        {
        }

        protected UserContextEventBase(DomainEventID id, IDomainEvent trigger, UserContextEvent code, SCASEventType type, SCASEventLevel level, SCASEventPriority priority, DateTime postTime, DTO obj, IExtractor extractor = null)
            : base(id, trigger, SCASModuleCode.UserContext, (uint)code, type, level, priority, postTime, obj, extractor)
        {
        }
    }

    public abstract class UserContextArtificialEventBase<T, DTO>
        : DomainArtificialEventBase<T, DTO, Person>
        where T : DomainEventValue
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
    }
}

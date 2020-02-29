using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterArchivedEventData
    {
        public string ID { get; }

        internal OrganizationRegisterArchivedEventData(OrganizationRegister register)
        {
            ID = register.ID;
        }
    }

    public class OrganizationRegisterArchivedEvent
        : UserContextArtificialEventBase<DomainEventValue, OrganizationRegisterArchivedEventData>
    {
        [NotNull] private OrganizationRegister register;

        public override string Message => string.Format("Register of organization {0} to region {1} archived.", register.RegisteredRegion.Info.Name, register.Org.Info.Name);

        private OrganizationRegisterArchivedEvent(IDomainEvent trigger, OrganizationRegister targetRegister, IExtractor extractor)
            : base(trigger, UserContextEvent.OrganizationRegisterArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal OrganizationRegisterArchivedEvent(Person op, OrganizationRegister targetRegister, IExtractor extractor)
            : base(op, UserContextEvent.OrganizationRegisterArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal OrganizationRegisterArchivedEvent(OrganizationRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)null, targetRegister, extractor) { }

        internal OrganizationRegisterArchivedEvent(OrganizationArchivedEvent trigger, OrganizationRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        internal OrganizationRegisterArchivedEvent(RegionArchivedEvent trigger, OrganizationRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

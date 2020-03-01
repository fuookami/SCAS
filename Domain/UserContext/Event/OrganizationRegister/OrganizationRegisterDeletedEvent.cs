using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterDeletedEventData
    {
        public string ID { get; }

        internal OrganizationRegisterDeletedEventData(OrganizationRegister register)
        {
            ID = register.ID;
        }
    }

    public class OrganizationRegisterDeletedEvent
        : UserContextArtificialEventBase<OrganizationRegisterArchivedEventData>
    {
        [NotNull] private OrganizationRegister register;

        public override string Message => string.Format("Register of organization {0} to region {1} deleted.", register.RegisteredRegion.Info.Name, register.Org.Info.Name);

        private OrganizationRegisterDeletedEvent(IDomainEvent trigger, OrganizationRegister targetRegister, IExtractor extractor)
            : base(trigger, UserContextEvent.OrganizationRegisterDeleted, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal OrganizationRegisterDeletedEvent(Person op, OrganizationRegister targetRegister, IExtractor extractor = null)
            : base(op, UserContextEvent.OrganizationRegisterDeleted, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal OrganizationRegisterDeletedEvent(OrganizationDeletedEvent trigger, OrganizationRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        internal OrganizationRegisterDeletedEvent(RegionDeletedEvent trigger, OrganizationRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }
    }
}

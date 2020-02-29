using SCAS.Module;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationDeletedEventData
    {
        public string ID { get; }

        internal OrganizationDeletedEventData(Organization org)
        {
            ID = org.ID;
        }
    }

    public class OrganizationDeletedEvent
        : UserContextArtificialEventBase<DomainEventValue, OrganizationDeletedEventData>
    {
        private Organization org;

        public override string Message => string.Format("Organization {0} was deleted.", org.Info.Name);

        internal OrganizationDeletedEvent(Person op, Organization targetOrg, IExtractor extractor = null)
            : base(op, UserContextEvent.OrganizationDeleted, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationDeletedEventData(targetOrg), extractor)
        {
            org = targetOrg;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

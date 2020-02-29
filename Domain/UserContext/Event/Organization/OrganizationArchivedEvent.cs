using SCAS.Module;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationArchivedEventData
    {
        public string ID { get; }

        internal OrganizationArchivedEventData(Organization org)
        {
            ID = org.ID;
        }
    }

    public class OrganizationArchivedEvent
        : UserContextArtificialEventBase<DomainEventValue, OrganizationArchivedEventData>
    {
        private Organization org;

        public override string Message => string.Format("Organization {0} was archived.", org.Info.Name);

        internal OrganizationArchivedEvent(Person op, Organization targetOrg, IExtractor extractor = null)
            : base(op, UserContextEvent.OrganizationArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationArchivedEventData(targetOrg), extractor)
        {
            org = targetOrg;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

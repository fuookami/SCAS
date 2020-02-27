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
        : DomainArtificialEventBase<DomainEventValue, OrganizationArchivedEventData, Person>
    {
        private Organization org;

        public override string Message => string.Format("Organization {0} archived.", org.Info.Name);

        internal OrganizationArchivedEvent(Person op, Organization targetOrg, IExtractor extractor = null)
            : base(op, (uint)SCASEvent.OrganizationArchived, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationArchivedEventData(targetOrg), extractor)
        {
            org = targetOrg;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

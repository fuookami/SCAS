using SCAS.Module;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationCreatedEventData
    {
        public string ID { get; }
        public string SID { get; }
        public string Name { get; }

        public OrganizationCreatedEventData(Organization org)
        {
            ID = org.ID;
            SID = org.SID;
            Name = org.Info.Name;
        }
    }

    public class OrganizationCreatedEvent
        : DomainArtificialEventBase<DomainEventValue, OrganizationCreatedEventData, Person>
    {
        private Organization org;

        public override string Message => string.Format("Organization {0} created.", org.Info.Name);

        internal OrganizationCreatedEvent(Person op, Organization newOrg, IExtractor extractor = null)
            : base(op, (uint)SCASEvent.OrganizationRegisterInitiated, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationCreatedEventData(newOrg), extractor)
        {
            org = newOrg;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

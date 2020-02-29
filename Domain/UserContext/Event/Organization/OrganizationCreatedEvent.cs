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
        : UserContextArtificialEventBase<DomainEventValue, OrganizationCreatedEventData>
    {
        private Organization org;

        public override string Message => string.Format("Organization {0} created.", org.Info.Name);

        internal OrganizationCreatedEvent(Person op, Organization newOrg, IExtractor extractor = null)
            : base(op, UserContextEvent.OrganizationRegisterInitiated, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new OrganizationCreatedEventData(newOrg), extractor)
        {
            org = newOrg;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

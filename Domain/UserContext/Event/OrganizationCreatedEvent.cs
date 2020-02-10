﻿using SCAS.Module;

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
        : DomainEventBase<DomainEventValue, OrganizationCreatedEventData>
    {
        private Organization org;

        public override string Message { get { return GetMessage(); } }

        internal OrganizationCreatedEvent(Organization newOrg, IExtractor extractor = null)
            : base((uint)SCASEvent.OrganizationCreated, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationCreatedEventData(newOrg), extractor)
        {
            org = newOrg;
        }

        private string GetMessage()
        {
            return string.Format("Organization {0} created", org.Info.Name);
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}
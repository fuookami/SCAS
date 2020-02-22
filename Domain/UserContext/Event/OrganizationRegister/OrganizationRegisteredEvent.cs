using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisteredEventData
    {
        public string ID { get; }
        public string SID { get; }
        public string OrgID { get; }
        public string RegionID { get; }
        public uint PrefixCode { get; }

        internal OrganizationRegisteredEventData(OrganizationRegister register)
        {
            ID = register.ID;
            SID = register.SID;
            OrgID = register.Org.ID;
            RegionID = register.RegisteredRegion.ID;
            PrefixCode = register.Info.PrefixCode;
        }
    }

    public class OrganizationRegisteredEvent
        : DomainEventBase<DomainEventValue, OrganizationRegisteredEventData>
    {
        [NotNull] private OrganizationRegister register;

        public override string Message => string.Format("Organization {0} registered to region {1} with prefix code {2}.", register.Org.Info.Name, register.RegisteredRegion.Info.Name, register.Info.PrefixCode);

        internal OrganizationRegisteredEvent(OrganizationRegisterApprovedEvent trigger, OrganizationRegister targetRegister, IExtractor extractor = null)
            : base(trigger, (uint)SCASEvent.OrganizationRegistered, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationRegisteredEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

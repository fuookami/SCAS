using SCAS.Module;
using SCAS.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterInitiatationEventData
    {
        public string ID { get; }
        public string SID { get; }
        public string OrgID { get; }
        public string RegionID { get; }
        public string InitiatorID { get; }
        public uint PrefixCode { get; }

        internal OrganizationRegisterInitiatationEventData(OrganizationRegisterForm form)
        {
            ID = form.ID;
            SID = form.SID;
            OrgID = form.Org.ID;
            RegionID = form.RegisteredRegion.ID;
            InitiatorID = form.Info.Initiator.ID;
            PrefixCode = form.Info.PrefixCode;
        }
    }

    public class OrganizationRegisterInitiatationEvent
        : DomainEventBase<DomainEventValue, OrganizationRegisterInitiatationEventData>
    {
        [NotNull] private OrganizationRegisterForm form;

        public override string Message { get { return string.Format("Register of organization {0} to region {1} initiated by {2}, prefix code {3}", form.Org.Info.Name, form.RegisteredRegion.Info.Name, form.Info.Initiator.Info.Name, form.Info.PrefixCode); } }

        internal OrganizationRegisterInitiatationEvent(OrganizationRegisterForm newForm, IExtractor extractor = null)
            : base((uint)SCASEvent.OrganizationInfoModified, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationRegisterInitiatationEventData(newForm), extractor)
        {
            form = newForm;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

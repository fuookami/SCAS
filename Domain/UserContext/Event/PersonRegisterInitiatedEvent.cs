using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterInitiatedEventData
    {
        public string ID { get; }
        public string SID { get; }
        public string PersonID { get; }
        public string RegionID { get; }
        public string InitiatorID { get; }
        public string OrgID { get; }

        internal PersonRegisterInitiatedEventData(PersonRegisterForm form)
        {
            ID = form.ID;
            SID = form.SID;
            PersonID = form.Person.ID;
            RegionID = form.RegisteredRegion.ID;
            InitiatorID = form.Info.Initiator.ID;
            OrgID = form.BelongingOrganization?.ID;
        }
    }

    public class PersonRegisterInitiatedEvent
        : DomainEventBase<DomainEventValue, PersonRegisterInitiatedEventData>
    {
        [NotNull] private PersonRegisterForm form;

        public override string Message { get { return GetMessage(); } }

        internal PersonRegisterInitiatedEvent(PersonRegisterForm newForm, IExtractor extractor = null)
            : base((uint)SCASEvent.PersonRegisterInitiated, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonRegisterInitiatedEventData(newForm), extractor)
        {
            form = newForm;
        }

        private string GetMessage()
        {
            var ret = string.Format("Register of person {0} to region {1} initiated by {2}", form.Person.Info.Name, form.RegisteredRegion.Info.Name, form.Info.Initiator.Info.Name);
            if (form.BelongingOrganization != null)
            {
                ret += string.Format(", belonging organization {0}", form.BelongingOrganization.Info.Name);
            }
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

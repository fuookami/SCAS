using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterApprovedEventData
    {
        public string ID { get; }
        public string ExaminerID { get; }
        public string Annotation { get; }

        internal OrganizationRegisterApprovedEventData(OrganizationRegisterForm form)
        {
            ID = form.ID;
            ExaminerID = form.Examination.Examiner.ID;
            Annotation = form.Examination.Annotation;
        }
    }

    public class OrganizationRegisterApprovedEvent
        : DomainArtificialEventBase<DomainEventValue, OrganizationRegisterApprovedEventData, Person>
    {
        [NotNull] private OrganizationRegisterForm form;

        public override string Message => string.Format("Register of organization {0} to region {1} approved by {2}.", form.Org.Info.Name, form.RegisteredRegion.Info.Name, form.Examination.Examiner.Info.Name);

        internal OrganizationRegisterApprovedEvent(Person op, OrganizationRegisterForm targetForm, IExtractor extractor = null)
            : base(op, (uint)SCASEvent.OrganizationRegisterApproved, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationRegisterApprovedEventData(targetForm), extractor)
        {
            form = targetForm;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

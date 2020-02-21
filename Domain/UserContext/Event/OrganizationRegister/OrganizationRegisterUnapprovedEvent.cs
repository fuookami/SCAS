using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterUnapprovedEventData
    {
        public string ID { get; }
        public string ExaminerID { get; }
        public string Annotation { get; }

        internal OrganizationRegisterUnapprovedEventData(OrganizationRegisterForm form)
        {
            ID = form.ID;
            ExaminerID = form.Examination.Examiner.ID;
            Annotation = form.Examination.Annotation;
        }
    }

    public class OrganizationRegisterUnapprovedEvent
        : DomainEventBase<DomainEventValue, OrganizationRegisterUnapprovedEventData>
    {
        [NotNull] private OrganizationRegisterForm form;

        public override string Message => string.Format("Register of organization {0} to region {1} unapproved by {2}", form.Org.Info.Name, form.RegisteredRegion.Info.Name, form.Examination.Examiner.Info.Name);

        internal OrganizationRegisterUnapprovedEvent(OrganizationRegisterForm targetForm, IExtractor extractor = null)
            : base((uint)SCASEvent.OrganizationRegisterUnapproved, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationRegisterUnapprovedEventData(targetForm), extractor)
        {
            form = targetForm;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterUnapprovedEventData
    {
        public string ID { get; }
        public string ExaminerID { get; }
        public string Annotation { get; }

        internal PersonRegisterUnapprovedEventData(PersonRegisterForm form)
        {
            ID = form.ID;
            ExaminerID = form.Examination.Examiner.ID;
            Annotation = form.Examination.Annotation;
        }
    }

    public class PersonRegisterUnapprovedEvent
        : DomainEventBase<DomainEventValue, PersonRegisterUnapprovedEventData>
    {
        [NotNull] private PersonRegisterForm form;

        public override string Message => GetMessage();

        internal PersonRegisterUnapprovedEvent(PersonRegisterForm targetForm, IExtractor extractor = null)
            : base((uint)SCASEvent.PersonRegisterUnapproved, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonRegisterUnapprovedEventData(targetForm), extractor)
        {
            form = targetForm;
        }

        private string GetMessage()
        {
            var ret = string.Format("Register of person {0} to region {1} Unapproved by {2}", form.Person.Info.Name, form.RegisteredRegion.Info.Name, form.Examination.Examiner.Info.Name);
            if (form.BelongingOrganization != null)
            {
                ret += string.Format(", belonging organization {0}", form.BelongingOrganization.Info.Name);
            }
            ret += ".";
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

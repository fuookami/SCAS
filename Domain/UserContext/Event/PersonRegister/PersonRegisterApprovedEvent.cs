using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterApprovedEventData
    {
        public string ID { get; }
        public string ExaminerID { get; }
        public string Annotation { get; }

        internal PersonRegisterApprovedEventData(PersonRegisterForm form)
        {
            ID = form.ID;
            ExaminerID = form.Examination.Examiner.ID;
            Annotation = form.Examination.Annotation;
        }
    }

    public class PersonRegisterApprovedEvent
        : DomainArtificialEventBase<DomainEventValue, PersonRegisterApprovedEventData, Person>
    {
        [NotNull] private PersonRegisterForm form;

        public override string Message => GetMessage();

        internal PersonRegisterApprovedEvent(Person op, PersonRegisterForm targetForm, IExtractor extractor = null)
            : base(op, (uint)SCASEvent.PersonRegisterApproved, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonRegisterApprovedEventData(targetForm), extractor)
        {
            form = targetForm;
        }

        private string GetMessage()
        {
            var ret = string.Format("Register of person {0} to region {1} approved by {2}", form.Person.Info.Name, form.RegisteredRegion.Info.Name, form.Examination.Examiner.Info.Name);
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

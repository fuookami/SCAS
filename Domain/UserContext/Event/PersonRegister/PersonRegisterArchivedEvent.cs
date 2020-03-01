using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterArchivedEventData
    {
        public string ID { get; }

        internal PersonRegisterArchivedEventData(PersonRegister register)
        {
            ID = register.ID;
        }
    }

    public class PersonRegisterArchivedEvent
        : UserContextArtificialEventBase<PersonRegisterArchivedEventData>
    {
        [NotNull] private PersonRegister register;

        public override string Message => GetMessage();

        private PersonRegisterArchivedEvent(IDomainEvent trigger, PersonRegister targetRegister, IExtractor extractor)
            : base(trigger, UserContextEvent.PersonRegisterArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal PersonRegisterArchivedEvent(Person op, PersonRegister targetRegister, IExtractor extractor = null)
            : base(op, UserContextEvent.PersonRegisterArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal PersonRegisterArchivedEvent(PersonArchivedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        internal PersonRegisterArchivedEvent(RegionArchivedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        internal PersonRegisterArchivedEvent(OrganizationArchivedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        private string GetMessage()
        {
            var ret = string.Format("Register of person {0} to region {1} archived", register.Person.Info.Name, register.RegisteredRegion.Info.Name);
            if (register.BelongingOrganization != null)
            {
                ret += string.Format(", belonging organization {0}", register.BelongingOrganization.Info.Name);
            }
            ret += ".";
            return ret;
        }
    }
}

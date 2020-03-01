using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterDeletedEventData
    {
        public string ID { get; }

        internal PersonRegisterDeletedEventData(PersonRegister register)
        {
            ID = register.ID;
        }
    }

    public class PersonRegisterDeletedEvent
        : UserContextArtificialEventBase<PersonRegisterDeletedEventData>
    {
        [NotNull] private PersonRegister register;

        public override string Message => GetMessage();

        internal PersonRegisterDeletedEvent(IDomainEvent trigger, PersonRegister targetRegister, IExtractor extractor)
            : base(trigger, UserContextEvent.PersonRegisterDeleted, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonRegisterDeletedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal PersonRegisterDeletedEvent(Person op, PersonRegister targetRegister, IExtractor extractor = null)
            : base(op, UserContextEvent.PersonRegisterDeleted, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonRegisterDeletedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal PersonRegisterDeletedEvent(PersonDeletedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        internal PersonRegisterDeletedEvent(RegionDeletedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        internal PersonRegisterDeletedEvent(OrganizationDeletedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)trigger, targetRegister, extractor) { }

        private string GetMessage()
        {
            var ret = string.Format("Register of person {0} to region {1} was deleted", register.Person.Info.Name, register.RegisteredRegion.Info.Name);
            if (register.BelongingOrganization != null)
            {
                ret += string.Format(", belonging organization {0}", register.BelongingOrganization.Info.Name);
            }
            ret += ".";
            return ret;
        }
    }
}

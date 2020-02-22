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
        : DomainEventBase<DomainEventValue, PersonRegisterArchivedEventData>
    {
        [NotNull] private PersonRegister register;

        public override string Message => GetMessage();

        private PersonRegisterArchivedEvent(IDomainEvent trigger, PersonRegister targetRegister, IExtractor extractor)
            : base(trigger, (uint)SCASEvent.PersonRegisterArchived, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonRegisterArchivedEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        internal PersonRegisterArchivedEvent(PersonRegister targetRegister, IExtractor extractor = null)
            : this((IDomainEvent)null, targetRegister, extractor) { }

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

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

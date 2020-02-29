using SCAS.Module;

namespace SCAS.Domain.UserContext
{
    public struct PersonDeletedEventData
    {
        public string ID { get; }

        internal PersonDeletedEventData(Person person)
        {
            ID = person.ID;
        }
    }

    public class PersonDeletedEvent
        : UserContextArtificialEventBase<DomainEventValue, PersonDeletedEventData>
    {
        private Person person;

        public override string Message => string.Format("Person {0} was deleted.", person.Info.Name);

        internal PersonDeletedEvent(Person op, Person targetPerson, IExtractor extractor = null)
            : base(op, UserContextEvent.OrganizationArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonDeletedEventData(targetPerson), extractor)
        {
            person = targetPerson;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

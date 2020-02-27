using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonCreatedEventData
    {
        public string ID { get; }
        public string SID { get; }
        public string Name { get; }

        internal PersonCreatedEventData(Person person)
        {
            ID = person.ID;
            SID = person.SID;
            Name = person.Info.Name;
        }
    }

    public class PersonCreatedEvent
        : DomainArtificialEventBase<DomainEventValue, PersonCreatedEventData, Person>
    {
        private Person person;

        public override string Message => string.Format("Person {0} created.", person.Info.Name);

        internal PersonCreatedEvent(Person op, Person newPerson, IExtractor extractor = null)
            : base(op, (uint)SCASEvent.OrganizationCreated, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonCreatedEventData(newPerson), extractor)
        {
            person = newPerson;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

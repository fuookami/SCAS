using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonArchivedEventData
    {
        public string ID { get; }

        internal PersonArchivedEventData(Person person)
        {
            ID = person.ID;
        }
    }

    public class PersonArchivedEvent
        : UserContextArtificialEventBase<PersonArchivedEventData>
    {
        [NotNull] private Person person;

        public override string Message => string.Format("Person {0} archived.", person.Info.Name);

        internal PersonArchivedEvent(Person op, Person targetPerson, IExtractor extractor = null)
            : base(op, UserContextEvent.PersonArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonArchivedEventData(targetPerson), extractor)
        {
            person = targetPerson;
        }
    }
}

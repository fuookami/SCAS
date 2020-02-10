using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonCreatedEventData
    {
        public string ID { get; }
    }

    public class PersonCreatedEvent
        : DomainEventBase<DomainEventValue, PersonCreatedEventData>
    {
    }
}

using SCAS.Module;

namespace SCAS.Domain.UserContext
{
    public class RegionCreatedEventValue
        : DomainEventValueBase
    {

    }

    public class RegionCreatedEvent
        : DomainEventBase<RegionCreatedEventValue>
    {
    }
}

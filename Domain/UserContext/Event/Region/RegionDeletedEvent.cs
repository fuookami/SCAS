using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct RegionDeletedEventData
    {
        public string ID { get; }

        internal RegionDeletedEventData(Region region)
        {
            ID = region.ID;
        }
    }

    public class RegionDeletedEvent
        : UserContextArtificialEventBase<DomainEventValue, RegionDeletedEventData>
    {
        [NotNull] private Region region;

        public override string Message => string.Format("Region {0} was deleted.", region.Info.Name);

        internal RegionDeletedEvent(Person op, Region targetRegion, IExtractor extractor = null)
            : base(op, UserContextEvent.RegionDeleted, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new RegionDeletedEventData(targetRegion), extractor)
        {
            region = targetRegion;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

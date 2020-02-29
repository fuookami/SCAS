using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct RegionArchivedEventData
    {
        public string ID { get; }

        internal RegionArchivedEventData(Region region)
        {
            ID = region.ID;
        }
    }

    public class RegionArchivedEvent
        : UserContextArtificialEventBase<DomainEventValue, RegionArchivedEventData>
    {
        [NotNull] private Region region;

        public override string Message => string.Format("Region {0} was archived.", region.Info.Name);

        internal RegionArchivedEvent(Person op, Region targetRegion, IExtractor extractor = null)
            : base(op, UserContextEvent.RegionArchived, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new RegionArchivedEventData(targetRegion), extractor)
        {
            region = targetRegion;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

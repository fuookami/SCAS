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
        : DomainEventBase<DomainEventValue, RegionArchivedEventData>
    {
        [NotNull] private Region region;

        public override string Message => string.Format("Region {0} archived.", region.Info.Name);

        internal RegionArchivedEvent(Region targetRegion, IExtractor extractor = null)
            : base((uint)SCASEvent.RegionArchived, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new RegionArchivedEventData(targetRegion), extractor)
        {
            region = targetRegion;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

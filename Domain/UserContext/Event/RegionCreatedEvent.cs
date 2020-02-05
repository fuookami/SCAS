
using SCAS.Module;
using System.Text;

namespace SCAS.Domain.UserContext
{
    public struct RegionCreatedEventData
    {
        public string ID { get; set; }
        public RegionType Type { get; set; }
        public string Name { get; set; }
        public string ParentID { get; set; }

        public RegionCreatedEventData(Region region)
        {
            ID = region.ID;
            Type = region.Type;
            Name = region.Info.Name;
            ParentID = region.ParentRegion?.ID;
        }
    }

    public class RegionCreatedEvent
        : DomainEventBase<DomainEventValue, RegionCreatedEventData>
    {
        private Region region;

        public override string Message { get { return GetMessage(); } }

        internal RegionCreatedEvent(Region newRegion, IExtractor extractor = null)
            : base((uint)SCASEvent.RegionCreated, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new RegionCreatedEventData(newRegion), extractor)
        {
            region = newRegion;
        }

        private string GetMessage()
        {
            var ret = string.Format("{0} region {1} created", region.Type.ToString(), region.Info.Name);
            if (!region.BeRoot)
            {
                ret += string.Format(", is subject to {0}.", region.ParentRegion.Info.Name);
            }
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

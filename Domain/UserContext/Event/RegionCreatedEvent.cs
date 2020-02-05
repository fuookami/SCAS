using Newtonsoft.Json;
using SCAS.Module;
using System.Text;

namespace SCAS.Domain.UserContext
{
    public struct RegionCreatedEventData
    {
        public RegionType Type { get; set; }
        public string Name { get; set; }
        public string ParentID { get; set; }
    }

    public class RegionCreatedEvent
        : DomainEventBase<DomainEventValue>
    {
        private Region parentRegion;

        public RegionCreatedEventData DataObj { get; }

        public override string Message { get { return GetMessage(); } }

        internal RegionCreatedEvent(RegionType type, string name, Region parent = null, IExtractor extractor = null)
            : base((uint)SCASEvent.RegionCreated, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common)
        {
            parentRegion = parent;
            DataObj = new RegionCreatedEventData {
                Type = type,
                Name = name,
                ParentID = parent?.ID
            };
            Data = JsonConvert.SerializeObject(DataObj);
            Digest = Encoding.UTF8.GetString(extractor.Extract(Encoding.UTF8.GetBytes(Data)));
        }

        private string GetMessage()
        {
            var ret = string.Format("{0} region {1} created", DataObj.Type.ToString(), DataObj.Name);
            if (parentRegion == null)
            {
                ret += string.Format(", is subject to {0}.", parentRegion.Info.Name);
            }
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

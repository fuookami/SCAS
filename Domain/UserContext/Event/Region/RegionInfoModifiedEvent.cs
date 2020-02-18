using SCAS.Module;
using SCAS.Utils;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct RegionInfoChangedEventData
    {
        public string ID { get; }
        public Modification<string> Name { get; }
        public Modification<string> Description { get; }
        public Modification<IReadOnlyCollection<string>> Tags { get; }

        internal RegionInfoChangedEventData(RegionInfo info, string name = null, string description = null, IReadOnlyCollection<string> tags = null)
        {
            ID = info.ID;
            Name = Modification<string>.Make(info.Name, name);
            Description = Modification<string>.Make(info.Description, description);
            Tags = Modification<IReadOnlyCollection<string>>.Make(info.Tags, tags);
        }
    }

    public class RegionInfoModifiedEvent
        : DomainEventBase<DomainEventValue, RegionInfoChangedEventData>
    {
        [NotNull] private RegionInfo info;

        public override string Message => GetMessage();

        internal RegionInfoModifiedEvent(RegionInfo targetInfo, string name = null, string description = null, IReadOnlyCollection<string> tags = null, IExtractor extractor = null)
            : base((uint)SCASEvent.RegionInfoModified, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new RegionInfoChangedEventData(targetInfo, name, description, tags), extractor)
        {
            info = targetInfo;
        }

        private string GetMessage()
        {
            var ret = string.Format("Info of region {0} changed", info.Name);
            if (DataObj.Name != null)
            {
                ret += string.Format(", now is {0}", DataObj.Name.NewValue);
            }
            ret += ".";
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

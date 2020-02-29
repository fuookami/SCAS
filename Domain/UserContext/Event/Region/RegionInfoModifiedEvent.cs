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
        : UserContextArtificialEventBase<DomainEventValue, RegionInfoChangedEventData>
    {
        [NotNull] private RegionInfo info;

        public override string Message => GetMessage();

        internal RegionInfoModifiedEvent(Person op, RegionInfo targetInfo, string name = null, string description = null, IReadOnlyCollection<string> tags = null, IExtractor extractor = null)
            : base(op, UserContextEvent.RegionInfoModified, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new RegionInfoChangedEventData(targetInfo, name, description, tags), extractor)
        {
            info = targetInfo;
        }

        private string GetMessage()
        {
            var ret = string.Format("Info of region {0} was modified", info.Name);
            if (DataObj.Name != null)
            {
                ret += string.Format(". It is {0} now", DataObj.Name.NewValue);
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

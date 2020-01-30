using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class RegionInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("RegionInfo-{0}", ID);
        }
    }

    public struct RegionInfoValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string RegionID { get; internal set; }

        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }

    public class RegionInfo
        : DomainAggregateChild<RegionInfoValue, RegionInfoID, RegionID>
    {
        public string RegionID { get { return pid.ID; } }

        public string Name { get; internal set; }
        public string Description { get; internal set; }

        internal RegionInfo(RegionID pid, string name)
            : base(pid)
        {
            Name = name;
        }

        internal RegionInfo(RegionID pid, RegionInfoID id, string name, string description = null)
            : base(pid, id)
        {
            Name = name;
            Description = description;
        }

        public override RegionInfoValue ToValue()
        {
            return new RegionInfoValue
            {
                ID = this.ID,
                RegionID = this.RegionID,
                Name = this.Name,
                Description = this.Description
            };
        }
    }
}

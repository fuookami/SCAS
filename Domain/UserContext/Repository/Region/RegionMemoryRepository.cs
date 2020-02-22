using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public class RegionMemoryRepository
        : RegionRepository
    {
        private Dictionary<string, RegionValue> regions;
        private Dictionary<string, RegionInfoValue> regionInfos;

        internal RegionMemoryRepository()
        {
            regions = new Dictionary<string, RegionValue>();
            regionInfos = new Dictionary<string, RegionInfoValue>();
        }

        public override Region Get(RegionID id)
        {
            var info = GetInfo(id);
            var value = regions[id.ID];
            var parentRegion = id == null ? null : Get(new RegionID(value.ParentRegionID));
            return new Region(id, value.Archived, value.Type, info, parentRegion);
        }

        public override Try Add(Region region)
        {
            if (regions.ContainsKey(region.ID))
            {
                //todo: return error
            }
            var ret = Add(region.Info);
            if (!ret.Succeed)
            {
                return ret;
            }
            regions.Add(region.ID, region.ToValue());
            return new Try();
        }

        public override Try Save(Region region)
        {
            if (!regions.ContainsKey(region.ID))
            {
                //todo: return error
            }
            if (regions[region.ID].Archived)
            {
                //todo: return error
            }
            var ret = Save(region.Info);
            if (!ret.Succeed)
            {
                return ret;
            }
            regions[region.ID] = region.ToValue();
            return new Try();
        }

        protected override RegionInfo GetInfo(RegionID id)
        {
            var value = regionInfos[id.ID];
            return new RegionInfo(id, new RegionInfoID(value.ID), value.Name, value.Description);
        }

        protected override Try Add(RegionInfo info)
        {
            if (regionInfos.ContainsKey(info.RegionID))
            {
                //todo: return error
            }
            regionInfos.Add(info.RegionID, info.ToValue());
            return new Try();
        }

        protected override Try Save(RegionInfo info)
        {
            if (!regionInfos.ContainsKey(info.RegionID))
            {
                //todo: return error
            }
            regionInfos[info.RegionID] = info.ToValue();
            return new Try();
        }
    }
}

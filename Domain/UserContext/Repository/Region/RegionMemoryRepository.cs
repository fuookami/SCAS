using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public class RegionMemoryRepository
        : IRegionRepository
    {
        private Dictionary<string, RegionValue> regions;
        private Dictionary<string, RegionInfoValue> regionInfos;

        internal RegionMemoryRepository()
        {
            regions = new Dictionary<string, RegionValue>();
            regionInfos = new Dictionary<string, RegionInfoValue>();
        }

        public Region Get(RegionID id)
        {
            var info = GetInfo(id);
            var value = regions[id.ID];
            var parentRegion = id == null ? null : Get(new RegionID(value.ParentRegionID));
            return new Region(id, value.Archived, value.Type, info, parentRegion);
        }

        public Try Add(Region region)
        {
            var ret = Add(region.Info);
            if (!ret.Succeed)
            {
                return ret;
            }
            if (regions.ContainsKey(region.ID))
            {
                //todo: return error
            }
            regions.Add(region.ID, region.ToValue());
            return new Try();
        }

        public Try Save(Region region)
        {
            var ret = Save(region.Info);
            if (!ret.Succeed)
            {
                return ret;
            }
            if (!regions.ContainsKey(region.ID))
            {
                //todo: return error
            }
            regions[region.ID] = region.ToValue();
            return new Try();
        }

        public RegionInfo GetInfo(RegionID id)
        {
            var value = regionInfos[id.ID];
            return new RegionInfo(id, new RegionInfoID(value.ID), value.Name, value.Description);
        }

        public Try Add(RegionInfo info)
        {
            if (regionInfos.ContainsKey(info.RegionID))
            {
                //todo: return error
            }
            regionInfos.Add(info.RegionID, info.ToValue());
            return new Try();
        }

        public Try Save(RegionInfo info)
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

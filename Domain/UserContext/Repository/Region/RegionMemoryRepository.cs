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

        public Try Save(Region region)
        {
            var ret = SaveInfo(region.Info);
            if (!ret.Succeed)
            {
                return ret;
            }
            if (!regions.ContainsKey(region.ID))
            {
                regions.Add(region.ID, region.ToValue());
            }
            else
            {
                regions[region.ID] = region.ToValue();
            }
            return new Try();
        }

        public RegionInfo GetInfo(RegionID id)
        {
            var value = regionInfos[id.ID];
            return new RegionInfo(id, new RegionInfoID(value.ID), value.Name, value.Description);
        }

        public Try SaveInfo(RegionInfo info)
        {
            if (!regionInfos.ContainsKey(info.RegionID))
            {
                regionInfos.Add(info.RegionID, info.ToValue());
            }
            else
            {
                regionInfos[info.RegionID] = info.ToValue();
            }
            return new Try();
        }
    }
}

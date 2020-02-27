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

        public override TryEx<Region> Get(RegionID id)
        {
            if (!regions.ContainsKey(id.ID))
            {
                //todo: return error
            }
            var value = regions[id.ID];
            if (value.Deleted)
            {
                //todo: return error
            }
            var info = GetInfo(id);
            if (!info.Succeeded)
            {
                //todo: return error
            }
            var parentRegion = value.ParentRegionID == null ? null : Get(new RegionID(value.ParentRegionID));
            if (parentRegion != null && !parentRegion.Succeeded)
            {
                //todo: return error
            }
            return new TryEx<Region>(new Region(id, value.Archived, value.Type, info.Value, parentRegion?.Value));
        }

        public override Try Add(Region region)
        {
            if (regions.ContainsKey(region.ID))
            {
                //todo: return error
            }
            var ret = Add(region.Info);
            if (!ret.Succeeded)
            {
                return ret;
            }
            regions.Add(region.ID, region.ToValue());
            return Try.Success;
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
            if (!ret.Succeeded)
            {
                return ret;
            }
            regions[region.ID] = region.ToValue();
            return Try.Success;
        }

        protected override TryEx<RegionInfo> GetInfo(RegionID id)
        {
            if (!regionInfos.ContainsKey(id.ID))
            {
                //todo: return error
            }
            var value = regionInfos[id.ID];
            if (value.Deleted)
            {
                //todo: return error
            }
            return new TryEx<RegionInfo>(new RegionInfo(id, new RegionInfoID(value.ID), value.Name, value.Description));
        }

        protected override Try Add(RegionInfo info)
        {
            if (regionInfos.ContainsKey(info.RegionID))
            {
                //todo: return error
            }
            regionInfos.Add(info.RegionID, info.ToValue());
            return Try.Success;
        }

        protected override Try Save(RegionInfo info)
        {
            if (!regionInfos.ContainsKey(info.RegionID))
            {
                //todo: return error
            }
            regionInfos[info.RegionID] = info.ToValue();
            return Try.Success;
        }
    }
}

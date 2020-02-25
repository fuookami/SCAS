using SCAS.Module;
using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public interface IRegionRepository
        : IAggregateRepository<Region, RegionID>
    {
    }

    public abstract class RegionRepository
        : IRegionRepository
    {
        public abstract TryEx<Region> Get(RegionID id);
        public abstract Try Add(Region region);
        public abstract Try Save(Region region);

        protected abstract TryEx<RegionInfo> GetInfo(RegionID id);
        protected abstract Try Add(RegionInfo info);
        protected abstract Try Save(RegionInfo info);
    }
}

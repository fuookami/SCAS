using SCAS.Module;
using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public abstract class RegionRepository
        : IAggregateRepository<Region, RegionID>
    {
        public abstract Region Get(RegionID id);
        public abstract Try Add(Region region);
        public abstract Try Save(Region region);

        protected abstract RegionInfo GetInfo(RegionID id);
        protected abstract Try Add(RegionInfo info);
        protected abstract Try Save(RegionInfo info);
    }
}

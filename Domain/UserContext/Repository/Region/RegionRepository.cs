using SCAS.Module;
using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public interface IRegionRepository
        : IAggregateRepository<Region, RegionID>
    {
        public RegionInfo GetInfo(RegionID id);
        public Try Add(RegionInfo info);
        public Try Save(RegionInfo info);
    }
}

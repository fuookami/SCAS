using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class UserMemoryRepository
        : IUserRepository
    {
        private RegionMemoryRepository regionImpl;

        internal UserMemoryRepository()
        {
            regionImpl = new RegionMemoryRepository();
        }

        public TryEx<Region> Get(RegionID id)
        {
            return regionImpl.Get(id);
        }

        public Try Add(Region region)
        {
            return regionImpl.Add(region);
        }

        public Try Save(Region region)
        {
            return regionImpl.Save(region);
        }
    }
}

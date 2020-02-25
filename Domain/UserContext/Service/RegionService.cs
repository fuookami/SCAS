using SCAS.Module;
using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public class RegionService
    {
        private IUserRepository repository;
        private UserEventHandler handler;
        private IExtractor extractor;

        internal RegionService(IUserRepository targetRepository, UserEventHandler targetHandler, IExtractor targetExtractor = null)
        {
            repository = targetRepository;
            handler = targetHandler;
            extractor = targetExtractor;
        }

        public Try CreateRegion(Region newRegion)
        {
            return repository.Add(newRegion)
                && handler.Push(new RegionCreatedEvent(newRegion, extractor));
        }

        public Try ArchiveRegion(Region targetRegion)
        {
            return repository.Save(targetRegion.Archive())
                && handler.Push(new RegionArchivedEvent(targetRegion, extractor));
        }

        public Try RegionInfoModified(Region targetRegion, string name = null, string description = null, IReadOnlyCollection<string> tags = null)
        {
            var info = targetRegion.Info;
            var snapshoot = info.Snapshoot();
            info.Refresh(name, description, tags);
            return repository.Save(targetRegion)
                && handler.Push(new RegionInfoModifiedEvent(snapshoot, name, description, tags, extractor));
        }
    }
}

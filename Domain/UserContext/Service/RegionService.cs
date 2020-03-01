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

        public TryEx<Region> GetRegion(string id)
        {
            return repository.Get(new RegionID(id));
        }

        public Try CreateRegion(Person op, Region newRegion)
        {
            return repository.Add(newRegion)
                && handler.Push(new RegionCreatedEvent(op, newRegion, extractor));
        }

        public Try ArchiveRegion(Person op, Region targetRegion)
        {
            if (!targetRegion.Archive())
            {
                //todo: return error
            }
            return repository.Save(targetRegion)
                && handler.Push(new RegionArchivedEvent(op, targetRegion, extractor));
        }

        public Try DeleteRegion(Person op, Region targetRegion)
        {
            if (!targetRegion.Delete())
            {
                //todo: return error
            }
            return repository.Save(targetRegion)
                && handler.Push(new RegionDeletedEvent(op, targetRegion, extractor));
        }

        public Try RegionInfoModified(Person op, Region targetRegion, string name = null, string description = null, IReadOnlyCollection<string> tags = null)
        {
            var info = targetRegion.Info;
            var snapshoot = info.Snapshoot();
            info.Refresh(name, description, tags);
            return repository.Save(targetRegion)
                && handler.Push(new RegionInfoModifiedEvent(op, snapshoot, name, description, tags, extractor));
        }
    }
}

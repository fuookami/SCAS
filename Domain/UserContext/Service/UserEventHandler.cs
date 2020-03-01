using SCAS.Domain.UserContext;
using SCAS.Module;
using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class UserEventHandler
    {
        private IEventRepository<IUserContextEvent> repository;

        internal UserEventHandler(IEventRepository<IUserContextEvent> targetRepository = null)
        {
            repository = targetRepository;
        }

        public Try Push(RegionCreatedEvent e)
        {
            return Save(e);
        }

        public delegate Try RegionArchivedEventHandler(RegionArchivedEvent e);
        public event RegionArchivedEventHandler RegionArchived;
        public Try Push(RegionArchivedEvent e)
        {
            return Save(e) && RegionArchived(e);
        }

        public Try Push(RegionInfoModifiedEvent e)
        {
            return Save(e);
        }

        public delegate Try RegionDeletedEventHandler(RegionDeletedEvent e);
        public event RegionDeletedEventHandler RegionDeleted;
        public Try Push(RegionDeletedEvent e)
        {
            return Save(e) && RegionDeleted(e);
        }

        private Try Save(IUserContextEvent e)
        {
            if (repository == null)
            {
                return Try.Success;
            }
            return repository.Save(e);
        }
    }
}

using SCAS.Domain.UserContext;
using SCAS.Module;
using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class UserEventHandler
    {
        private IEventRepository repository;

        internal UserEventHandler(IEventRepository targetRepository = null)
        {
            repository = targetRepository;
        }

        public Try Push(RegionCreatedEvent e)
        {
            return Save(e);
        }

        public Try Push(RegionArchivedEvent e)
        {
            var ret = Save(e);
            if (!ret)
            {
                return ret;
            }
            // todo: raise organization register archived and person register archived
        }

        public Try Push(RegionInfoModifiedEvent e)
        {
            return Save(e);
        }

        private Try Save(IDomainEvent e)
        {
            if (repository == null)
            {
                return Try.Success;
            }
            return repository.Save(e);
        }
    }
}

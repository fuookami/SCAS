using SCAS.Module;
using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public class UserContextEventMemoryRepository
        : IEventRepository<IUserContextEvent>
    {
        private Dictionary<string, DomainEventValue> events;

        public Try Save(IUserContextEvent e)
        {
            if (events.ContainsKey(e.Digest))
            {
                //todo: return error
            }
            events.Add(e.Digest, e.ToValue());
            return Try.Success;
        }
    }
}

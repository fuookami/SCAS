using SCAS.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCAS.Module
{
    public interface IEventRepository
    {
        public Try Save(IDomainEvent e);
    }
}

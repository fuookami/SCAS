using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public abstract class RegisterFormInfoValueBase
        : FormInfoValueBase
    {
    }

    public interface IRegisterFormInfo
        : IFormInfo<Person>
    {
    }

    public abstract class RegisterFormInfoBase<T, U, P>
        : FormInfoBase<T, U, P, Person>, IRegisterFormInfo
        where T : RegisterFormInfoValueBase
        where U : DomainEntityID, new()
        where P : DomainEntityID
    {
        internal RegisterFormInfoBase(P pid, Person initiator)
            : base(pid, initiator)
        {
        }

        internal RegisterFormInfoBase(P pid, U id, Person initiator, DateTime initialTime)
            : base(pid, id, initiator, initialTime)
        {
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            return value;
        }
    }
}

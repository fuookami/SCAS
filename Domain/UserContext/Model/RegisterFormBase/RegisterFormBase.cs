using SCAS.Module;

namespace SCAS.Domain.UserContext
{
    public abstract class RegisterFormValueBase
        : FormValueBase
    {
    }

    public abstract class RegisterFormBase<T, U, R, I, E>
        : FormBase<T, U, R, I, Person, E, Person>
        where T : RegisterFormValueBase
        where U : DomainEntityID, new()
        where R : IDomainEntity
        where I : IRegisterFormInfo
        where E : IRegisterFormExamination
    {
        protected RegisterFormBase(string sid)
            : base(sid)
        {
        }

        protected RegisterFormBase(U id, string sid, I info, E examination)
            : base(id, sid, info, examination)
        {
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            return value;
        }
    }
}

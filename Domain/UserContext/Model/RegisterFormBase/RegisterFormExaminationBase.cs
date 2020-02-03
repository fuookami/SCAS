using SCAS.Module;
using System;

namespace SCAS.Domain.UserContext
{
    public abstract class RegisterFormExaminiationValueBase
        : FormExaminiationValueBase
    {
    }

    public interface IRegisterFormExamination
        : IFormExamination<Person>
    {
    }

    public abstract class RegisterFormExaminationBase<T, U, P>
        : FormExaminationBase<T, U, P, Person>, IRegisterFormExamination
        where T : RegisterFormExaminiationValueBase
        where U : DomainEntityID, new()
        where P : DomainEntityID
    {
        protected RegisterFormExaminationBase(P pid, Person examiner, bool approved, string annotation)
            : base(pid, examiner, approved, annotation)
        {
        }

        protected RegisterFormExaminationBase(P pid, U id, Person examiner, DateTime examinationTime, bool approved, string annotation)
            : base(pid, id, examiner, examinationTime, approved, annotation)
        {
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            return value;
        }
    }
}

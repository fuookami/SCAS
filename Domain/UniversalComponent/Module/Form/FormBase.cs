using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public abstract class FormValueBase
        : DomainAggregateRootValueBase
    {
        public string SID { get; internal set; }
    }

    public interface IForm<R, I, II, E, EE>
        where R : IDomainEntity
        where I : IFormInfo<II>
        where II : IDomainEntity
        where E : IFormExamination<EE>
        where EE : IDomainEntity
    {
        // 应用识别码，由系统使用者给定规则（流水号）
        [DisallowNull] public string SID { get; }

        // 审批表单信息
        [DisallowNull] public I Info { get; }
        // 审批结果
        [DisallowNull] public E Examination { get; }

        // 是否已经审批
        public bool Examined { get; }
        // 审批是否通过
        public bool Approved { get; }

        public R Approve(string sid, EE examiner, string annotation);
        public void Unapprove(EE examiner, string annotation);
    }

    public abstract class FormBase<T, U, R, I, II, E, EE>
        : DomainAggregateRoot<T, U>, IForm<R, I, II, E, EE>
        where T : FormValueBase
        where U : DomainEntityID, new()
        where R : IDomainEntity
        where I : IFormInfo<II>
        where II : IDomainEntity
        where E : IFormExamination<EE>
        where EE : IDomainEntity
    {
        [DisallowNull] public string SID { get; }

        // 审批表单信息
        [DisallowNull] public I Info { get; protected set; }
        // 审批结果
        public E Examination { get; protected set; }

        // 是否已经审批
        public bool Examined => Examination != null;
        // 审批是否通过
        public bool Approved => Examination.Approved;

        protected FormBase(string sid)
        {
            SID = sid;
        }

        protected FormBase(U id, string sid, I info, E examination)
            : base(id, examination != null)
        {
            SID = sid;
            Info = info;
            Examination = examination;
        }

        public abstract R Approve(string sid, EE examiner, string annotation);
        public abstract void Unapprove(EE examiner, string annotation);

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.SID = this.SID;
            return value;
        }
    }
}

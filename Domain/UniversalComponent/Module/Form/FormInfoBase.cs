using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public abstract class FormInfoValueBase
        : DomainEntityValueBase
    {
        [NotNull] public string FormID { get; internal set; }
        [NotNull] public string InitiatorID { get; internal set; }
        [NotNull] public DateTime InitialTime { get; internal set; }
    }

    public interface IFormInfo<I>
        where I : IDomainEntity
    {
        // 审批表单系统识别码
        [NotNull] public string FormID { get; }

        // 发起者
        [DisallowNull] public I Initiator { get; }
        // 发起时间
        public DateTime InitialTime { get; }
    }

    public abstract class FormInfoBase<T, U, P, I>
        : DomainAggregateChild<T, U, P>, IFormInfo<I>
        where T : FormInfoValueBase
        where U : DomainEntityID, new()
        where P : DomainEntityID
        where I : IDomainEntity
    {
        // 审批表单系统识别码
        [NotNull] public string FormID => pid.ID;

        // 发起者
        [DisallowNull] public I Initiator { get; }
        // 发起时间
        public DateTime InitialTime { get; }

        protected FormInfoBase(P pid, I initiator)
            : base(pid)
        {
            Initiator = initiator;
            InitialTime = DateTime.Now;
        }

        protected FormInfoBase(P pid, U id, I initiator, DateTime initialTime)
            : base(pid, id)
        {
            Initiator = initiator;
            InitialTime = initialTime;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.FormID = this.FormID;
            value.InitiatorID = this.Initiator.ID;
            value.InitialTime = this.InitialTime;
            return value;
        }
    }
}

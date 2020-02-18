using System;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Module
{
    public abstract class FormExaminiationValueBase
        : DomainEntityValueBase
    {
        [NotNull] public string FormID { get; internal set; }

        [NotNull] public string ExaminerID { get; internal set; }
        public DateTime ExaminationTime { get; internal set; }

        public bool Approved { get; internal set; }
        public string Annotation { get; internal set; }
    }

    public interface IFormExamination<E>
        where E : IDomainEntity
    {
        // 审批表单系统识别码
        [NotNull] public string FormID { get; }

        // 审批人
        [DisallowNull] public E Examiner { get; }
        // 审批时间
        public DateTime ExaminationTime { get; }

        // 是否通过
        public bool Approved { get; }
        // 审批意见
        public string Annotation { get; }
    }

    public abstract class FormExaminationBase<T, U, P, E>
        : DomainAggregateChild<T, U, P>, IFormExamination<E>
        where T : FormExaminiationValueBase
        where U : DomainEntityID, new()
        where P : DomainEntityID
        where E : IDomainEntity
    {
        // 审批表单系统识别码
        [NotNull] public string FormID => pid.ID;

        // 审批人
        [DisallowNull] public E Examiner { get; }
        // 审批时间
        [DisallowNull] public DateTime ExaminationTime { get; }

        // 是否通过
        public bool Approved { get; }
        // 审批意见
        public string Annotation { get; }

        protected FormExaminationBase(P pid, E examiner, bool approved, string annotation)
            : base(pid)
        {
            Examiner = examiner;
            ExaminationTime = DateTime.Now;
            Approved = approved;
            Annotation = annotation;
        }

        protected FormExaminationBase(P pid, U id, E examiner, DateTime examinationTime, bool approved, string annotation)
            : base(pid, id)
        {
            Examiner = examiner;
            ExaminationTime = DateTime.Now;
            Approved = approved;
            Annotation = annotation;
        }

        protected new T ToValue(T value)
        {
            base.ToValue(value);
            value.FormID = this.FormID;
            value.ExaminerID = this.Examiner.ID;
            value.ExaminationTime = this.ExaminationTime;
            value.Approved = this.Approved;
            value.Annotation = this.Annotation;
            return value;
        }
    }
}

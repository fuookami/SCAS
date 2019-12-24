using System.Collections.Generic;
using System.Linq;

namespace SCAS.Domain.User
{
    public enum UsageType
    {
        Login, 
        Logout
    }

    public partial class UsageRecord
    {
        internal UsageRecord(User op, UsageType type, string message)
        {
            Operator = op;
            Type = type;
            Message = message;
        }

        // 操作人
        public User Operator { get; }
        // 使用记录类型
        public UsageType Type { get; }
        // 使用记录信息
        public string Message { get; }
    }

    class UsageRecordList
    {
        // 对应账号
        public Account Account { get; internal set; }

        // 记录列表
        internal List<UsageRecord> RecordsList { get; set; }
        public IReadOnlyList<UsageRecord> Records { get { return RecordsList; } }

        public IReadOnlyList<UsageRecord> RecordsOf(User op)
        {
            return RecordsList.Where(r => r.Operator == op).ToList();
        }

        public IReadOnlyList<UsageRecord> RecordsOf(UsageType type)
        {
            return RecordsList.Where(r => r.Type == type).ToList();
        }

        public IReadOnlyList<UsageRecord> RecordsOf(string message)
        {
            return RecordsList.Where(r => r.Message.Contains(message)).ToList();
        }
    }
}

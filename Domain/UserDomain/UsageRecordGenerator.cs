using System;

namespace SCAS.Domain.User
{
    partial class UsageRecord
    {
        public static UsageRecord GenerateLoginRecord(User user, DateTime time)
        {
            return new UsageRecord(user, UsageType.Login, time.ToString());
        }

        public static UsageRecord GenerateLogoutRecord(User user, DateTime time)
        {
            return new UsageRecord(user, UsageType.Logout, time.ToString());
        }
    }
}

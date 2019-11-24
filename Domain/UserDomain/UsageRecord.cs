using System;
using System.Collections.Generic;

namespace SCAS.Domain.User
{
    enum UsageType
    {
        
    }

    class UsageRecord
    {
        UsageType Type;
        string Message;
    }

    class UsageRecordList
    {
        User AccountUser;

        Account UsageAccount;

        DateTime LoginTime;
        DateTime LogoutTime;

        List<UsageRecord> RecordsList;
        IReadOnlyList<UsageRecord> Records;
    }
}

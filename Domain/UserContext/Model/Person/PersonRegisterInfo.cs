using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterInfoValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string RegisterId { get; internal set; }

        public DateTime RegisteredTime { get; internal set; }
    }

    public class PersonRegisterInfo
        : IDomainEntity<PersonRegisterInfoValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 注册表系统识别码
        internal string RegisterId { get; }

        // 注册时间
        public DateTime RegisteredTime { get; }

        public PersonRegisterInfoValue ToValue()
        {
            return new PersonRegisterInfoValue
            {
                Id = this.Id,
                RegisterId = this.RegisterId,
                RegisteredTime = this.RegisteredTime
            };
        }
    }
}

﻿using SCAS.Utils;
using System;

namespace SCAS.Domain.UserContext
{
    public partial class PersonRegisterInfo
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 注册表系统识别码
        public string RegisterId { get; }

        // 注册时间
        public DateTime RegisteredTime { get; }
    }

    public struct PersonRegisterInfoValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string RegisterId { get; internal set; }

        public DateTime RegisteredTime { get; internal set; }
    }

    public partial class PersonRegisterInfo
        : IPersistentType<PersonRegisterInfoValue>
    {
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

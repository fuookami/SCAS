﻿
using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public struct PersonInfoValue
        : IPersistentValue
    {
        public string Id { get; internal set; }
        public string PersonId { get; internal set; }

        public string Name { get; internal set; }

        public IReadOnlyList<string> Titles { get; internal set; }
        public IReadOnlyList<string> TelephoneNumber { get; internal set; }
        public IReadOnlyList<string> EmailAddresses { get; internal set; }
    }

    // 个人信息
    public partial class PersonInfo
        : IDomainEntity<PersonInfoValue>
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 个人系统识别码
        public string PersonId { get; }

        // 姓名
        public string Name { get; internal set; }

        // 称号、头衔
        internal List<string> TitlesList { get; }
        public IReadOnlyList<string> Titles { get { return TitlesList; } }
        // 电话号码
        internal List<string> TelephoneNumbersList { get; }
        public IReadOnlyList<string> TelephoneNumbers { get { return TelephoneNumbersList; } }
        // 邮箱地址
        internal List<string> EmailAddressesList { get; }
        public IReadOnlyList<string> EmailAddresses { get { return EmailAddressesList; } }

        public PersonInfoValue ToValue()
        {
            return new PersonInfoValue
            {
                Id = this.Id,
                Name = this.Name,
                PersonId = this.PersonId,
                Titles = new List<string>(this.Titles),
                TelephoneNumber = new List<string>(this.TelephoneNumbers),
                EmailAddresses = new List<string>(this.EmailAddresses)
            };
        }
    }
}

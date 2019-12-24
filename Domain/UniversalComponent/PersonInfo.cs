
using SCAS.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SCAS.Domain.Basic
{

    // 个人信息
    public partial class PersonInfo
    {
        // 系统识别码，由系统生成
        public string Id { get; }
        // 个人系统识别码
        public string PersonId { get; }

        // 称号、头衔
        internal List<string> TitlesList { get; }
        public IReadOnlyList<string> Titles { get { return TitlesList; } }
        // 电话号码
        internal List<string> TelephoneNumbersList { get; }
        public IReadOnlyList<string> TelephoneNumbers { get { return TelephoneNumbersList; } }
        // 邮箱地址
        internal List<string> EmailAddressesList { get; }
        public IReadOnlyList<string> EmailAddresses { get { return EmailAddressesList; } }

        internal PersonInfo(string personId, string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString("N");
            PersonId = personId;
            TitlesList = new List<string>();
            TelephoneNumbersList = new List<string>();
            EmailAddressesList = new List<string>();
        }
    }

    public struct PersonInfoValue
        : IPersistentValue
    {
        public IReadOnlyList<string> Titles { get; internal set; }
        public IReadOnlyList<string> TelephoneNumber { get; internal set; }
        public IReadOnlyList<string> EmailAddresses { get; internal set; }
    };

    public partial class PersonInfo
        : IPersistentType<PersonInfoValue>
    {
        public PersonInfoValue ToValue()
        {
            return new PersonInfoValue
            {
                Titles = new List<string>(this.Titles),
                TelephoneNumber = new List<string>(this.TelephoneNumbers),
                EmailAddresses = new List<string>(this.EmailAddresses)
            };
        }
    };
};

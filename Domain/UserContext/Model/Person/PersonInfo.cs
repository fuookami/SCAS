﻿
using SCAS.Utils;
using System.Collections.Generic;

namespace SCAS.Domain.UserContext
{
    public class PersonInfoID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonInfo-{0}", ID);
        }
    }

    public struct PersonInfoValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string PersonID { get; internal set; }

        public string Name { get; internal set; }

        public IReadOnlyList<string> Titles { get; internal set; }
        public IReadOnlyList<string> TelephoneNumber { get; internal set; }
        public IReadOnlyList<string> EmailAddresses { get; internal set; }
    }

    // 个人信息
    public class PersonInfo
        : DomainAggregateChild<PersonInfoValue, PersonInfoID, PersonID>
    {
        // 个人系统识别码
        internal string PersonID { get { return pid.ID; } }

        // 姓名
        public string Name { get; }

        // 称号、头衔
        internal List<string> TitlesList { get; }
        public IReadOnlyList<string> Titles { get { return TitlesList; } }
        // 电话号码
        internal List<string> TelephoneNumbersList { get; }
        public IReadOnlyList<string> TelephoneNumbers { get { return TelephoneNumbersList; } }
        // 邮箱地址
        internal List<string> EmailAddressesList { get; }
        public IReadOnlyList<string> EmailAddresses { get { return EmailAddressesList; } }

        internal PersonInfo(PersonID pid, string name)
            : base(pid)
        {
            Name = name;
            TitlesList = new List<string>();
            TelephoneNumbersList = new List<string>();
            EmailAddressesList = new List<string>();
        }

        internal PersonInfo(PersonID pid, PersonInfoID id, string name, List<string> titles, List<string> telephoneNumbers, List<string> emailAddresses)
            : base(pid, id)
        {
            Name = name;
            TitlesList = titles;
            TelephoneNumbersList = telephoneNumbers;
            EmailAddressesList = emailAddresses;
        }

        public override PersonInfoValue ToValue()
        {
            return new PersonInfoValue
            {
                ID = this.ID,
                Name = this.Name,
                PersonID = this.PersonID,
                Titles = new List<string>(this.Titles),
                TelephoneNumber = new List<string>(this.TelephoneNumbers),
                EmailAddresses = new List<string>(this.EmailAddresses)
            };
        }
    }
}

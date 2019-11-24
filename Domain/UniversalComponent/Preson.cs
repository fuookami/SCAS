using System.Collections.Generic;

namespace SCAS.Domain.Basic
{
    public class Person
    {
        // 系统识别码，由系统生成
        public string Id { get; internal set; }

        // 在当前域下的识别码，由系统使用者给定，
        public string Sid { get; internal set; }
        // 姓名
        public string Name { get; internal set; }

        // 所属组织
        internal List<Organization> BelongedOrganizationsList { get; set; }
        public IReadOnlyList<Organization> BelongedOrganizations { get { return BelongedOrganizationsList; } }

        // 称号、头衔
        internal List<string> TitlesList { get; set; }
        public IReadOnlyList<string> Titles { get { return TitlesList; } }
        // 电话号码
        internal List<string> TelephoneNumbersList  { get; set; }
        public IReadOnlyList<string> TelephoneNumber { get { return TelephoneNumbersList; } }
        // 邮箱地址
        internal List<string> EmailAddressList { get; set; }
        public IReadOnlyList<string> EmailAddress { get { return EmailAddressList; } }
    }
};

using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class PersonID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("Person-{0}", ID);
        }
    }

    public struct PersonValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string SID { get; internal set; }
    }

    // 个人
    public class Person
        : DomainAggregateRoot<PersonValue, PersonID>
    {
        // 应用识别码，由系统使用者给定规则
        public string SID { get; internal set; }

        // 个人信息
        public PersonInfo Info { get; internal set; }

        internal Person()
        {
            Info = new PersonInfo(id);
        }

        internal Person(PersonID id, string sid, PersonInfo info)
            : base(id)
        {
            SID = sid;
            Info = info;
        }

        public override PersonValue ToValue()
        {
            return new PersonValue
            {
                ID = this.ID,
                SID = this.SID
            };
        }
    }
}

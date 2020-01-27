using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class PersonRegisterFormID
        : DomainEntityID
    {
        public override string ToString()
        {
            return string.Format("PersonRegisterForm-{0}", ID);
        }
    }

    public struct PersonRegisterFormValue
        : IPersistentValue
    {
        public string ID { get; internal set; }
        public string SID { get; internal set; }
        public string PersonID { get; internal set; }
        public string OrgID { get; internal set; }
    }

    // 个人注册表审批表单
    public class PersonRegisterForm
        : DomainAggregateRoot<PersonRegisterFormValue, PersonRegisterFormID>
    {
        // 应用识别码，由系统使用者给定规则（流水号）
        public string SID { get; internal set; }

        public Person Person { get; internal set; }
        public Organization Org { get; internal set; }

        public override PersonRegisterFormValue ToValue()
        {
            return new PersonRegisterFormValue
            {
                ID = this.ID, 
                SID = this.SID, 
                PersonID = this.Person.ID, 
                OrgID = this.Org.SID
            };
        }

        public PersonRegister Pass(string sid)
        {
            return new PersonRegister(sid, Person, Org, this);
        }
    }
}

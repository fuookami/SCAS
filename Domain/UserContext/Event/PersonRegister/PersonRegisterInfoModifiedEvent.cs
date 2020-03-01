using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisterInfoModifiedEventData
    {
        public string ID { get; }

        public PersonRegisterInfoModifiedEventData(PersonRegisterInfo info)
        {
            ID = info.ID;
        }
    }

    public class PersonRegisterInfoModifiedEvent
        : UserContextArtificialEventBase<PersonRegisterInfoModifiedEventData>
    {
        [NotNull] public PersonRegister register;

        public override string Message => GetMessage();

        internal PersonRegisterInfoModifiedEvent(Person op, PersonRegister targetRegister, IExtractor extractor = null)
            : base(op, UserContextEvent.PersonRegisterInfoModified, SCASEventType.Model, SCASEventLevel.Common, SCASEventPriority.Common, new PersonRegisterInfoModifiedEventData(targetRegister.Info), extractor)
        {
            register = targetRegister;
        }

        private string GetMessage()
        {
            var ret = string.Format("Register info of person {0} to region {1} modified", register.Person.Info.Name, register.RegisteredRegion.Info.Name);
            if (register.BelongingOrganization != null)
            {
                ret += string.Format(", belonging organization {0}", register.BelongingOrganization.Info.Name);
            }
            ret += ".";
            return ret;
        }
    }
}

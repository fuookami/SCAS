using SCAS.Module;
using SCAS.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationRegisterInfoModifiedEventData
    {
        public string ID { get; }
        public Modification<uint?> PrefixCode { get; }

        public OrganizationRegisterInfoModifiedEventData(OrganizationRegisterInfo info, uint? prefixCode = null)
        {
            ID = info.ID;
            PrefixCode = Modification<uint?>.Make(info.PrefixCode, prefixCode);
        }
    }

    public class OrganizationRegisterInfoModifiedEvent
        : DomainEventBase<DomainEventValue, OrganizationRegisterInfoModifiedEventData>
    {
        [NotNull] public OrganizationRegister register;

        public override string Message => GetMessage();

        internal OrganizationRegisterInfoModifiedEvent(OrganizationRegister targetRegister, uint? prefixCode, IExtractor extractor = null)
            : base((uint)SCASEvent.OrganizationRegisterInfoModified, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationRegisterInfoModifiedEventData(targetRegister.Info), extractor)
        {
            register = targetRegister;
        }

        private string GetMessage()
        {
            var ret = string.Format("Register info of organization {0} to region {1} ({2}) changed", register.Org.Info.Name, register.RegisteredRegion.Info.Name, register.Info.PrefixCode);
            if (DataObj.PrefixCode != null)
            {
                ret += string.Format(", now prefix code is {0}", DataObj.PrefixCode.NewValue);
            }
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}

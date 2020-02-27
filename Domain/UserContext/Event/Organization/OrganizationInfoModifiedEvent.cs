﻿using SCAS.Module;
using SCAS.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct OrganizationInfoModifiedEventData
    {
        public string ID { get; }
        public Modification<string> Name { get; }
        public Modification<string> Description { get; }

        internal OrganizationInfoModifiedEventData(OrganizationInfo info, string name = null, string description = null)
        {
            ID = info.ID;
            Name = Modification<string>.Make(info.Name, name);
            Description = Modification<string>.Make(info.Description, description);
        }
    }

    public class OrganizationInfoModifiedEvent
        : DomainArtificialEventBase<DomainEventValue, OrganizationInfoModifiedEventData, Person>
    {
        [NotNull] private OrganizationInfo info;

        public override string Message => GetMessage();

        internal OrganizationInfoModifiedEvent(Person op, OrganizationInfo targetInfo, string name = null, string description = null, IExtractor extractor = null)
            : base(op, (uint)SCASEvent.OrganizationInfoModified, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationInfoModifiedEventData(targetInfo, name, description), extractor)
        {
            info = targetInfo;
        }

        private string GetMessage()
        {
            var ret = string.Format("Info of organization {0} modified", info.Name);
            if (DataObj.Name != null)
            {
                ret += string.Format(", now is {0}", DataObj.Name.NewValue);
            }
            ret += ".";
            return ret;
        }

        public override DomainEventValue ToValue()
        {
            return base.ToValue(new DomainEventValue { });
        }
    }
}
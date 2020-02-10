﻿using SCAS.Module;
using SCAS.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    class OrganizationInfoModifiedEventData
    {
        public string ID { get; }
        public Modification<string> Name { get; }
        public Modification<string> Description { get; }

        internal OrganizationInfoModifiedEventData(OrganizationInfo info, string name = null, string description = null)
        {
            ID = info.ID;
            Name = name != null ? new Modification<string>(info.Name, name) : null;
            Description = description != null ? new Modification<string>(info.Description, description) : null;
        }
    }

    class OrganizationInfoModifiedEvent
        : DomainEventBase<DomainEventValue, OrganizationInfoModifiedEventData>
    {
        [NotNull] private OrganizationInfo info;

        public override string Message { get { return GetMessage(); } }

        internal OrganizationInfoModifiedEvent(OrganizationInfo targetInfo, string name, string description, IExtractor extractor = null)
            : base((uint)SCASEvent.OrganizationInfoModified, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new OrganizationInfoModifiedEventData(targetInfo, name, description), extractor)
        {
            info = targetInfo;
        }

        private string GetMessage()
        {
            var ret = string.Format("Info of organization {0} changed", info.Name);
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
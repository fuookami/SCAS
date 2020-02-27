﻿using SCAS.Module;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonRegisteredEventData
    {
        public string ID { get; }
        public string SID { get; }
        public string PersonID { get; }
        public string RegionID { get; }
        public string OrgID { get; }

        internal PersonRegisteredEventData(PersonRegister register)
        {
            ID = register.ID;
            SID = register.SID;
            PersonID = register.Person.ID;
            RegionID = register.RegisteredRegion.ID;
            OrgID = register.BelongingOrganization?.ID;
        }
    }

    public class PersonRegisteredEvent
        : DomainEventBase<DomainEventValue, PersonRegisteredEventData>
    {
        [NotNull] private PersonRegister register;

        public override string Message => GetMessage();

        internal PersonRegisteredEvent(PersonRegisterApprovedEvent trigger, PersonRegister targetRegister, IExtractor extractor = null)
            : base(trigger, (uint)SCASEvent.PersonRegistered, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonRegisteredEventData(targetRegister), extractor)
        {
            register = targetRegister;
        }

        private string GetMessage()
        {
            var ret = string.Format("Person {0} registered to region {1}", register.Person.Info.Name, register.RegisteredRegion.Info.Name);
            if (register.BelongingOrganization != null)
            {
                ret += string.Format(", belonging organization {0}", register.BelongingOrganization.Info.Name);
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
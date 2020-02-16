using SCAS.Module;
using SCAS.Utils;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SCAS.Domain.UserContext
{
    public struct PersonInfoModifiedEventData
    {
        public string ID { get; }
        public Modification<string> Name { get; }
        public Modification<IReadOnlyCollection<string>> Titles { get; }
        public Modification<IReadOnlyCollection<string>> TelephoneNumbers { get; }
        public Modification<IReadOnlyCollection<string>> EmailAddresses { get; }

        internal PersonInfoModifiedEventData(PersonInfo info, string name = null, IReadOnlyCollection<string> titles = null, IReadOnlyCollection<string> telephoneNumbers = null, IReadOnlyCollection<string> emailAddresses = null)
        {
            ID = info.ID;
            Name = Modification<string>.Make(info.Name, name);
            Titles = Modification<IReadOnlyCollection<string>>.Make(info.Titles, titles);
            TelephoneNumbers = Modification<IReadOnlyCollection<string>>.Make(info.TelephoneNumbers, telephoneNumbers);
            EmailAddresses = Modification<IReadOnlyCollection<string>>.Make(info.EmailAddresses, emailAddresses);
        }
    }

    public class PersonInfoModifiedEvent
        : DomainEventBase<DomainEventValue, PersonInfoModifiedEventData>
    {
        [NotNull] private PersonInfo info;

        public override string Message { get { return GetMessage(); } }

        internal PersonInfoModifiedEvent(PersonInfo targetInfo, string name = null, IReadOnlyCollection<string> titles = null, IReadOnlyCollection<string> telephoneNumbers = null, IReadOnlyCollection<string> emailAddresses = null, IExtractor extractor = null)
            : base((uint)SCASEvent.OrganizationInfoModified, (uint)SCASEventType.Model, (uint)SCASEventLevel.Common, (uint)SCASEventPriority.Common, new PersonInfoModifiedEventData(targetInfo, name, titles, telephoneNumbers, emailAddresses), extractor)
        {
            info = targetInfo;
        }

        private string GetMessage()
        {
            var ret = string.Format("Info of person {0} changed", info.Name);
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

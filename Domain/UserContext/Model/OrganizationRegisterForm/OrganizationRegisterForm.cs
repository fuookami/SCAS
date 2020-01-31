using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
    public class OrganizationRegisterFormID
        : DomainEntityID
    {

    }

    public struct OrganizationRegisterFormValue
        : IPersistentValue
    {

    }

    public class OrganizationRegisterForm
        : DomainAggregateRoot<OrganizationRegisterFormValue, OrganizationRegisterFormID>
    {
    }
}

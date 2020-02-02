namespace SCAS.Domain.UserContext
{
    public enum RegionType
    {
        Organization,
        Personality,
        Multiple
    }

    public abstract class RegionTypeTrait
    {
        public static bool OrganizationAllowed(RegionType type)
        {
            return type == RegionType.Organization || type == RegionType.Multiple;
        }

        public static bool IndependentPersonalityAllowed(RegionType type)
        {
            return type == RegionType.Personality || type == RegionType.Multiple;
        }

        public static bool Allow(RegionType type, PersonRegisterForm register)
        {
            if (!IndependentPersonalityAllowed(type) && register.BelongingOrganization == null)
            {
                return false;
            }
            return true;
        }

        public static bool Allow(RegionType type, OrganizationRegisterForm register)
        {
            return OrganizationAllowed(type);
        }
    }
}

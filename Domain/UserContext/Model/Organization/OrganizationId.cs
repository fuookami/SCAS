using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
	public class OrganizationID
		: DomainEntityID
	{ 
		public override string ToString()
        {
			return string.Format("Organization-{0}", ID);
        }
	};
}
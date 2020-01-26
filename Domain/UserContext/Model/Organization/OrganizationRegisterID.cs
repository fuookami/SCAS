using SCAS.Utils;

namespace SCAS.Domain.UserContext
{
	public class OrganizationRegisterID
		: DomainEntityID
	{
		public override string ToString()
		{
			return string.Format("OrganizationRegister-{0}", ID);
		}
	};
}
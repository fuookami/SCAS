#include "XMLUtils.h"

int main(void)
{
	auto roots(XMLUtils::scanXMLFile<StringConvertUtils::StringCodeId::UTF8>("ac_menu.xml"));

	system("pause");
	return 0;
}

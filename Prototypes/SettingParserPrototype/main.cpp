#include "XMLUtils.h"

int main(void)
{
	auto roots(XMLUtils::scanXMLFile<StringConvertUtils::StringCodeId::UTF8>("setting.xml"));

	system("pause");
	return 0;
}

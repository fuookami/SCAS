#include "XMLUtils.h"
#include "test.h"

int main(void)
{
	const auto info(generateTestCompetitionInfo());
	testSaveToXML(info);

	system("pause");
	return 0;
}

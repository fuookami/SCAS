#include "UUIDUtils.h"

int main(void)
{
	auto ret1 = UUIDUtil::generateUUIDV1();
	auto ret2 = UUIDUtil::generateUUIDV4();

	system("pause");
	return 0;
}

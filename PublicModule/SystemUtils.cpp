#include "SystemUtils/_pri_cpu_id.h"

namespace SSUtils
{
	namespace System
	{
		const std::array<byte, CPUIdLength> CPUId = getCPUId();
		const uint32 CPUCoreNumber = getCPUCoreNumber();
	};
};

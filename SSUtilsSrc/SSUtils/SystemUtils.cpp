#include "stdafx.h"
#include "SystemUtils.h"
#include "SystemUtils/_pri_cpu_id.h"

namespace SSUtils
{
	namespace System
	{
		const std::array<byte, CPUIdLength>& CPUId(void)
		{
			static const std::array<byte, CPUIdLength> ret = getCPUId();
			return ret;
		}

		uint32 CPUCoreNumber(void)
		{
			return getCPUCoreNumber();
		}
	};
};

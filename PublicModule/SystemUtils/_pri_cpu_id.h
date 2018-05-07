#pragma once

#include "_pri_system_global.h"
#include "Global.h"
#include <array>

namespace SSUtils
{
	namespace System
	{
		std::array<byte, CPUIdLength> getCPUId(void);
		uint32 getCPUCoreNumber(void);
		void _getCPUId(uint32 CPUInfo[4], uint32  infoType);
		void _getCPUIdEx(uint32 CPUInfo[4], uint32  infoType, uint32 ecxValue);
	}
};

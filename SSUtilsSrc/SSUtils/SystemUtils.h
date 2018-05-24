#pragma once

#include "Global.h"
#include "SystemUtils/_pri_system_global.h"
#include "SystemUtils/EndianTranslator.h"
#include <array>

namespace SSUtils
{
	namespace System
	{
		SSUtils_API_DECLSPEC const std::array<byte, CPUIdLength> &CPUId(void);
		SSUtils_API_DECLSPEC uint32 CPUCoreNumber(void);
	};
};

#pragma once

#include "Global.h"
#include "SystemUtils/_pri_system_global.h"
#include "SystemUtils/EndianTranslator.h"
#include <array>

namespace SSUtils
{
	namespace System
	{
		extern const std::array<byte, CPUIdLength> CPUId;
		extern const uint32 CPUCoreNumber;
	};
};

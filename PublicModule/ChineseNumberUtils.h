#pragma once

#include "Global.h"
#include <string>

namespace SSUtils
{
	namespace ChineseNumber
	{
		std::wstring toChineseNumber(uint32 num);
		uint32 fromChineseNumber(const std::wstring &str);
	};
};

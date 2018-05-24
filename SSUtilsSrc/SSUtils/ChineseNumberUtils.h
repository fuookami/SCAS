#pragma once

#include "Global.h"
#include <string>

namespace SSUtils
{
	namespace ChineseNumber
	{
		SSUtils_API_DECLSPEC std::wstring toChineseNumber(uint32 num);
		SSUtils_API_DECLSPEC uint32 fromChineseNumber(const std::wstring &str);
	};
};

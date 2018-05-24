#pragma once

#include "_pri_string_global.h"

namespace SSUtils
{
	namespace String
	{
		// GBK全角转半角
		std::string _toDBS(const std::string &src);
		// GBK半角转全角
		std::string _toQBS(const std::string &src);
	}
};

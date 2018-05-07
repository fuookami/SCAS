#pragma once

#include "_pri_string_global.h"

namespace SSUtils
{
	namespace String
	{
		// GBKȫ��ת���
		std::string _toDBS(const std::string &src);
		// GBK���תȫ��
		std::string _toQBS(const std::string &src);
	}
};

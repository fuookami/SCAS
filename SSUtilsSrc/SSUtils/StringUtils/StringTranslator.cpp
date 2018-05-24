#include "stdafx.h"
#include "StringTranslator.h"
#include "_pri_string_global.h"

namespace SSUtils
{
	namespace String
	{
		std::string toString(const bool val)
		{
			return val ? True() : False();
		}

		const bool toBoolean(const std::string & src)
		{
			return src == True() ? true : false;
		}
	};
};

#include "StringUtils.h"

namespace StringUtils
{
	const std::string &getSpaceChars(void)
	{
		static std::string ret;
		if (ret.empty())
		{
			for (int i(0); i != INT8_MAX; ++i)
			{
				if (isspace(i))
				{
					ret.push_back(static_cast<char>(i));
				}
			}
		}

		return ret;
	}
};

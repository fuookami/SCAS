#include "stdafx.h"
#include "_pri_string_convert.h"

namespace SSUtils
{
	namespace String
	{
		std::string _toDBS(const std::string & src)
		{
			std::string ret;

			for (auto currIt(src.cbegin()), edIt(src.cend()); currIt != edIt; ++currIt)
			{
				unsigned char thisByte(*currIt);
				unsigned char nextByte(*std::next(currIt));

				if (thisByte == 0xA1 && nextByte == 0xA1) // 全角空格
				{
					ret.push_back(0x20);
					++currIt;
				}
				else if (thisByte == 0xA3 && nextByte >= 0xA1 && nextByte <= 0xFE) // ASCII码其它可显示字符
				{
					ret.push_back(nextByte - 0x80);
					++currIt;
				}
				else
				{
					if (thisByte >= 0x80)
					{
						ret.push_back(thisByte);
						ret.push_back(nextByte);
						++currIt;
					}
					else
					{
						ret.push_back(thisByte);
					}
				}
			}

			return ret;
		}

		std::string _toQBS(const std::string & src)
		{
			std::string ret;

			for (auto currIt(src.cbegin()), edIt(src.cend()); currIt != edIt; ++currIt)
			{
				unsigned char thisByte(*currIt);

				if (thisByte == 0x20) // 半角空格
				{
					ret.insert(ret.end(), 2, 0xA1i8);
				}
				else if (thisByte >= 0x21 && thisByte < 0x80)
				{
					ret.push_back(0xA3i8);
					ret.push_back(thisByte + 0x80);
				}
				else
				{
					if (thisByte >= 0x80)
					{
						ret.push_back(thisByte);
						++currIt;
						ret.push_back(*currIt);
					}
					else
					{
						ret.push_back(thisByte);
					}
				}
			}

			return ret;
		}
	};
};

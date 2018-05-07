#include "ChineseNumberUtils.h"
#include "ChineseNumberUntils/_pri_chinese_number.h"

namespace SSUtils
{
	namespace ChineseNumber
	{
		std::wstring toChineseNumber(uint32 num)
		{
			if (num == 0)
			{
				return std::wstring(1, Digits[0]);
			}
			else
			{
				std::wstring ret;
				uint32 unitPos = 0;
				std::wstring strIns;
				bool needZero = false;

				while (num > 0)
				{
					uint32 section = num % 10000;
					if (needZero)
					{
						ret.insert(ret.begin(), Digits[0]);
					}
					strIns = sectionToChinese(section);
					if (section != 0 && unitPos != 0)
					{
						strIns += Sections[unitPos - 1];
					}
					ret.insert(0, strIns);
					needZero = (section < 1000) && (section > 0);
					num = num / 10000;
					unitPos++;
				}

				return ret;
			}
		}

		uint32 fromChineseNumber(const std::wstring & str)
		{
			uint32 ret = 0;
			uint32 section = 0;
			int32 number = 0;
			std::wstring::size_type pos = 0;


			for (std::wstring::size_type i(0), j(str.size()); i != j; )
			{
				int32 num = chineseToValue(str[i]);
				if (num >= 0)
				{
					number = num;
					if (++i == j)
					{
						section += number;
						ret += section;
						break;
					}
				}
				else
				{
					auto sectionUnit = chineseToUnit(str[i]);
					if (sectionUnit.first)//是节权位说明一个节已经结束
					{
						section = (section + number) * sectionUnit.second;
						ret += section;
						section = 0;
					}
					else
					{
						section += (number * sectionUnit.second);
					}
					number = 0;
					if (++i == j)
					{
						ret += section;
						break;
					}
				}
			}

			return ret;
		}
	};
};

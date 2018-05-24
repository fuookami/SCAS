#include "stdafx.h"
#include "_pri_chinese_number.h"
#include <algorithm>

namespace SSUtils
{
	namespace ChineseNumber
	{
		const std::vector<wchar> Digits =
		{
			L'零', L'一', L'二', L'三', L'四', L'五', L'六', L'七', L'八', L'九'
		};

		const std::vector<wchar> Units =
		{
			L'十', L'百', L'千'
		};

		const std::vector<wchar> Sections = 
		{
			L'万', L'亿'
		};

		const std::vector<ChineseTokenValue> UnitValues =
		{
			ChineseTokenValue(std::make_tuple(L'十', 10, false)),
			ChineseTokenValue(std::make_tuple(L'百', 100, false)),
			ChineseTokenValue(std::make_tuple(L'千', 1000, false)),
			ChineseTokenValue(std::make_tuple(L'万', 10000, true)),
			ChineseTokenValue(std::make_tuple(L'亿', 100000000, true))
		};

		ChineseTokenValue::ChineseTokenValue(const wchar _token)
			: token(_token), value(0), isSection(false)
		{
		}

		ChineseTokenValue::ChineseTokenValue(const std::tuple<wchar, int, bool>& val)
			: token(std::get<0>(val)), value(std::get<1>(val)), isSection(std::get<2>(val))
		{
		}

		std::wstring sectionToChinese(uint32 section)
		{
			std::wstring ret;
			std::wstring strIns;
			uint32 unitPos = 0;
			bool zero = true;

			while (section > 0)
			{
				int v = section % 10;
				if (v == 0)
				{
					if ((section == 0) || !zero)
					{
						zero = true;
						ret.insert(ret.begin(), Digits[v]);
					}
				}
				else
				{
					zero = false;
					if (unitPos != 0)
					{
						ret.insert(ret.begin(), Units[unitPos - 1]);
					}
					ret.insert(ret.begin(), Digits[v]);
				}
				unitPos++;
				section = section / 10;
			}

			return ret;
		}

		uint32 chineseToValue(const wchar ch)
		{
			auto it(std::find(Digits.cbegin(), Digits.cend(), ch));
			return static_cast<uint32>((it != Digits.cend()) ? (it - Digits.cbegin()) : -1);
		}

		std::pair<bool, uint32> chineseToUnit(const wchar ch)
		{
			auto it(std::find_if(UnitValues.cbegin(), UnitValues.cend(), 
				[ch](const ChineseTokenValue &value) 
			{
				return ch == value.token;
			}));
			return (it != UnitValues.cend()) ? std::make_pair(it->isSection, it->value) : std::make_pair(false, 1);
		}
	};
};

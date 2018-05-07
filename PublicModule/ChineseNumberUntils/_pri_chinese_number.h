#pragma once

#include "Global.h"
#include <string>
#include <utility>
#include <tuple>

namespace SSUtils
{
	namespace ChineseNumber
	{
		extern const std::vector<wchar> Digits;
		extern const std::vector<wchar> Units;
		extern const std::vector<wchar> Sections;

		struct ChineseTokenValue;
		extern const std::vector<ChineseTokenValue> UnitValues;

		struct ChineseTokenValue
		{
			ChineseTokenValue(const wchar _token);
			ChineseTokenValue(const std::tuple<wchar, int, bool> &val);
			ChineseTokenValue(const ChineseTokenValue &ano) = default;
			ChineseTokenValue(ChineseTokenValue &&ano) = default;
			ChineseTokenValue &operator=(const ChineseTokenValue &rhs) = default;
			ChineseTokenValue &operator=(ChineseTokenValue &&rhs) = default;
			~ChineseTokenValue(void) = default;

			const wchar token;
			int value;
			bool isSection;
		};

		std::wstring sectionToChinese(uint32 section);

		uint32 chineseToValue(const wchar ch);
		std::pair<bool, uint32> chineseToUnit(const wchar ch);
	}
};

#include "ChineseNumberUtils.h"
#include "StringUtils.h"

namespace ChineseNumberUtils
{
	const std::vector<std::string> SingleDigitChineseNumber =
	{
		"��", "һ", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ"
	};

	std::string getChineseNumberUnderHundred(const unsigned char number)
	{
		if (number < 10)
		{
			return SingleDigitChineseNumber[number];
		}
		else if (number % 10 == 0)
		{
			return SingleDigitChineseNumber[number / 10] + SingleDigitChineseNumber[10];
		}
		else if (number < 20)
		{
			return SingleDigitChineseNumber[10] + SingleDigitChineseNumber[number - 10];
		}
		else if (number < 100)
		{
			return SingleDigitChineseNumber[number / 10] + SingleDigitChineseNumber[10] + SingleDigitChineseNumber[number % 10];
		}
		else
		{
			return StringUtils::EmptyString;
		}
	}
};

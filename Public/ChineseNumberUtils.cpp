#include "ChineseNumberUtils.h"

namespace ChineseNumberUtils
{
	std::string getChineseNumberUnderHundred(const unsigned char number)
	{
		static const std::string empty("");
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
			return empty;
		}
	}
};

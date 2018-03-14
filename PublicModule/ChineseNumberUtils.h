#pragma once

#include <string>
#include <vector>

namespace ChineseNumberUtils
{
	extern const std::vector<std::string> SingleDigitChineseNumber;

	std::string getChineseNumberUnderHundred(const unsigned char number);
};

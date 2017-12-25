#pragma once

#include <string>
#include <vector>

namespace ChineseNumberUtils
{
	static const std::vector<std::string> SingleDigitChineseNumber =
	{
		"零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"
	};

	std::string getChineseNumberUnderHundred(const unsigned char number);
};
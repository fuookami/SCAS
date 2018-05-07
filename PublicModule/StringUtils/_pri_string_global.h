#pragma once

#include "Global.h"
#include <map>

namespace SSUtils
{
	enum class CharType
	{
		UTF8,
		UTF16,
		BIG5,
		GBK,
		GB2312
	};

	namespace String
	{
		extern const std::string EmptyString;

		extern const std::string Empty;
		extern const std::string Null;
		extern const std::string None;

		extern const std::string Right;
		extern const std::string Error;
		extern const std::string Wrong;

		extern const std::string Enabled;
		extern const std::string Disabled;

		extern const std::string True;
		extern const std::string False;

		extern const std::string Infinity;
		extern const std::string NegativeInfinity;
		extern const std::string PositiveInfinity;
		extern const std::string NotANumber;

		extern const std::string SpaceCharacters;

		extern const std::string HexStringPrefix;

		extern const std::map<CharType, std::string> CharTypeCode;

		extern const CharType LocalCharType;
		extern const std::string LocalCharTypeCode;
	};
};

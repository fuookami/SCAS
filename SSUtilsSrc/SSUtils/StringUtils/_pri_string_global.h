#pragma once

#include "..\Global.h"
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
#define String_Declaration(name) SSUtils_API_DECLSPEC const std::string &##name##(void);

		String_Declaration(EmptyString);

		String_Declaration(Empty);
		String_Declaration(Null);
		String_Declaration(None);

		String_Declaration(Right);
		String_Declaration(Error);
		String_Declaration(Wrong);

		String_Declaration(Enabled);
		String_Declaration(Disabled);

		String_Declaration(True);
		String_Declaration(False);

		String_Declaration(Infinity);
		String_Declaration(NegativeInfinity);
		String_Declaration(PositiveInfinity);
		String_Declaration(NotANumber);

		String_Declaration(SpaceCharacters);

		String_Declaration(HexStringPrefix);

		SSUtils_API_DECLSPEC const std::map<CharType, std::string> &CharTypeCode(void);
		SSUtils_API_DECLSPEC const CharType LocalCharType(void);
		
		String_Declaration(LocalCharTypeCode);

#undef String_Declaration
	};
};

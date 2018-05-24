#include "stdafx.h"
#include "_pri_string_global.h"
#include "..\SystemUtils.h"
#include <cctype>

namespace SSUtils
{
	namespace String
	{
#define String_Definition(name, str)\
		const std::string &##name##(void)\
		{\
			static const std::string name = std::string(str);\
			return name;\
		}\

		String_Definition(EmptyString, "");

		String_Definition(Empty, "empty");
		String_Definition(Null, "null");
		String_Definition(None, "none");

		String_Definition(Right, "right");
		String_Definition(Error, "error");
		String_Definition(Wrong, "wrong");

		String_Definition(Enabled, "enabled");
		String_Definition(Disabled, "disabled");

		String_Definition(True, "true");
		String_Definition(False, "false");

		String_Definition(Infinity, "inf");
		String_Definition(NegativeInfinity, "+inf");
		String_Definition(PositiveInfinity, "-inf");
		String_Definition(NotANumber, "nan");

		const std::string &SpaceCharacters(void)
		{
			static const std::string ret = []()
			{
				std::string ret;
				if (ret.empty())
				{
					for (int i(0); i != INT8_MAX; ++i)
					{
						if (isspace(i))
						{
							ret.push_back(static_cast<char>(i));
						}
					}
				}

				return ret;
			}();
			return ret;
		}

		String_Definition(HexStringPrefix, "0x");

		const std::map<CharType, std::string> &CharTypeCode(void)
		{
			static const std::map<CharType, std::string> CharTypeCode =
			{
				std::make_pair(CharType::UTF8, std::string("UTF-8")),
				std::make_pair(CharType::UTF16, std::string("UTF-16")),
				std::make_pair(CharType::BIG5, std::string("BIG5")),
				std::make_pair(CharType::GBK, std::string("GBK")),
				std::make_pair(CharType::GB2312, std::string("GB2312"))
			};

			return CharTypeCode;
		} 

		const CharType LocalCharType(void)
		{
			static const CharType LocalCharType = System::LocalSystemType == OperationSystemType::Windows ? CharType::GB2312 : CharType::UTF8;
			return LocalCharType;
		}

		String_Definition(LocalCharTypeCode, CharTypeCode().find(LocalCharType())->second);

#undef String_Definition
	};
};

#pragma once

#include "StringConverter.h"
#include <string>
#include <sstream>

namespace SSUtils
{
	namespace String
	{
		SSUtils_API_DECLSPEC std::string toString(const bool val);
		SSUtils_API_DECLSPEC const bool toBoolean(const std::string &src);

		struct SSUtils_API_DECLSPEC StringTranslator
		{
			StringTranslator(void) = default;
			StringTranslator(const StringTranslator &ano) = delete;
			StringTranslator(StringTranslator &&ano) = delete;
			StringTranslator &operator=(const StringTranslator &ano) = delete;
			StringTranslator &operator=(StringTranslator &&ano) = delete;
			~StringTranslator(void) = default;

			template<typename T>
			std::string operator()(const T &val) const
			{
				std::ostringstream sout;
				sout << val;
				return sout.str();
			};

			template<>
			std::string operator()(const std::string &val) const
			{
				return val;
			}

			template<>
			std::string operator()(const std::wstring &val) const
			{
				return toString(val);
			}

			template<>
			std::string operator()(const bool &val) const
			{
				return toString(val);
			}
		};
	};
};

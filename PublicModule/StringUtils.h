#pragma once

#include "StringUtils/_pri_string_global.h"
#include "StringUtils/StringConverter.h"
#include "StringUtils/StringRegexChecker.h"
#include "StringUtils/StringTranslator.h"
#include <numeric>

namespace SSUtils
{
	namespace String
	{
		std::string getVersion(const int major, const int sub, const int modify);

		std::string base64Encode(const std::string &str, const char fillCharacter = '=');
		std::string base64Decode(const std::string &str);

		std::vector<std::string> split(const std::string &source, const std::string &tokens, const bool removeSpace = true);
		std::vector<std::wstring> split(const std::wstring &source, const std::wstring &tokens, const bool removeSpace = true);

		template<typename iter, typename translator_t = StringTranslator>
		typename std::enable_if_t<std::is_same_v<typename iter::value_type, std::string>, std::string> join(const iter bgIt, const iter edIt, const std::string &seperator = std::string(""),
			const translator_t &translator = translator_t())
		{
			return bgIt >= edIt ? EmptyString
				: std::accumulate(std::next(bgIt), edIt,
					*bgIt, [&seperator]
					(const std::string &lhs, const typename iter::value_type &rhs) -> std::string
			{
				return lhs + seperator + rhs;
			});
		}

		template<typename iter, typename translator_t = StringTranslator>
		typename std::enable_if_t<!std::is_same_v<typename iter::value_type, std::string>, std::string> join(const iter bgIt, const iter edIt, const std::string &seperator = std::string(""),
			const translator_t &translator = translator_t())
		{
			return bgIt >= edIt ? EmptyString
				: std::accumulate(std::next(bgIt), edIt,
					translator(*bgIt), [&seperator, &translator]
					(const std::string &lhs, const typename iter::value_type &rhs) -> std::string
			{
				return lhs + seperator + translator(rhs);
			});
		}

		template<typename container, typename translator_t = StringTranslator>
		std::string join(const container &cont, const std::string &seperator = std::string(""),
			const translator_t &translator = translator_t())
		{
			return join(cont.cbegin(), cont.cend(), seperator, translator);
		}
	};
};

namespace std
{
	string to_string(const bool value);
	bool stoboolean(const std::string &str);
};

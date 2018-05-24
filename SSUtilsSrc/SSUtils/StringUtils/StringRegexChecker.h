#pragma once

#include "..\Global.h"
#include <string>
#include <regex>

namespace SSUtils
{
	namespace String
	{
		struct SSUtils_API_DECLSPEC RegexChecker
		{
			const std::string pattern;
			const std::regex reg;

			RegexChecker(const std::string &_pattern);
			RegexChecker(std::string &&_pattern);
			RegexChecker(const RegexChecker &ano) = delete;
			RegexChecker(RegexChecker &&ano) = delete;
			RegexChecker &operator=(const RegexChecker &rhs) = delete;
			RegexChecker &operator=(RegexChecker &&rhs) = delete;
			~RegexChecker(void) = default;

			const bool operator()(const std::string &src) const;
		};

		struct SSUtils_API_DECLSPEC RegexCatcher
		{
			const std::string pattern;
			const std::regex reg;

			RegexCatcher(const std::string &_pattern);
			RegexCatcher(std::string &&_pattern);
			RegexCatcher(const RegexCatcher &ano) = delete;
			RegexCatcher(RegexCatcher &&ano) = delete;
			RegexCatcher &operator=(const RegexCatcher &ano) = delete;
			RegexCatcher &operator=(RegexCatcher &&ano) = delete;
			~RegexCatcher(void) = default;

			std::vector<std::string> operator()(const std::string &src) const;
		};

		struct SSUtils_API_DECLSPEC RegexMatcher
		{
			const std::string pattern;
			const std::regex reg;

			RegexMatcher(const std::string &_pattern);
			RegexMatcher(std::string &&_pattern);
			RegexMatcher(const RegexMatcher &ano) = delete;
			RegexMatcher(RegexMatcher &&ano) = delete;
			RegexMatcher &operator=(const RegexMatcher &ano) = delete;
			RegexMatcher &operator=(RegexMatcher &&ano) = delete;
			~RegexMatcher(void) = default;

			std::vector<std::string> operator()(const std::string &src) const;
		};

		namespace RegexPatterns
		{
			extern SSUtils_API_DECLSPEC const std::string PatternPrefix;
			extern SSUtils_API_DECLSPEC const std::string PatternSuffix;
			extern SSUtils_API_DECLSPEC const std::string NaturalNumberPattern;
			extern SSUtils_API_DECLSPEC const std::string DecIntegerPattern;
			extern SSUtils_API_DECLSPEC const std::string PositiveDecIntegerPattern;
			extern SSUtils_API_DECLSPEC const std::string HexIntegerPattern;
			extern SSUtils_API_DECLSPEC const std::string DecimalPattern;
		};

		namespace RegexCheckers
		{
			extern SSUtils_API_DECLSPEC const RegexChecker NaturalNumberChecker;
			extern SSUtils_API_DECLSPEC const RegexChecker DecIntegerChecker;
			extern SSUtils_API_DECLSPEC const RegexChecker PositiveDecIntegerChecker;
			extern SSUtils_API_DECLSPEC const RegexChecker HexIntegerChecker;
			extern SSUtils_API_DECLSPEC const RegexChecker DecimalChecker;
		};

		namespace RegexMatchers
		{
			extern SSUtils_API_DECLSPEC const RegexMatcher NaturalNumberMatcher;
			extern SSUtils_API_DECLSPEC const RegexMatcher DecIntegerMatcher;
			extern SSUtils_API_DECLSPEC const RegexMatcher PositiveDecIntegerMatcher;
			extern SSUtils_API_DECLSPEC const RegexMatcher HexIntegerMatcher;
			extern SSUtils_API_DECLSPEC const RegexMatcher DecimalMatcher;
		};

		SSUtils_API_DECLSPEC const bool isNaturalNumber(const std::string &src);
		SSUtils_API_DECLSPEC std::vector<std::string> matchNaturalNumber(const std::string &src);

		SSUtils_API_DECLSPEC const bool isInteger(const std::string &src);
		SSUtils_API_DECLSPEC std::vector<std::string> matchInteger(const std::string &src);

		SSUtils_API_DECLSPEC const bool isDecInteger(const std::string &src);
		SSUtils_API_DECLSPEC std::vector<std::string> mathchDecInteger(const std::string &src);

		SSUtils_API_DECLSPEC const bool isPositiveDecInteger(const std::string &src);
		SSUtils_API_DECLSPEC std::vector<std::string> matchPositiveDecInteger(const std::string &src);

		SSUtils_API_DECLSPEC const bool isHexInteger(const std::string &src);
		SSUtils_API_DECLSPEC std::vector<std::string> matchHexInteger(const std::string &src);

		SSUtils_API_DECLSPEC const bool isDecimal(const std::string &src);
		SSUtils_API_DECLSPEC std::vector<std::string> matchDecimal(const std::string &src);
	};
};

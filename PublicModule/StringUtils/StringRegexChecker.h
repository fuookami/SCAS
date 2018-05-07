#pragma once

#include <string>
#include <regex>

namespace SSUtils
{
	namespace String
	{
		struct RegexChecker
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

		struct RegexMatcher
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
			extern const std::string PatternPrefix;
			extern const std::string PatternSuffix;
			extern const std::string NaturalNumberPattern;
			extern const std::string DecIntegerPattern;
			extern const std::string PositiveDecIntegerPattern;
			extern const std::string HexIntegerPattern;
			extern const std::string DecimalPattern;
		};

		namespace RegexCheckers
		{
			extern const RegexChecker NaturalNumberChecker;
			extern const RegexChecker DecIntegerChecker;
			extern const RegexChecker PositiveDecIntegerChecker;
			extern const RegexChecker HexIntegerChecker;
			extern const RegexChecker DecimalChecker;
		};

		namespace RegexMatchers
		{
			extern const RegexMatcher NaturalNumberMatcher;
			extern const RegexMatcher DecIntegerMatcher;
			extern const RegexMatcher PositiveDecIntegerMatcher;
			extern const RegexMatcher HexIntegerMatcher;
			extern const RegexMatcher DecimalMatcher;
		};

		const bool isNaturalNumber(const std::string &src);
		std::vector<std::string> matchNaturalNumber(const std::string &src);

		const bool isInteger(const std::string &src);
		std::vector<std::string> matchInteger(const std::string &src);

		const bool isDecInteger(const std::string &src);
		std::vector<std::string> mathchDecInteger(const std::string &src);

		const bool isPositiveDecInteger(const std::string &src);
		std::vector<std::string> matchPositiveDecInteger(const std::string &src);

		const bool isHexInteger(const std::string &src);
		std::vector<std::string> matchHexInteger(const std::string &src);

		const bool isDecimal(const std::string &src);
		std::vector<std::string> matchDecimal(const std::string &src);
	};
};

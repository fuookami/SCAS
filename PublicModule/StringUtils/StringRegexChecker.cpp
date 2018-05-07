#include "StringRegexChecker.h"

namespace SSUtils
{
	namespace String
	{
		RegexChecker::RegexChecker(const std::string & _pattern)
			: pattern(_pattern), reg(pattern)
		{
		}

		RegexChecker::RegexChecker(std::string && _pattern)
			: pattern(std::move(_pattern)), reg(pattern)
		{
		}

		const bool RegexChecker::operator()(const std::string & src) const
		{
			std::smatch result;
			return std::regex_match(src, result, reg);
		}

		RegexMatcher::RegexMatcher(const std::string & _pattern)
			: pattern(_pattern), reg(pattern)
		{
		}

		RegexMatcher::RegexMatcher(std::string && _pattern)
			: pattern(std::move(_pattern)), reg(pattern)
		{
		}

		std::vector<std::string> RegexMatcher::operator()(const std::string & src) const
		{
			std::vector<std::string> ret;
			for (std::sregex_iterator curr(src.cbegin(), src.cend(), reg), ed; curr != ed; ++curr)
			{
				ret.push_back(curr->str());
			}
			return ret;
		}

		namespace RegexPatterns
		{
			const std::string PatternPrefix("^");
			const std::string PatternSuffix("$");
			const std::string NaturalNumberPattern("(0|[1-9]\\d*)");
			const std::string DecIntegerPattern("-?(0|[1-9]\\d*)");
			const std::string HexIntegerPattern("-?0x(0|[1-9a-fA-F][0-9a-fA-F]*)");
			const std::string PositiveDecIntegerPattern("[1-9]\\d*");
			const std::string DecimalPattern("-?(0|[1-9]\\d*)(.\\d*)");
		};

		namespace RegexCheckers
		{
#define pattern_join(pattern) (RegexPatterns::PatternPrefix + pattern + RegexPatterns::PatternSuffix)
			const RegexChecker NaturalNumberChecker(pattern_join(RegexPatterns::NaturalNumberPattern));
			const RegexChecker DecIntegerChecker(pattern_join(RegexPatterns::DecIntegerPattern));
			const RegexChecker PositiveDecIntegerChecker(pattern_join(RegexPatterns::PositiveDecIntegerPattern));
			const RegexChecker HexIntegerChecker(pattern_join(RegexPatterns::HexIntegerPattern));
			const RegexChecker DecimalChecker(pattern_join(RegexPatterns::DecimalPattern));
#undef pattern_join
		};

		namespace RegexMatchers
		{
			const RegexMatcher NaturalNumberMatcher(RegexPatterns::NaturalNumberPattern);
			const RegexMatcher DecIntegerMatcher(RegexPatterns::DecIntegerPattern);
			const RegexMatcher PositiveDecIntegerMatcher(RegexPatterns::PositiveDecIntegerPattern);
			const RegexMatcher HexIntegerMatcher(RegexPatterns::HexIntegerPattern);
			const RegexMatcher DecimalMatcher(RegexPatterns::DecimalPattern);
		};

		const bool isNaturalNumber(const std::string & src)
		{
			return RegexCheckers::NaturalNumberChecker(src);
		}

		std::vector<std::string> matchNaturalNumber(const std::string & src)
		{
			return RegexMatchers::NaturalNumberMatcher(src);
		}

		const bool isInteger(const std::string &src)
		{
			return isDecInteger(src) || isHexInteger(src);
		}

		std::vector<std::string> matchInteger(const std::string &src)
		{
			std::vector<std::string> temp;
			std::vector<std::string> ret;

			temp = RegexMatchers::DecIntegerMatcher(src);
			std::move(temp.begin(), temp.end(), std::back_inserter(ret));

			temp = RegexMatchers::HexIntegerMatcher(src);
			std::move(temp.begin(), temp.end(), std::back_inserter(ret));

			std::sort(ret.begin(), ret.end());
			ret.erase(std::unique(ret.begin(), ret.end()), ret.end());
			return ret;
		}

		const bool isDecInteger(const std::string & src)
		{
			return RegexCheckers::DecIntegerChecker(src);
		}

		std::vector<std::string> mathchDecInteger(const std::string & src)
		{
			return RegexMatchers::DecIntegerMatcher(src);
		}

		const bool isPositiveDecInteger(const std::string & src)
		{
			return RegexCheckers::PositiveDecIntegerChecker(src);
		}

		std::vector<std::string> matchPositiveDecInteger(const std::string & src)
		{
			return RegexMatchers::PositiveDecIntegerMatcher(src);
		}

		const bool isHexInteger(const std::string &src)
		{
			return RegexCheckers::HexIntegerChecker(src);
		}

		std::vector<std::string> matchHexInteger(const std::string &src)
		{
			return RegexMatchers::HexIntegerMatcher(src);
		}

		const bool isDecimal(const std::string & src)
		{
			return RegexCheckers::DecimalChecker(src);
		}

		std::vector<std::string> matchDecimal(const std::string & src)
		{
			return RegexMatchers::DecimalMatcher(src);
		}
	};
};

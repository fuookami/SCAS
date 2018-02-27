#pragma once

#include <vector>
#include <string>
#include <numeric>
#include <sstream>

namespace StringUtils
{
	static const std::string EmptyString("");

	static const std::string Empty("empty");
	static const std::string Null("null");
	static const std::string None("none");

	static const std::string Right("right");
	static const std::string Error("error");
	static const std::string Wrong("wrong");

	static const std::string Enabled("enabled");
	static const std::string Disabled("disabled");

	static const std::string True("true");
	static const std::string False("false");

	const std::string &getSpaceChars(void);

	inline std::string to_string(const bool val) { return val ? True : False; }
	inline const bool to_bool(const std::string &src) { return src == True ? true : false; }

	struct to_string_t
	{
		template<typename T>
		inline std::string operator()(const T &val) const
		{
			std::ostringstream sout;
			sout << val;
			return sout.str();
		}

		template<>
		inline std::string operator()(const bool &val) const
		{
			return to_string(val);
		}

		template<>
		inline std::string operator()(const std::string &val)const
		{
			return val;
		}
	};

	std::string base64Encode(const std::string &str, const char fillCharacter = '=');
	std::string base64Decode(const std::string &str);

	std::vector<std::string> split(const std::string &source, const std::string &tokens, const bool removeSpace = true);

	namespace _Join
	{
		template<typename iter, typename valT, typename transFunT = to_string_t>
		struct _join_t
		{
			std::string operator()(const iter bgIt, const iter edIt, const std::string &seperator = std::string(""),
				const transFunT translator = to_string_t())
			{
				return std::accumulate(std::next(bgIt), edIt,
					translator(*bgIt), [&seperator, &translator]
					(const std::string &lhs, const valT &rhs) -> std::string
				{
					return lhs + seperator + translator(rhs);
				});
			}
		};
		template<typename iter, typename transFunT>
		struct _join_t<iter, std::string, transFunT>
		{
			std::string operator()(const iter bgIt, const iter edIt, const std::string &seperator = std::string(""),
				const transFunT translator = to_string_t())
			{
				return std::accumulate(std::next(bgIt), edIt,
					*bgIt, [&seperator]
					(const valT &lhs, const valT &rhs) -> std::string
				{
					return lhs + seperator + rhs;
				});
			}
		};
	};

	template<typename iter, typename transFunT = to_string_t>
	std::string join(const iter bgIt, const iter edIt, const std::string &seperator = std::string(""),
		const transFunT translator = to_string_t())
	{
		using valT = typename iter::value_type;
		_Join::_join_t<iter, valT, transFunT> _join;
		return bgIt < edIt
			? _join(bgIt, edIt, seperator, translator)
			: EmptyString;
	}
	template<typename Container, typename transFunT = to_string_t>
	std::string join(const Container cont, const std::string &seperator = std::string(" "),
		const transFunT translator = to_string_t())
	{
		return join(cont.cbegin(), cont.cend(), seperator, translator);
	}
};

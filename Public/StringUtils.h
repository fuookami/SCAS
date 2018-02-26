#pragma once

#include <string>

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

	std::string base64Encode(const std::string &str, const char fillCharacter = '=');
	std::string base64Decode(const std::string &str);

	inline std::string to_string(const bool val) { return val ? True : False; }
	inline const bool to_bool(const std::string &src) { return src == True ? true : false; }
};

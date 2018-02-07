#pragma once

#include <string>

namespace StringUtils
{
	const std::string &getSpaceChars(void);

	std::string base64Encode(const std::string &str, const char fillCharacter = '=');
	std::string base64Decode(const std::string &str);
};

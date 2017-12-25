#pragma once

#include <vector>
#include <string>

namespace DataUtils
{
	using byte = unsigned char;
	using uint32 = unsigned int;
	using Data = std::vector<byte>;

	const std::string toHexString(const Data &data);
};

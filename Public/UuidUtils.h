#pragma once

#include <array>

namespace UUIDUtil
{
	using byte = unsigned char;
	static const unsigned char UUIDLength = 16;

	std::array<byte, UUIDLength> generateUUIDV1(void);
	std::array<byte, UUIDLength> generateUUIDV4(void);
};

#pragma once

#include "Global.h"
#include "DataUtils/DataTranslator.h"
#include <array>
#include <string>

#include <algorithm>

namespace SSUtils
{
	namespace Data
	{
		template<uint32 size>
		Block fromArray(const std::array<byte, size> &data)
		{
			return Block(data.cbegin(), data.cend());
		}
		template<uint32 size>
		std::array<byte, size> toArray(const Block &data)
		{
			std::array<byte, size> ret;
			std::copy(data.cbegin(), size < data.size() ? (data.cbegin() + size) : data.cend(), ret.begin());
			return ret;
		}

		std::string toHexString(const Block &data, const std::string seperator = "");
		Block fromHexString(const std::string &str, const std::string seperator = "");

		std::string toBase64String(const Block &data, const char fillCharacter = '=');
		Block fromBase64String(const std::string &str);

		std::string toString(const Block &data);
		Block fromString(const std::string &str);
		std::wstring toWString(const Block &data);
		Block fromWString(const std::wstring &str);

		Block generateRandomBlock(const uint32 length = 8);
	};
};

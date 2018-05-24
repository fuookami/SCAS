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

		SSUtils_API_DECLSPEC std::string toHexString(const Block &data, const std::string seperator = "");
		SSUtils_API_DECLSPEC Block fromHexString(const std::string &str, const std::string seperator = "");

		SSUtils_API_DECLSPEC std::string toBase64String(const Block &data, const char fillCharacter = '=');
		SSUtils_API_DECLSPEC Block fromBase64String(const std::string &str);

		SSUtils_API_DECLSPEC std::string toString(const Block &data);
		SSUtils_API_DECLSPEC Block fromString(const std::string &str);
		SSUtils_API_DECLSPEC std::wstring toWString(const Block &data);
		SSUtils_API_DECLSPEC Block fromWString(const std::wstring &str);

		SSUtils_API_DECLSPEC Block generateRandomBlock(const uint32 length = 8);
	};
};

SSUtils_API_DECLSPEC const bool operator==(const SSUtils::Block &lhs, const SSUtils::Block &rhs);
SSUtils_API_DECLSPEC const bool operator!=(const SSUtils::Block &lhs, const SSUtils::Block &rhs);
SSUtils_API_DECLSPEC const bool operator<(const SSUtils::Block &lhs, const SSUtils::Block &rhs);
SSUtils_API_DECLSPEC const bool operator<=(const SSUtils::Block &lhs, const SSUtils::Block &rhs);
SSUtils_API_DECLSPEC const bool operator>(const SSUtils::Block &lhs, const SSUtils::Block &rhs);
SSUtils_API_DECLSPEC const bool operator>=(const SSUtils::Block &lhs, const SSUtils::Block &rhs);

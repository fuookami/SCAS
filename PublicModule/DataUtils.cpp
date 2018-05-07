#include "DataUtils.h"
#include "DataUtils/DataTranslator.h"
#include "RandomUtils.h"

#include <boost/archive/iterators/base64_from_binary.hpp>
#include <boost/archive/iterators/binary_from_base64.hpp>
#include <boost/archive/iterators/transform_width.hpp>

#include <sstream>
#include <iomanip>

namespace SSUtils
{
	namespace Data
	{
		std::string toHexString(const Block & data, const std::string seperator)
		{
			std::ostringstream sout;
			for (const byte b : data)
			{
				sout << std::setfill('0') << std::setw(2) << std::hex << static_cast<uint32>(b) << seperator;
			}

			return sout.str();
		}

		Block fromHexString(const std::string & str, const std::string seperator)
		{
			static const uint32 HexSize(2);

			uint32 partSize(HexSize + static_cast<uint32>(seperator.size()));
			Block ret;
			for (uint32 i(0), j(static_cast<uint32>(str.size()) / partSize); i != j; ++i)
			{
				uint16 b;
				std::istringstream(str.substr(i * partSize, HexSize)) >> std::hex >> b;
				ret.push_back(static_cast<byte>(b));
			}

			return ret;
		}

		std::string toBase64String(const Block & data, const char fillCharacter)
		{
			using namespace boost::archive::iterators;
			typedef base64_from_binary<transform_width<std::vector<byte>::const_iterator, 6, 8>> Base64EncodeIter;

			std::stringstream  result;
			std::copy(Base64EncodeIter(data.begin()), Base64EncodeIter(data.end()), std::ostream_iterator<char>(result));

			size_t Num = (3 - data.size() % 3) % 3;
			for (size_t i = 0; i < Num; i++)
			{
				result.put(fillCharacter);
			}
			return result.str();
		}

		Block fromBase64String(const std::string & str)
		{
			using namespace boost::archive::iterators;
			typedef transform_width<binary_from_base64<std::string::const_iterator>, 8, 6> Base64DecodeIter;

			std::stringstream result;
			try
			{
				std::copy(Base64DecodeIter(str.begin()), Base64DecodeIter(str.end()), std::ostream_iterator<char>(result));
				return fromString(result.str());
			}
			catch (...)
			{
				return Block();
			}
		}

		std::string toString(const Block & data)
		{
			return std::string(data.cbegin(), data.cend());
		}

		Block fromString(const std::string & str)
		{
			return Block(str.cbegin(), str.cend());
		}

		std::wstring toWString(const Block & data)
		{
			static const DataTranslator<wchar> tranlator;
			return tranlator.toDataContainer<std::wstring>(data);
		}

		Block fromWString(const std::wstring & str)
		{
			static const DataTranslator<wchar> tranlator;
			return tranlator.fromDataContainer<std::wstring>(str);
		}

		Block generateRandomBlock(const uint32 length)
		{
			Block ret(length, 0);
			auto gen(SSUtils::Random::generateNewRandomGenerator_64<uint8>());
			std::uniform_int_distribution<> dis(0, 0xff);
			for (uint32 i(0); i != length; ++i)
			{
				ret[i] = static_cast<byte>(dis(gen));
			}

			return ret;
		}
	};
};

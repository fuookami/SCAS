#include "StringConvertUtils.h"

#include <boost/archive/iterators/base64_from_binary.hpp>  
#include <boost/archive/iterators/binary_from_base64.hpp>  
#include <boost/archive/iterators/transform_width.hpp>

namespace StringConvertUtils
{
	const std::string toDBS(const std::string & src)
	{
		std::string ret;

		for (auto currIt(src.cbegin()), edIt(src.cend()); currIt != edIt; ++currIt)
		{
			unsigned char thisByte(*currIt);
			unsigned char nextByte(*std::next(currIt));

			if (thisByte == 0xA1 && nextByte == 0xA1) // 全角空格
			{
				ret.push_back(0x20);
				++currIt;
			}
			else if (thisByte == 0xA3 && nextByte >= 0xA1 && nextByte <= 0xFE) // ASCII码其它可显示字符
			{
				ret.push_back(nextByte - 0x80);
				++currIt;
			}
			else
			{
				if (thisByte >= 0x80)
				{
					ret.push_back(thisByte);
					ret.push_back(nextByte);
					++currIt;
				}
				else
				{
					ret.push_back(thisByte);
				}
			}
		}

		return ret;
	}

	const std::string toQBS(const std::string & src)
	{
		std::string ret;

		for (auto currIt(src.cbegin()), edIt(src.cend()); currIt != edIt; ++currIt)
		{
			unsigned char thisByte(*currIt);

			if (thisByte == 0x20) // 半角空格
			{
				ret.insert(ret.end(), 2, 0xA1i8);
			}
			else if (thisByte >= 0x21 && thisByte < 0x80)
			{
				ret.push_back(0xA3i8);
				ret.push_back(thisByte + 0x80);
			}
			else
			{
				if (thisByte >= 0x80)
				{
					ret.push_back(thisByte);
					++currIt;
					ret.push_back(*currIt);
				}
				else
				{
					ret.push_back(thisByte);
				}
			}
		}

		return ret;
	}

	const std::string base64Encode(const std::string & str, const char fillCharacter)
	{
		using namespace boost::archive::iterators;
		typedef base64_from_binary<transform_width<std::string::const_iterator, 6, 8>> Base64EncodeIter;

		std::stringstream  result;
		std::copy(Base64EncodeIter(str.begin()), Base64EncodeIter(str.end()), std::ostream_iterator<char>(result));

		size_t Num = (3 - str.size() % 3) % 3;
		for (size_t i = 0; i < Num; i++)
		{
			result.put(fillCharacter);
		}
		return result.str();
	}

	const std::string base64Decode(const std::string & str)
	{
		using namespace boost::archive::iterators;
		typedef transform_width<binary_from_base64<std::string::const_iterator>, 8, 6> Base64DecodeIter;

		std::stringstream result;
		try
		{
			std::copy(Base64DecodeIter(str.begin()), Base64DecodeIter(str.end()), std::ostream_iterator<char>(result));
			return result.str();
		}
		catch (...)
		{
			static const std::string EmptyString("");
			return EmptyString;
		}
	}
};

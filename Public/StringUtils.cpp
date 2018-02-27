#include "StringUtils.h"

#include <sstream>

#include <boost/algorithm/string/classification.hpp>
#include <boost/algorithm/string/split.hpp>
#include <boost/archive/iterators/base64_from_binary.hpp>  
#include <boost/archive/iterators/binary_from_base64.hpp>  
#include <boost/archive/iterators/transform_width.hpp>

namespace StringUtils
{
	const std::string &getSpaceChars(void)
	{
		static std::string ret;
		if (ret.empty())
		{
			for (int i(0); i != INT8_MAX; ++i)
			{
				if (isspace(i))
				{
					ret.push_back(static_cast<char>(i));
				}
			}
		}

		return ret;
	}

	std::string base64Encode(const std::string & str, const char fillCharacter)
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

	std::string base64Decode(const std::string & str)
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
			return EmptyString;
		}
	}

	std::vector<std::string> split(const std::string & source, const std::string & tokens, const bool removeSpace)
	{
		std::vector<std::string> ret;
		if (removeSpace)
		{
			boost::split(ret, source, boost::is_any_of(tokens));
		}
		else
		{
			boost::split(ret, source, boost::is_any_of(tokens), boost::token_compress_off);
		}
		
		return ret;
	}
};

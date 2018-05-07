#include "StringUtils.h"
#include <sstream>
#include <iomanip>
#include <boost/algorithm/string/classification.hpp>
#include <boost/algorithm/string/split.hpp>
#include <boost/archive/iterators/base64_from_binary.hpp>  
#include <boost/archive/iterators/binary_from_base64.hpp>  
#include <boost/archive/iterators/transform_width.hpp>

namespace SSUtils
{
	namespace String
	{
		std::string getVersion(const int major, const int sub, const int modify)
		{
			std::ostringstream sout;
			sout << major << "." << std::setfill('0') << std::setw(2) << sub << "." << std::setw(3) << modify;
			return sout.str();
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
				std::string ret(result.str());
				ret.erase(std::find(ret.begin(), ret.end(), '\0'), ret.end());
				return ret;
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

		std::vector<std::wstring> split(const std::wstring & source, const std::wstring & tokens, const bool removeSpace)
		{
			std::vector<std::wstring> ret;
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
};

namespace std
{
	std::string to_string(const bool value)
	{
		return value ? SSUtils::String::True : SSUtils::String::False;
	}

	bool stoboolean(const std::string & str)
	{
		if (SSUtils::String::isInteger(str))
		{
			SSUtils::integer i(str);
			return i != 0;
		}
		else
		{
			return str == SSUtils::String::True;
		}
		return false;
	}
};

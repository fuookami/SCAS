#pragma once

#include <vector>
#include <string>
#include <boost/locale.hpp>

namespace StringConvertUtils
{
	enum class CharType
	{
		UTF8,
		UTF16,
		BIG5,
		GBK,
		GB2312
	};

	static const std::vector<std::string> StringCodeName =
	{
		"UTF-8", "UTF-16", "BIG5", "GBK", "GB2312"
	};

#ifdef _WIN32
	static const CharType LocalStringCodeId = CharType::GB2312;
#else
	static const CharType LocalStringCodeId = CharType::UTF8;
#endif

	static const std::string LocalStringCode = StringCodeName[static_cast<unsigned int>(LocalStringCodeId)];

	template<CharType srcCodeId, CharType destCodeId = LocalStringCodeId>
	inline std::string convert(const std::string &src)
	{
		if (srcCodeId == destCodeId)
		{
			return src;
		}
		else
		{
			return boost::locale::conv::between(src, StringCodeName[static_cast<unsigned int>(destCodeId)], StringCodeName[static_cast<unsigned int>(srcCodeId)]);
		}
	}

	template<CharType id>
	inline std::string fromLocal(const std::string &src)
	{
		return convert<LocalStringCodeId, id>(src);
	}
	template<CharType id>
	inline std::string toLocal(const std::string &src)
	{
		return convert<id, LocalStringCodeId>(src);
	}

	// GBK全角转半角
	std::string _toDBS(const std::string &src);
	// GBK半角转全角
	std::string _toQBS(const std::string &src);

	// 全角转半角
	template<CharType id>
	inline std::string toDBS(const std::string &src)
	{
		return convert<id>(_toDBS(convert<CharType::GBK>(src)));
	}
	// 半角转全角
	template<CharType id>
	inline std::string toQBS(const std::string &src)
	{
		return convert<id>(_toQBS(convert<CharType::GBK>(src)));
	}
};

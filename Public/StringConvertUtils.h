﻿#pragma once

#include <vector>
#include <string>
#include <boost/locale.hpp>

namespace StringConvertUtils
{
#ifdef WIN32
	static const std::string LocalStringCode = "GB2312";
#else
	static const std::string LocalStringCode = "UTF-8";
#endif

	enum class StringCodeId
	{
		UTF8,
		UTF16,
		BIG5,
		GBK
	};

	static const std::vector<std::string> StringCodeName = 
	{
		"UTF-8", "UTF-16", "BIG5", "GBK"
	};

	template<StringCodeId id>
	const std::string fromLocal(const std::string &src)
	{
		return boost::locale::conv::between(src, StringCodeName[static_cast<unsigned int>(id)], LocalStringCode);
	}

	template<StringCodeId id>
	const std::string toLocal(const std::string &src)
	{
		return boost::locale::conv::between(src, LocalStringCode, StringCodeName[static_cast<unsigned int>(id)]);
	}

	// 转半角
	const std::string toDBS(const std::string &src);
	// 转全角
	const std::string toQBS(const std::string &src);

	const std::string base64Encode(const std::string &str, const char fillCharacter = '=');
	const std::string base64Decode(const std::string &str);
};

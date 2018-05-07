#include "StringConverter.h"
#include "_pri_string_convert.h"
#include <boost/locale.hpp>

namespace SSUtils
{
	namespace String
	{
		std::wstring toWString(const std::string & src, const CharType charType)
		{
			return boost::locale::conv::to_utf<wchar>(src, CharTypeCode.find(charType)->second);
		}

		std::string toString(const std::wstring & src, const CharType charType)
		{
			return boost::locale::conv::from_utf<wchar>(src, CharTypeCode.find(charType)->second);
		}

		Converter::Converter(const CharType _srcCharType, const CharType _destCharType)
			: srcCharType(_srcCharType), destCharType(_destCharType)
		{
		}

		std::string Converter::operator()(const std::string & src) const
		{
			return boost::locale::conv::between(src, CharTypeCode.find(destCharType)->second, CharTypeCode.find(srcCharType)->second);
		}

		std::wstring Converter::operator()(const std::wstring & src) const
		{
			return toWString(boost::locale::conv::between(toString(src), CharTypeCode.find(destCharType)->second, CharTypeCode.find(srcCharType)->second));
		}

		std::string fromLocal(const CharType targetCharType, const std::string & src)
		{
			Converter converter(LocalCharType, targetCharType);
			return converter(src);
		}

		std::wstring fromLocal(const CharType targetCharType, const std::wstring & src)
		{
			Converter converter(LocalCharType, targetCharType);
			return converter(src);
		}

		std::string toLocal(const CharType srcCharType, const std::string & src)
		{
			Converter converter(srcCharType);
			return converter(src);
		}

		std::wstring toLocal(const CharType srcCharType, const std::wstring & src)
		{
			Converter converter(srcCharType);
			return converter(src);
		}

		std::string toDBS(const std::string & src, const CharType charType)
		{
			if (charType == CharType::GBK)
			{
				return _toDBS(src);
			}
			else
			{
				Converter toGBKConverter(charType, CharType::GBK);
				Converter fromGBKConverter(CharType::GBK, charType);
				return fromGBKConverter(_toDBS(toGBKConverter(src)));
			}
		}

		std::string toQBS(const std::string & src, const CharType charType)
		{
			if (charType == CharType::GBK)
			{
				return _toQBS(src);
			}
			else
			{
				Converter toGBKConverter(charType, CharType::GBK);
				Converter fromGBKConverter(CharType::GBK, charType);
				return fromGBKConverter(_toQBS(toGBKConverter(src)));
			}
		}
	};
};

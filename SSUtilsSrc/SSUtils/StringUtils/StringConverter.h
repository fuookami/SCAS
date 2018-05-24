#pragma once

#include "_pri_string_global.h"

namespace SSUtils
{
	namespace String
	{
		SSUtils_API_DECLSPEC std::wstring toWString(const std::string &src, const CharType charType = LocalCharType());
		SSUtils_API_DECLSPEC std::string toString(const std::wstring &src, const CharType charType = LocalCharType());

		struct SSUtils_API_DECLSPEC Converter
		{
			CharType srcCharType;
			CharType destCharType;

			Converter(const CharType _srcCharType, const CharType _destCharType = LocalCharType());
			Converter(const Converter &ano) = delete;
			Converter(Converter &&ano) = delete;
			Converter &operator=(const Converter &rhs) = delete;
			Converter &operator=(Converter &&rhs) = delete;
			~Converter(void) = default;

			std::string operator()(const std::string &src) const;
			std::wstring operator()(const std::wstring &src) const;
		};

		SSUtils_API_DECLSPEC std::string fromLocal(const CharType targetCharType, const std::string &src);
		SSUtils_API_DECLSPEC std::wstring fromLocal(const CharType targetCharType, const std::wstring &src);
		SSUtils_API_DECLSPEC std::string toLocal(const CharType srcCharType, const std::string &src);
		SSUtils_API_DECLSPEC std::wstring toLocal(const CharType srcCharType, const std::wstring &src);

		SSUtils_API_DECLSPEC std::string toDBS(const std::string &src, const CharType charType = LocalCharType());
		SSUtils_API_DECLSPEC std::string toQBS(const std::string &src, const CharType charType = LocalCharType());
	};
};

#pragma once

#include "_pri_datetime_global.h"

namespace SSUtils
{
	namespace Datetime
	{
		class SSUtils_API_DECLSPEC Date
		{
		public:
			static const Date EmptyDate;

		public:
			Date(void);
			Date(const int16 year, const uint8 month, const uint8 day);
			Date(const Date &ano) = default;
			Date(Date &&ano) = default;
			Date &operator=(const Date &rhs) = default;
			Date &operator=(Date &&rhs) = default;
			~Date(void) = default;

			Date &operator+=(const DateDuration &duration);
			Date &operator-=(const DateDuration &duration);

			Date getDateAfter(const DateDuration &duration) const;
			Date getDateBefore(const DateDuration &duration) const;

			uint16 year(void) const;
			void setYear(const uint16 year);
			uint8 month(void) const;
			void setMonth(const uint8 month);
			uint8 day(void) const;
			void setDay(const uint8 day);

			const bool isLegalDate(void) const;
			const bool isLeapYear(void) const;
			const uint8 getDayOfMonth(void) const;
			const uint8 getDayInWeek(void) const;

			static Date fromString(const std::string &str);
			std::string toString(void) const;

		private:
			int16 m_year;
			uint8 m_month;
			uint8 m_day;
		};

		class SSUtils_API_DECLSPEC DateDuration
		{
		public:
			static const DateDuration EmptyDateDuration;

			DateDuration(void);
			DateDuration(const int32 day);
			DateDuration(const int32 month, const int32 day);
			DateDuration(const int32 year, const int32 month, const int32 day);
			DateDuration(const DateDuration &ano) = default;
			DateDuration(DateDuration &&ano) = default;
			DateDuration &operator=(const DateDuration &rhs) = default;
			DateDuration &operator=(DateDuration &&rhs) = default;
			~DateDuration(void) = default;

			DateDuration &operator+=(const DateDuration &duration);
			DateDuration &operator-=(const DateDuration &duration);

			DateDuration operator+(void) const;
			DateDuration operator-(void) const;

			int32 year(void) const;
			void setYear(const int32 year);
			int32 month(void) const;
			void setMonth(const int32 month);
			int32 day(void) const;
			void setDay(const int32 day);

			static DateDuration fromString(const std::string &str);
			std::string toString(void) const;

		private:
			int32 m_year;
			int32 m_month;
			int32 m_day;
		};

		SSUtils_API_DECLSPEC Date getLocalDate(void);
		SSUtils_API_DECLSPEC Date getDateAfterLocalDate(const DateDuration &duration);
		SSUtils_API_DECLSPEC Date getDateBeforeLocalDate(const DateDuration &duration);

		SSUtils_API_DECLSPEC const bool isLeapYear(const int16 year);
		SSUtils_API_DECLSPEC const bool isLeapYear(const Date & date);

		SSUtils_API_DECLSPEC const uint8 getDaysOfMonth(const int16 year, const uint8 m_month);
		SSUtils_API_DECLSPEC const uint8 getDaysOfMonth(const Date &date);

		SSUtils_API_DECLSPEC const uint8 getDayInWeek(int16 year, uint8 month, const uint8 day);
		SSUtils_API_DECLSPEC const uint8 getDayInWeek(const Date &date);
	};
};

SSUtils_API_DECLSPEC const bool operator<(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);
SSUtils_API_DECLSPEC const bool operator<=(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);
SSUtils_API_DECLSPEC const bool operator>(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);
SSUtils_API_DECLSPEC const bool operator>=(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);
SSUtils_API_DECLSPEC const bool operator==(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);
SSUtils_API_DECLSPEC const bool operator!=(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);

SSUtils_API_DECLSPEC const bool operator<(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const bool operator<=(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const bool operator>(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const bool operator>=(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const bool operator==(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const bool operator!=(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);

SSUtils_API_DECLSPEC const SSUtils::Datetime::Date operator+(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const SSUtils::Datetime::Date operator-(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const SSUtils::Datetime::DateDuration operator-(const SSUtils::Datetime::Date &lhs, const SSUtils::Datetime::Date &rhs);

SSUtils_API_DECLSPEC const SSUtils::Datetime::DateDuration operator+(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);
SSUtils_API_DECLSPEC const SSUtils::Datetime::DateDuration operator-(const SSUtils::Datetime::DateDuration &lhs, const SSUtils::Datetime::DateDuration &rhs);

SSUtils_API_DECLSPEC std::ostream &operator<<(std::ostream &os, const SSUtils::Datetime::Date &date);
SSUtils_API_DECLSPEC std::ostream &operator<<(std::ostream &os, const SSUtils::Datetime::DateDuration &dateDuration);

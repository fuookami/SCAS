#pragma once

#include "Date.h"
#include "Time.h"

namespace SSUtils
{
	namespace Datetime
	{
		class SSUtils_API_DECLSPEC Datetime : public Date
		{
		public:
			static const Datetime EmptyDatetime;

			Datetime(void);
			Datetime(const int16 year, const uint8 month, const uint8 day, const int32 hour, const uint8 minute, const uint8 second, const uint16 millisecond = 0, const uint16 microsecond = 0);
			Datetime(const Date &date, const uint32 second);
			Datetime(const Date &date, const int32 hour, const uint8 minute, const uint8 second, const uint16 millisecond = 0, const uint16 microsecond = 0);
			Datetime(const int16 year, const uint8 month, const uint8 day, const Time &time);
			Datetime(const Date &date);
			Datetime(const Date &date, const Time &time);
			Datetime(const Datetime &ano) = default;
			Datetime(Datetime &&ano) = default;
			Datetime &operator=(const Datetime &rhs) = default;
			Datetime &operator=(Datetime &&rhs) = default;
			~Datetime(void) = default;

			Datetime &operator+=(const DatetimeDuration &duration);
			Datetime &operator-=(const DatetimeDuration &duration);

			Datetime getDatetimeAfter(const DatetimeDuration &duration) const;
			Datetime getDatetimeBefore(const DatetimeDuration &duration) const;

			const int32 hour(void) const;
			void setHour(const int32 hour);
			const uint8 minute(void) const;
			void setMinute(const uint8 minute);
			const uint8 second(void) const;
			void setSecond(const uint8 second);
			const uint16 millisecond(void) const;
			void setMillisecond(const uint16 millisecond);
			const uint16 microsecond(void) const;
			void setMicrosecond(const uint16 microsecond);
			const Precision precision(void)const;
			void setPrecision(const Precision precision);
			const uint32 fractionsecond(void) const;

			const bool isTomorrow(void) const;
			const bool isTheDayAfterTomorrow(void) const;
			const bool isTheDaysAfter(void) const;

			const bool isYesterday(void) const;
			const bool isTheDayBeforeYesterday(void) const;
			const bool isTheDaysBefore(void) const;

			static Datetime fromString(const std::string &str);
			std::string toString(void) const;

			void tidy(void);

		private:
			int32 m_hour;
			uint8 m_minute;
			uint8 m_second;
			uint16 m_millisecond;
			uint16 m_microsecond;
			Precision m_precision;
		};

		class SSUtils_API_DECLSPEC DatetimeDuration : public DateDuration
		{
		public:
			DatetimeDuration(void);
			DatetimeDuration(const int32 year, const int32 month, const int32 day, const int32 hour, const int32 minute, const int32 second, const int32 millisecond = 0, const int32 microsecond = 0);
			DatetimeDuration(const DateDuration &dateDuration, const uint32 second);
			DatetimeDuration(const DateDuration &dateDuration, const int32 hour, const int32 minute, const int32 second, const int32 millisecond = 0, const int32 microsecond = 0);
			DatetimeDuration(const int32 day, const TimeDuration &timeDuration);
			DatetimeDuration(const int32 month, const int32 day, const TimeDuration &timeDuration);
			DatetimeDuration(const int32 year, const int32 month, const int32 day, const TimeDuration &timeDuration);
			DatetimeDuration(const DateDuration &dateDuration);
			DatetimeDuration(const DateDuration &dateDuration, const TimeDuration &timeDuration);
			DatetimeDuration(const TimeDuration &timeDuration);
			DatetimeDuration(const DatetimeDuration &ano) = default;
			DatetimeDuration(DatetimeDuration &&ano) = default;
			DatetimeDuration &operator=(const DatetimeDuration &rhs) = default;
			DatetimeDuration &operator=(DatetimeDuration &&rhs) = default;
			~DatetimeDuration(void) = default;

			static DatetimeDuration fromTimeDuration(const TimeDuration &timeDuration);
			TimeDuration toTimeDuration(void) const;

			DatetimeDuration &operator+=(const DatetimeDuration &duration);
			DatetimeDuration &operator-=(const DatetimeDuration &duration);
			
			DatetimeDuration operator+(void) const;
			DatetimeDuration operator-(void) const;

			const int32 hour(void) const;
			void setHour(const int32 hour);
			const int32 minute(void) const;
			void setMinute(const int32 minute);
			const int32 second(void) const;
			void setSecond(const int32 second);
			const int32 millisecond(void) const;
			void setMillisecond(const int32 millisecond);
			const int32 microsecond(void) const;
			void setMicrosecond(const int32 microsecond);
			const Precision precision(void)const;
			void setPrecision(const Precision precision);
			const int32 fractionsecond(void) const;

			static DatetimeDuration fromString(const std::string &str);
			std::string toString(void) const;

			void tidy(void);

		private:
			int32 m_hour;
			int32 m_minute;
			int32 m_second;
			int32 m_millisecond;
			int32 m_microsecond;
			Precision m_precision;
		};

		SSUtils_API_DECLSPEC Datetime getLocalDatetime(void);
		SSUtils_API_DECLSPEC Datetime getDatetimeAfterLocalDatetime(const DatetimeDuration &duration);
		SSUtils_API_DECLSPEC Datetime getDatetimeBeforeLocalDatetime(const DatetimeDuration &duration);
	};
};

SSUtils_API_DECLSPEC const bool operator<(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);
SSUtils_API_DECLSPEC const bool operator<=(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);
SSUtils_API_DECLSPEC const bool operator>(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);
SSUtils_API_DECLSPEC const bool operator>=(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);
SSUtils_API_DECLSPEC const bool operator==(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);
SSUtils_API_DECLSPEC const bool operator!=(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);

SSUtils_API_DECLSPEC const bool operator<(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const bool operator<=(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const bool operator>(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const bool operator>=(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const bool operator==(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const bool operator!=(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);

SSUtils_API_DECLSPEC const SSUtils::Datetime::Datetime operator+(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const SSUtils::Datetime::Datetime operator-(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const SSUtils::Datetime::DatetimeDuration operator-(const SSUtils::Datetime::Datetime &lhs, const SSUtils::Datetime::Datetime &rhs);

SSUtils_API_DECLSPEC const SSUtils::Datetime::DatetimeDuration operator+(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);
SSUtils_API_DECLSPEC const SSUtils::Datetime::DatetimeDuration operator-(const SSUtils::Datetime::DatetimeDuration &lhs, const SSUtils::Datetime::DatetimeDuration &rhs);

SSUtils_API_DECLSPEC std::ostream &operator<<(std::ostream &os, const SSUtils::Datetime::Datetime &datetime);
SSUtils_API_DECLSPEC std::ostream &operator<<(std::ostream &os, const SSUtils::Datetime::DatetimeDuration &datetimeDuration);

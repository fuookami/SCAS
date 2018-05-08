#include "Datetime.h"
#include "Time.h"
#include "StringUtils.h"
#include "MathUtils.h"
#include <boost/date_time/gregorian/gregorian.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <sstream>
#include <iomanip>

namespace SSUtils
{
	namespace Datetime
	{
		Datetime::Datetime(void)
			: Datetime(EmptyDate, 0, 0, 0, 0, 0)
		{
		}

		Datetime::Datetime(const int16 year, const uint8 month, const uint8 day, const int32 hour, const uint8 minute, const uint8 second, const uint16 millisecond, const uint16 microsecond)
			: Datetime(Date(year, month, day), hour, minute, second, millisecond, microsecond)
		{
		}

		Datetime::Datetime(const Date & date, const uint32 second)
			: Datetime(date, second / SecondsPerHour, Math::mod(second / SecondsPerMinute, MinutesPerHour), Math::mod(second, SecondsPerMinute), static_cast<uint16>(0), static_cast<uint16>(0))
		{
		}

		Datetime::Datetime(const Date & date, const int32 hour, const uint8 minute, const uint8 second, const uint16 millisecond, const uint16 microsecond)
			: Date(date), m_hour(hour), m_minute(minute), m_second(second), m_millisecond(millisecond), m_microsecond(microsecond)
		{
			m_precision = microsecond != 0 ? Precision::MicroSecond
				: millisecond != 0 ? Precision::MilliSecond : Precision::Second;
		}

		Datetime::Datetime(const int16 year, const uint8 month, const uint8 day, const Time & time)
			: Datetime(Date(year, month, day), time.hour(), time.minute(), time.second(), time.millisecond(), time.microsecond())
		{
		}

		Datetime::Datetime(const Date & date)
			: Datetime(date, 0, 0, 0, 0, 0)
		{
		}

		Datetime::Datetime(const Date & date, const Time & time)
			: Datetime(date, time.hour(), time.minute(), time.second(), time.millisecond(), time.microsecond())
		{
		}

		Datetime & Datetime::operator+=(const DatetimeDuration & duration)
		{
			*this = this->getDatetimeAfter(duration);
			return *this;
		}

		Datetime & Datetime::operator-=(const DatetimeDuration & duration)
		{
			*this = this->getDatetimeBefore(duration);
			return *this;
		}

		Datetime Datetime::getDatetimeAfter(const DatetimeDuration & duration) const
		{
			using namespace boost::gregorian;
			using namespace boost::posix_time;

			Datetime temp(*this);
			temp.tidy();

			date currDate(temp.year(), temp.month(), temp.day());
			time_duration currTime(temp.hour(), temp.minute(), temp.second(), temp.fractionsecond());
			ptime targetDatetime(currDate, currTime);

			targetDatetime = duration.year() > 0 ? (targetDatetime + years(duration.year()))
				: temp.year() < 0 ? (targetDatetime - years(abs(duration.year()))) : targetDatetime;
			targetDatetime = duration.month() > 0 ? (targetDatetime + months(duration.month()))
				: duration.month() < 0 ? (targetDatetime - months(abs(duration.month()))) : targetDatetime;
			targetDatetime = duration.day() > 0 ? (targetDatetime + days(duration.day()))
				: duration.day() < 0 ? (targetDatetime - days(abs(duration.day()))) : targetDatetime;
			targetDatetime = duration.hour() > 0 ? (targetDatetime + hours(duration.hour()))
				: duration.hour() < 0 ? (targetDatetime - hours(abs(duration.hour()))) : targetDatetime;
			targetDatetime = duration.minute() > 0 ? (targetDatetime + minutes(duration.minute()))
				: duration.minute() < 0 ? (targetDatetime - minutes(abs(duration.minute()))) : targetDatetime;
			targetDatetime = duration.second() > 0 ? (targetDatetime + seconds(duration.second()))
				: duration.second() < 0 ? (targetDatetime - seconds(abs(duration.second()))) : targetDatetime;
			targetDatetime = duration.millisecond() > 0 ? (targetDatetime + milliseconds(duration.millisecond()))
				: duration.millisecond() < 0 ? (targetDatetime - milliseconds(abs(duration.millisecond()))) : targetDatetime;
			targetDatetime = duration.microsecond() > 0 ? (targetDatetime + microseconds(duration.microsecond()))
				: duration.microsecond() < 0 ? (targetDatetime - microseconds(abs(duration.microsecond()))) : targetDatetime;

			const date &targetDate(targetDatetime.date());
			const time_duration &targetTime(targetDatetime.time_of_day());

			uint32 microseconds = static_cast<uint32>(targetTime.fractional_seconds() / MicrosecondPerFractionSecond);
			return Datetime(static_cast<int16>(targetDate.year()), static_cast<uint8>(targetDate.month()), static_cast<uint8>(targetDate.day()), static_cast<int32>(targetTime.hours()), static_cast<uint8>(targetTime.minutes()), static_cast<uint8>(targetTime.seconds()), static_cast<uint16>(microseconds / MicrosecondsPerMillisecond), static_cast<uint16>(Math::mod(microseconds, MicrosecondsPerMillisecond)));
		}

		Datetime Datetime::getDatetimeBefore(const DatetimeDuration & duration) const
		{
			return this->getDatetimeAfter(-duration);
		}

		const int32 Datetime::hour(void) const
		{
			return m_hour;
		}

		void Datetime::setHour(const int32 hour)
		{
			m_hour = hour;
		}

		const uint8 Datetime::minute(void) const
		{
			return m_minute;
		}

		void Datetime::setMinute(const uint8 minute)
		{
			m_minute = minute;
		}

		const uint8 Datetime::second(void) const
		{
			return m_second;
		}

		void Datetime::setSecond(const uint8 second)
		{
			m_second = second;
		}

		const uint16 Datetime::millisecond(void) const
		{
			return m_precision == Precision::MilliSecond || m_precision == Precision::MicroSecond ? m_millisecond : 0;
		}

		void Datetime::setMillisecond(const uint16 millisecond)
		{
			m_millisecond = millisecond;
			if (m_precision == Precision::Second)
			{
				m_precision = Precision::MilliSecond;
			}
		}

		const uint16 Datetime::microsecond(void) const
		{
			return m_precision == Precision::MicroSecond ? m_microsecond : 0;
		}

		void Datetime::setMicrosecond(const uint16 microsecond)
		{
			m_microsecond = microsecond;
			if (m_precision != Precision::MicroSecond)
			{
				m_precision = Precision::MicroSecond;
			}
		}

		const Precision Datetime::precision(void) const
		{
			return m_precision;
		}

		void Datetime::setPrecision(const Precision precision)
		{
			m_precision = precision;
		}

		const uint32 Datetime::fractionsecond(void) const
		{
			return (static_cast<uint32>(millisecond()) * MicrosecondsPerMillisecond + microsecond()) * MicrosecondPerFractionSecond;
		}

		const bool Datetime::isTomorrow(void) const
		{
			return hour() / HoursPerDay == 1;
		}

		const bool Datetime::isTheDayAfterTomorrow(void) const
		{
			return hour() / HoursPerDay == 2;
		}

		const bool Datetime::isTheDaysAfter(void) const
		{
			return hour() / HoursPerDay >= 1;
		}

		const bool Datetime::isYesterday(void) const
		{
			return hour() / HoursPerDay == -1;
		}

		const bool Datetime::isTheDayBeforeYesterday(void) const
		{
			return hour() / HoursPerDay == -2;
		}

		const bool Datetime::isTheDaysBefore(void) const
		{
			return hour() / HoursPerDay <= -1;
		}

		Datetime Datetime::fromString(const std::string & str)
		{
			static const std::string Tokens(" ");
			const auto parts(String::split(str, Tokens));

			if (parts.size() != 2)
			{
				return Datetime();
			}
			else
			{
				return Datetime(Date::fromString(parts[0]), Time::fromString(parts[1]));
			}
		}

		std::string Datetime::toString(void) const
		{
			static const std::string Seperator(":");
			static const std::string FractionSeperator(".");

			std::ostringstream datatimeSout;
			datatimeSout << Date::toString()
				<< hour() << Seperator
				<< std::setw(2) << static_cast<uint16>(minute()) << Seperator
				<< std::setw(2) << static_cast<uint16>(second());
			if (precision() == Precision::MilliSecond || precision() == Precision::MicroSecond)
			{
				datatimeSout << FractionSeperator << std::setfill('0') << std::setw(3) << millisecond();
			}
			if (precision() == Precision::MicroSecond)
			{
				datatimeSout << std::setfill('0') << std::setw(3) << microsecond();
			}
			return datatimeSout.str();
		}

		void Datetime::tidy(void)
		{
			m_precision = m_microsecond != 0 ? Precision::MicroSecond
				: m_millisecond != 0 ? Precision::MilliSecond : Precision::Second;

			m_millisecond += m_microsecond / MicrosecondsPerMillisecond;
			m_microsecond = Math::mod(m_microsecond, MicrosecondsPerMillisecond);

			m_second += m_millisecond / MillisecondsPerSecond;
			m_millisecond = Math::mod(m_millisecond, MillisecondsPerSecond);

			m_minute += m_second / SecondsPerMinute;
			m_second = Math::mod(m_second, SecondsPerMinute);

			m_hour += m_minute / MinutesPerHour;
			m_minute = Math::mod(m_minute, MinutesPerHour);

			int32 disDays(m_hour % HoursPerDay);
			*this += DatetimeDuration(DateDuration(0, 0, disDays));
		}

		DatetimeDuration::DatetimeDuration(void)
			: DatetimeDuration(DateDuration(), 0, 0, 0, 0, 0)
		{
		}

		DatetimeDuration::DatetimeDuration(const int32 year, const int32 month, const int32 day, const int32 hour, const int32 minute, const int32 second, const int32 millisecond, const int32 microsecond)
			: DatetimeDuration(DateDuration(year, month, day), hour, minute, second, millisecond, microsecond)
		{
		}

		DatetimeDuration::DatetimeDuration(const DateDuration & dateDuration, const uint32 second)
			: DatetimeDuration(dateDuration, second / SecondsPerHour, Math::mod(second / SecondsPerMinute, MinutesPerHour), Math::mod(second, SecondsPerMinute), 0, 0)
		{
		}

		DatetimeDuration::DatetimeDuration(const DateDuration & dateDuration, const int32 hour, const int32 minute, const int32 second, const int32 millisecond, const int32 microsecond)
			: DateDuration(dateDuration), m_hour(hour), m_minute(minute), m_second(second), m_millisecond(millisecond), m_microsecond(microsecond)
		{
			tidy();
		}

		DatetimeDuration::DatetimeDuration(const int32 day, const TimeDuration & timeDuration)
			: DatetimeDuration(DateDuration(0, 0, day), timeDuration)
		{
		}

		DatetimeDuration::DatetimeDuration(const int32 month, const int32 day, const TimeDuration & timeDuration)
			: DatetimeDuration(DateDuration(0, month, day), timeDuration)
		{
		}

		DatetimeDuration::DatetimeDuration(const int32 year, const int32 month, const int32 day, const TimeDuration & timeDuration)
			: DatetimeDuration(DateDuration(year, month, day), timeDuration)
		{
		}

		DatetimeDuration::DatetimeDuration(const DateDuration & dateDuration)
			: DatetimeDuration(dateDuration, 0, 0, 0, 0, 0)
		{
		}

		DatetimeDuration::DatetimeDuration(const DateDuration & dateDuration, const TimeDuration & timeDuration)
			: DatetimeDuration(dateDuration, timeDuration.hour(), timeDuration.minute(), timeDuration.second(), timeDuration.millisecond(), timeDuration.microsecond())
		{
		}

		DatetimeDuration::DatetimeDuration(const TimeDuration & timeDuration)
			: DatetimeDuration(DateDuration(), timeDuration.hour(), timeDuration.minute(), timeDuration.second(), timeDuration.millisecond(), timeDuration.microsecond())
		{
		}

		DatetimeDuration DatetimeDuration::fromTimeDuration(const TimeDuration &timeDuration)
		{
			DatetimeDuration ret(timeDuration);
			return ret;
		}

		TimeDuration DatetimeDuration::toTimeDuration(void) const
		{
			TimeDuration ret(day() * HoursPerDay + hour(), minute(), second(), millisecond(), microsecond());
			return ret;
		}

		DatetimeDuration & DatetimeDuration::operator+=(const DatetimeDuration & duration)
		{
			DateDuration::operator+=(duration);
			m_hour += duration.hour();
			m_minute += duration.minute();
			m_second += duration.second();
			m_millisecond += duration.millisecond();
			m_microsecond += duration.microsecond();
			tidy();
			return *this;
		}

		DatetimeDuration & DatetimeDuration::operator-=(const DatetimeDuration & duration)
		{
			DateDuration::operator-=(duration);
			m_hour -= duration.hour();
			m_minute -= duration.minute();
			m_second -= duration.second();
			m_millisecond -= duration.millisecond();
			m_microsecond -= duration.microsecond();
			tidy();
			return *this;
		}

		DatetimeDuration DatetimeDuration::operator+(void) const
		{
			return *this;
		}

		DatetimeDuration DatetimeDuration::operator-(void) const
		{
			return DatetimeDuration(-DateDuration(*this), -hour(), -minute(), -second(), -millisecond(), -microsecond());
		}

		const int32 DatetimeDuration::hour(void) const
		{
			return m_hour;
		}

		void DatetimeDuration::setHour(const int32 hour)
		{
			m_hour = hour;
		}

		const int32 DatetimeDuration::minute(void) const
		{
			return m_minute;
		}

		void DatetimeDuration::setMinute(const int32 minute)
		{
			m_minute = minute;
		}

			const int32 DatetimeDuration::second(void) const
		{
			return m_second;
		}

		void DatetimeDuration::setSecond(const int32 second)
		{
			m_second = second;
		}

		const int32 DatetimeDuration::millisecond(void) const
		{
			return m_precision == Precision::MilliSecond || m_precision == Precision::MicroSecond ? m_millisecond : 0;
		}

		void DatetimeDuration::setMillisecond(const int32 millisecond)
		{
			m_millisecond = millisecond;
			if (m_precision == Precision::Second)
			{
				m_precision = Precision::MilliSecond;
			}
		}

		const int32 DatetimeDuration::microsecond(void) const
		{
			return m_precision == Precision::MicroSecond ? m_microsecond : 0;
		}

		void DatetimeDuration::setMicrosecond(const int32 microsecond)
		{
			m_microsecond = microsecond;
			if (m_precision != Precision::MicroSecond)
			{
				m_precision = Precision::MicroSecond;
			}
		}

		const Precision DatetimeDuration::precision(void) const
		{
			return m_precision;
		}

		void DatetimeDuration::setPrecision(const Precision precision)
		{
			m_precision = precision;
		}

		const int32 DatetimeDuration::fractionsecond(void) const
		{
			return (millisecond() * MicrosecondsPerMillisecond + microsecond()) * MicrosecondPerFractionSecond;
		}

		DatetimeDuration DatetimeDuration::fromString(const std::string & str)
		{
			static const std::string Tokens(" ");
			const auto parts(String::split(str, Tokens));

			if (parts.size() != 2)
			{
				return DatetimeDuration();
			}
			else
			{
				return DatetimeDuration(DateDuration::fromString(parts[0]), TimeDuration::fromString(parts[1]));
			}
		}

		std::string DatetimeDuration::toString(void) const
		{
			static const std::string Seperator(":");
			static const std::string FractionSeperator(".");

			std::ostringstream datatimeSout;
			datatimeSout << DateDuration::toString()
				<< hour() << Seperator << minute() << Seperator << second();
			if (precision() == Precision::MilliSecond || precision() == Precision::MicroSecond)
			{
				datatimeSout << FractionSeperator << millisecond();
			}
			if (precision() == Precision::MicroSecond)
			{
				datatimeSout << microsecond();
			}
			return datatimeSout.str();
		}

		void DatetimeDuration::tidy(void)
		{
			m_precision = m_microsecond != 0 ? Precision::MicroSecond
				: m_millisecond != 0 ? Precision::MilliSecond : Precision::Second;

			m_millisecond += m_microsecond / MicrosecondsPerMillisecond;
			m_microsecond = Math::mod(m_microsecond, MicrosecondsPerMillisecond);

			m_second += m_millisecond / MillisecondsPerSecond;
			m_millisecond = Math::mod(m_millisecond, MillisecondsPerSecond);

			m_minute += m_second / SecondsPerMinute;
			m_second = Math::mod(m_second, SecondsPerMinute);

			m_hour += m_minute / MinutesPerHour;
			m_minute = Math::mod(m_minute, MinutesPerHour);

			setDay(day() + m_hour / HoursPerDay);
			m_hour = Math::mod(m_hour, HoursPerDay);
		}

		Datetime getLocalDatetime(void)
		{
			using namespace boost::gregorian;
			using namespace boost::posix_time;

			ptime localDatetime(second_clock::local_time());
			const date &localDate(localDatetime.date());
			const time_duration &localTime(localDatetime.time_of_day());

			uint32 microseconds = static_cast<uint32>(localTime.fractional_seconds() / MicrosecondPerFractionSecond);
			return Datetime(static_cast<int16>(localDate.year()), static_cast<uint8>(localDate.month()), static_cast<uint8>(localDate.day()), static_cast<int32>(localTime.hours()), static_cast<uint8>(localTime.minutes()), static_cast<uint8>(localTime.seconds()), static_cast<uint16>(microseconds / MicrosecondsPerMillisecond), static_cast<uint16>(Math::mod(microseconds, MicrosecondsPerMillisecond)));
		}

		Datetime getDatetimeAfterLocalDatetime(const DatetimeDuration & duration)
		{
			return getLocalDatetime().getDatetimeAfter(duration);
		}

		Datetime getDatetimeBeforeLocalDatetime(const DatetimeDuration & duration)
		{
			return getLocalDatetime().getDatetimeBefore(duration);
		}
	};
};

const bool operator<(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() < rhs.day() ? true
		: lhs.day() > rhs.day() ? false
		: lhs.hour() < rhs.hour() ? true
		: lhs.hour() > rhs.hour() ? false
		: lhs.minute() < rhs.minute() ? true
		: lhs.minute() > rhs.minute() ? false
		: lhs.second() < rhs.second() ? true
		: lhs.second() > rhs.second() ? false
		: lhs.millisecond() < rhs.millisecond() ? true
		: lhs.millisecond() > rhs.millisecond() ? false
		: lhs.microsecond() < rhs.microsecond() ? true : false;
}

const bool operator<=(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() < rhs.day() ? true
		: lhs.day() > rhs.day() ? false
		: lhs.hour() < rhs.hour() ? true
		: lhs.hour() > rhs.hour() ? false
		: lhs.minute() < rhs.minute() ? true
		: lhs.minute() > rhs.minute() ? false
		: lhs.second() < rhs.second() ? true
		: lhs.second() > rhs.second() ? false
		: lhs.millisecond() < rhs.millisecond() ? true
		: lhs.millisecond() > rhs.millisecond() ? false
		: lhs.microsecond() <= rhs.microsecond() ? true : false;
}

const bool operator>(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	return !(lhs <= rhs);
}

const bool operator>=(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	return !(lhs < rhs);
}

const bool operator==(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	return lhs.year() == rhs.year() && lhs.month() == rhs.month() && lhs.day() == rhs.day() && lhs.hour() == rhs.hour() && lhs.minute() == rhs.minute() && lhs.second() == rhs.second() && lhs.millisecond() == rhs.millisecond() && lhs.microsecond() == rhs.microsecond();
}

const bool operator!=(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	return !(lhs == rhs);
}

const bool operator<(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() < rhs.day() ? true
		: lhs.day() > rhs.day() ? false
		: lhs.hour() < rhs.hour() ? true
		: lhs.hour() > rhs.hour() ? false
		: lhs.minute() < rhs.minute() ? true
		: lhs.minute() > rhs.minute() ? false
		: lhs.second() < rhs.second() ? true
		: lhs.second() > rhs.second() ? false
		: lhs.millisecond() < rhs.millisecond() ? true
		: lhs.millisecond() > rhs.millisecond() ? false
		: lhs.microsecond() < rhs.microsecond() ? true : false;
}

const bool operator<=(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() < rhs.day() ? true
		: lhs.day() > rhs.day() ? false
		: lhs.hour() < rhs.hour() ? true
		: lhs.hour() > rhs.hour() ? false
		: lhs.minute() < rhs.minute() ? true
		: lhs.minute() > rhs.minute() ? false
		: lhs.second() < rhs.second() ? true
		: lhs.second() > rhs.second() ? false
		: lhs.millisecond() < rhs.millisecond() ? true
		: lhs.millisecond() > rhs.millisecond() ? false
		: lhs.microsecond() <= rhs.microsecond() ? true : false;
}

const bool operator>(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return !(lhs <= rhs);
}

const bool operator>=(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return !(lhs < rhs);
}

const bool operator==(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return lhs.year() == rhs.year() && lhs.month() == rhs.month() && lhs.day() == rhs.day() && lhs.hour() == rhs.hour() && lhs.minute() == rhs.minute() && lhs.second() == rhs.second() && lhs.millisecond() == rhs.millisecond() && lhs.microsecond() == rhs.microsecond();
}

const bool operator!=(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return !(lhs == rhs);
}

const SSUtils::Datetime::Datetime operator+(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return lhs.getDatetimeAfter(rhs);
}

const SSUtils::Datetime::Datetime operator-(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	return lhs.getDatetimeBefore(rhs);
}

const SSUtils::Datetime::DatetimeDuration operator-(const SSUtils::Datetime::Datetime & lhs, const SSUtils::Datetime::Datetime & rhs)
{
	using namespace boost::gregorian;
	using namespace boost::posix_time;
	using namespace SSUtils;

	date lDate(lhs.year(), lhs.month(), lhs.day()), rDate(rhs.year(), rhs.month(), rhs.day());
	time_duration lTime(lhs.hour(), lhs.minute(), lhs.second(), lhs.fractionsecond());
	time_duration rTime(rhs.hour(), rhs.minute(), rhs.second(), rhs.fractionsecond());
	ptime lDatetime(lDate, lTime), rDatetime(rDate, rTime);
	
	auto disTime(lDatetime - rDatetime);

	return Datetime::DatetimeDuration(Datetime::TimeDuration(static_cast<int32>(disTime.hours()), static_cast<int32>(disTime.minutes()), static_cast<int32>(disTime.seconds())));
}

const SSUtils::Datetime::DatetimeDuration operator+(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	SSUtils::Datetime::DatetimeDuration ret(lhs);
	ret += rhs;
	return ret;
}

const SSUtils::Datetime::DatetimeDuration operator-(const SSUtils::Datetime::DatetimeDuration & lhs, const SSUtils::Datetime::DatetimeDuration & rhs)
{
	SSUtils::Datetime::DatetimeDuration ret(lhs);
	ret -= rhs;
	return ret;
}

std::ostream & operator<<(std::ostream & os, const SSUtils::Datetime::Datetime & datetime)
{
	os << datetime.toString();
	return os;
}

std::ostream & operator<<(std::ostream & os, const SSUtils::Datetime::DatetimeDuration & datetimeDuration)
{
	os << datetimeDuration.toString();
	return os;
}

#include "Time.h"
#include "Datetime.h"
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
		Time::Time(void)
			: Time(static_cast<int8>(0), static_cast<uint8>(0), static_cast<uint8>(0), static_cast<uint16>(0), static_cast<uint16>(0))
		{
		}

		Time::Time(const int32 second)
			: Time(second / SecondsPerHour, Math::mod(second / SecondsPerMinute, MinutesPerHour), Math::mod(second, SecondsPerMinute), static_cast<uint16>(0), static_cast<uint16>(0))
		{
		}

		Time::Time(const int32 hour, const uint8 minute, const uint8 second, const uint16 millisecond, const uint16 microsecond)
			: m_hour(hour), m_minute(minute), m_second(second), m_millisecond(millisecond), m_microsecond(microsecond), m_precision(Precision::Second)
		{
			m_precision = microsecond != 0 ? Precision::MicroSecond
				: millisecond != 0 ? Precision::MilliSecond : Precision::Second;
		}

		Time Time::fromMillisecond(const int32 millisecond)
		{
			int32 second(millisecond / MillisecondsPerSecond);
			Time ret(second);
			ret.m_millisecond = Math::mod(millisecond, MillisecondsPerSecond);
			ret.m_precision = Precision::MilliSecond;
			return ret;
		}

		Time Time::fromMicrosecond(const int32 microsecond)
		{
			int32 millisecond(microsecond / MicrosecondsPerMillisecond);
			Time ret(fromMillisecond(millisecond));
			ret.m_microsecond = Math::mod(microsecond, MicrosecondsPerMillisecond);
			ret.m_precision = Precision::MicroSecond;
			return ret;
		}

		Time & Time::operator+=(const TimeDuration & duration)
		{
			*this = this->getTimeAfter(duration);
			return *this;
		}

		Time & Time::operator-=(const TimeDuration & duration)
		{
			*this = this->getTimeBefore(duration);
			return *this;
		}

		Time Time::getTimeAfter(const TimeDuration & duration) const
		{
			Time ret;
			int32 microsecond(this->microsecond() + duration.microsecond());
			ret.m_microsecond = Math::mod(microsecond, MicrosecondsPerMillisecond);
			int32 millisecond(microsecond / MicrosecondsPerMillisecond + this->millisecond() + duration.millisecond());
			ret.m_millisecond = Math::mod(millisecond, MillisecondsPerSecond);
			int32 second(millisecond / MillisecondsPerSecond + this->second() + duration.second());
			ret.m_second = Math::mod(second, SecondsPerMinute);
			int32 minute(second / SecondsPerMinute + this->minute() + duration.minute());
			ret.m_minute = Math::mod(minute, MinutesPerHour);
			ret.m_hour = minute / MinutesPerHour + this->hour() + duration.hour();
			ret.m_precision = microsecond != 0 ? Precision::MicroSecond
				: millisecond != 0 ? Precision::MilliSecond : Precision::Second;
			return ret;
		}

		Time Time::getTimeBefore(const TimeDuration & duration) const
		{
			return getTimeAfter(-duration);
		}

		const int32 Time::hour(void) const
		{
			return m_hour;
		}

		void Time::setHour(const int32 hour)
		{
			m_hour = hour;
		}

		const uint8 Time::minute(void) const
		{
			return m_minute;
		}

		void Time::setMinute(const uint8 minute)
		{
			m_minute = minute;
		}

		const uint8 Time::second(void) const
		{
			return m_second;
		}

		void Time::setSecond(const uint8 second)
		{
			m_second = second;
		}

		const uint16 Time::millisecond(void) const
		{
			return m_precision == Precision::MilliSecond || m_precision == Precision::MicroSecond ? m_millisecond : 0;
		}

		void Time::setMillisecond(const uint16 millisecond)
		{
			m_millisecond = millisecond;
			if (m_precision == Precision::Second)
			{
				m_precision = Precision::MilliSecond;
			}
		}

		const uint16 Time::microsecond(void) const
		{
			return m_precision == Precision::MicroSecond ? m_microsecond : 0;
		}

		void Time::setMicrosecond(const uint16 microsecond)
		{
			m_microsecond = microsecond;
			if (m_precision != Precision::MicroSecond)
			{
				m_precision = Precision::MicroSecond;
			}
		}

		const Precision Time::precision(void) const
		{
			return m_precision;
		}

		void Time::setPrecision(const Precision precision)
		{
			m_precision = precision;
		}

		const uint32 Time::fractionsecond(void) const
		{
			return (static_cast<uint32>(millisecond()) * MicrosecondsPerMillisecond + microsecond()) * MicrosecondPerFractionSecond;
		}

		const int64 Time::totalMicroseconds(void) const
		{
			int64 milliseconds(totalMilliseconds());
			return milliseconds * MicrosecondsPerMillisecond + microsecond();
		}

		const int64 Time::totalMilliseconds(void) const
		{
			int32 seconds(totalSeconds());
			return seconds * MillisecondsPerSecond + millisecond();
		}

		const int32 Time::totalSeconds(void) const
		{
			return totalMinutes() * SecondsPerMinute + second();
		}

		const int32 Time::totalMinutes(void) const
		{
			return totalHours() * MinutesPerHour + minute();
		}

		const int32 Time::totalHours(void) const
		{
			return hour();
		}

		const int32 Time::totalDays(void) const
		{
			return hour() / HoursPerDay;
		}

		const bool Time::isTomorrow(void) const
		{
			return totalDays() == 1;
		}

		const bool Time::isTheDayAfterTomorrow(void) const
		{
			return totalDays() == 2;
		}

		const bool Time::isTheDaysAfter(void) const
		{
			return totalDays() >= 1;
		}

		const bool Time::isYesterday(void) const
		{
			return totalDays() == -1;
		}

		const bool Time::isTheDayBeforeYesterday(void) const
		{
			return totalDays() == -2;
		}

		const bool Time::isTheDaysBefore(void) const
		{
			return totalDays() <= -1;
		}

		Time Time::fromString(const std::string & str)
		{
			static const std::string Tokens(":.");
			auto numbers(String::split(str, Tokens));

			if (numbers.size() != 3 || numbers.size() != 4 ||
				std::find_if(numbers.cbegin(), numbers.cend(), [](const std::string &str)
			{
				return !String::isInteger(str);
			}) != numbers.cend())
			{
				return Time();
			}

			Time ret(std::stoi(numbers[0]) * SecondsPerHour + std::stoi(numbers[1]) * SecondsPerMinute + std::stoi(numbers[2]));
			if (numbers.size() == 4)
			{
				if (numbers[3].size() > 3)
				{
					for (uint32 i(0), j(FractionSecondDigits - static_cast<uint32>(numbers[3].size())); i != j; ++i)
					{
						numbers[3].push_back('0');
					}
				}
				uint32 fraction(std::stoul(numbers[3]));
				if (fraction > 1000)
				{
					ret.m_millisecond = fraction / MicrosecondsPerMillisecond;
					ret.m_microsecond = Math::mod(fraction, MicrosecondsPerMillisecond);
					ret.m_precision = Precision::MicroSecond;
				}
				else
				{
					ret.m_millisecond = fraction;
					ret.m_precision = Precision::MilliSecond;
				}
			}
			return ret;
		}

		std::string Time::toString(void) const
		{
			static const std::string Seperator(":");
			static const std::string FractionSeperator(".");

			std::ostringstream timeSout;
			timeSout << std::setfill('0') << hour() << Seperator
				<< std::setw(2) << static_cast<uint16>(minute()) << Seperator
				<< std::setw(2) << static_cast<uint16>(second());
			if (precision() == Precision::MilliSecond || precision() == Precision::MicroSecond)
			{
				timeSout << FractionSeperator << std::setfill('0') << std::setw(3) << millisecond();
			}
			if (precision() == Precision::MicroSecond)
			{
				timeSout << std::setfill('0') << std::setw(3) << microsecond();
			}
			return timeSout.str();
		}

		void Time::tidy(void)
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
		}

		TimeDuration::TimeDuration(void)
			: TimeDuration(0, 0, 0, 0, 0)
		{
		}

		TimeDuration::TimeDuration(const int32 second)
			: TimeDuration(second / SecondsPerHour, Math::mod(second / SecondsPerMinute, MinutesPerHour), Math::mod(second, SecondsPerMinute))
		{
		}

		TimeDuration::TimeDuration(const int32 hour, const int32 minute, const int32 second, const int32 millisecond, const int32 microsecond)
			: m_hour(hour), m_minute(minute), m_second(second), m_millisecond(millisecond), m_microsecond(microsecond)
		{
			tidy();
		}

		TimeDuration::TimeDuration(const DatetimeDuration & datetimeDuration)
			: TimeDuration(datetimeDuration.day() * HoursPerDay + datetimeDuration.hour(), datetimeDuration.minute(), datetimeDuration.second(), datetimeDuration.millisecond(), datetimeDuration.microsecond())
		{
		}

		TimeDuration TimeDuration::fromMillisecond(const int32 millisecond)
		{
			int32 second(millisecond / MillisecondsPerSecond);
			TimeDuration ret(second);
			ret.m_millisecond = Math::mod(millisecond, MillisecondsPerSecond);
			ret.m_precision = Precision::MilliSecond;
			return ret;
		}

		TimeDuration TimeDuration::fromMicrosecond(const int32 microsecond)
		{
			int32 millisecond(microsecond / MicrosecondsPerMillisecond);
			TimeDuration ret(fromMillisecond(millisecond));
			ret.m_microsecond = Math::mod(microsecond, MicrosecondsPerMillisecond);
			ret.m_precision = Precision::MicroSecond;
			return ret;
		}

		TimeDuration TimeDuration::fromDatetimeDuration(const DatetimeDuration & datetimeDuration)
		{
			return TimeDuration(datetimeDuration);
		}

		DatetimeDuration TimeDuration::toDatetimeDuration(void) const
		{
			return DatetimeDuration(*this);
		}

		TimeDuration & TimeDuration::operator+=(const TimeDuration & duration)
		{
			m_hour += duration.hour();
			m_minute += duration.minute();
			m_second += duration.second();
			m_millisecond += duration.millisecond();
			m_microsecond += duration.microsecond();
			tidy();
			return *this;
		}

		TimeDuration & TimeDuration::operator-=(const TimeDuration & duration)
		{
			m_hour -= duration.hour();
			m_minute -= duration.minute();
			m_second -= duration.second();
			m_millisecond -= duration.millisecond();
			m_microsecond -= duration.microsecond();
			tidy();
			return *this;
		}

		TimeDuration TimeDuration::operator+(void) const
		{
			return *this;
		}

		TimeDuration TimeDuration::operator-(void) const
		{
			return TimeDuration(-hour(), -minute(), -second(), -millisecond(), -microsecond());
		}

		const int32 TimeDuration::hour(void) const
		{ 
			return m_hour; 
		}

		void TimeDuration::setHour(const int32 hour)
		{ 
			m_hour = hour; 
		}

		const int32 TimeDuration::minute(void) const
		{
			return m_minute; 
		}

		void TimeDuration::setMinute(const int32 minute)
		{ 
			m_minute = minute; 
		}

		const int32 TimeDuration::second(void) const
		{ 
			return m_second; 
		}

		void TimeDuration::setSecond(const int32 second)
		{ 
			m_second = second; 
		}

		const int32 TimeDuration::millisecond(void) const
		{
			return m_precision == Precision::MilliSecond || m_precision == Precision::MicroSecond ? m_millisecond : 0;
		}

		void TimeDuration::setMillisecond(const int32 millisecond)
		{
			m_millisecond = millisecond;
			if (m_precision == Precision::Second)
			{
				m_precision = Precision::MilliSecond;
			}
		}

		const int32 TimeDuration::microsecond(void) const
		{
			return m_precision == Precision::MicroSecond ? m_microsecond : 0;
		}

		void TimeDuration::setMicrosecond(const int32 microsecond)
		{
			m_microsecond = microsecond;
			if (m_precision != Precision::MicroSecond)
			{
				m_precision = Precision::MicroSecond;
			}
		}

		const Precision TimeDuration::precision(void) const
		{ 
			return m_precision; 
		}

		void TimeDuration::setPrecision(const Precision precision)
		{
			m_precision = precision;
		}

		const int32 TimeDuration::fractionsecond(void) const
		{
			return (millisecond() * MicrosecondsPerMillisecond + microsecond()) * MicrosecondPerFractionSecond;
		}

		const int64 TimeDuration::totalMicroseconds(void) const
		{
			int64 milliseconds(totalMilliseconds());
			return milliseconds * MicrosecondsPerMillisecond + (precision() == Precision::MicroSecond ? microsecond() : 0);
		}

		const int64 TimeDuration::totalMilliseconds(void) const
		{
			int32 seconds(totalSeconds());
			return seconds * MillisecondsPerSecond + (precision() == Precision::Second ? 0 : millisecond());
		}

		const int32 TimeDuration::totalSeconds(void) const
		{
			return totalMinutes() * SecondsPerMinute + second();
		}

		const int32 TimeDuration::totalMinutes(void) const
		{
			return totalHours() * MinutesPerHour + minute();
		}

		const int32 TimeDuration::totalHours(void) const
		{
			return hour();
		}

		const int32 TimeDuration::totalDays(void) const
		{
			return hour() / HoursPerDay;
		}

		TimeDuration TimeDuration::fromString(const std::string & str)
		{
			static const std::string Tokens(":.");
			auto numbers(String::split(str, Tokens));

			if (numbers.size() != 3 || numbers.size() != 4 ||
				std::find_if(numbers.cbegin(), numbers.cend(), [](const std::string &str)
			{
				return !String::isInteger(str);
			}) != numbers.cend())
			{
				return TimeDuration();
			}

			TimeDuration ret(std::stoi(numbers[0]) * SecondsPerHour + std::stoi(numbers[1]) * SecondsPerMinute + std::stoi(numbers[2]));
			if (numbers.size() == 4)
			{
				if (numbers[3].size() > 3)
				{
					for (uint32 i(0), j(FractionSecondDigits - static_cast<uint32>(numbers[3].size())); i != j; ++i)
					{
						numbers[3].push_back('0');
					}
				}
				uint32 fraction(std::stoul(numbers[3]));
				if (fraction > 1000)
				{
					ret.m_millisecond = fraction / MicrosecondsPerMillisecond;
					ret.m_microsecond = Math::mod(fraction, MicrosecondsPerMillisecond);
					ret.m_precision = Precision::MicroSecond;
				}
				else
				{
					ret.m_millisecond = fraction;
					ret.m_precision = Precision::MilliSecond;
				}
			}
			return ret;
		}

		std::string TimeDuration::toString(void) const
		{
			static const std::string Seperator(":");
			static const std::string FractionSeperator(".");

			std::ostringstream timeSout;
			timeSout << hour() << Seperator << minute() << Seperator << second();
			if (precision() == Precision::MilliSecond || precision() == Precision::MicroSecond)
			{
				timeSout << FractionSeperator << millisecond();
			}
			if (precision() == Precision::MicroSecond)
			{
				timeSout << microsecond();
			}
			return timeSout.str();
		}

		void TimeDuration::tidy(void)
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
		}

		Time getLocalTime(void)
		{
			using namespace boost::posix_time;

			ptime localDatetime(second_clock::local_time());
			const time_duration &localTime(localDatetime.time_of_day());

			uint32 microseconds = static_cast<uint32>(localTime.fractional_seconds() / MicrosecondPerFractionSecond);
			return Time(static_cast<int32>(localTime.hours()), static_cast<uint8>(localTime.minutes()), static_cast<uint8>(localTime.seconds()), static_cast<uint16>(microseconds / MicrosecondsPerMillisecond), static_cast<uint16>(Math::mod(microseconds, MicrosecondsPerMillisecond)));
		}

		Time getTimeAfterLocalTime(const TimeDuration & duration)
		{
			return getLocalTime().getTimeAfter(duration);
		}

		Time getTimeBeforeLocalTime(const TimeDuration & duration)
		{
			return getLocalTime().getTimeBefore(duration);
		}
	};
};

const bool operator<(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	return lhs.totalMicroseconds() < rhs.totalMicroseconds();
}

const bool operator<=(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	return lhs.totalMicroseconds() <= rhs.totalMicroseconds();
}

const bool operator>(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	return lhs.totalMicroseconds() > rhs.totalMicroseconds();
}

const bool operator>=(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	return lhs.totalMicroseconds() >= rhs.totalMicroseconds();
}

const bool operator==(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	return lhs.totalMicroseconds() == rhs.totalMicroseconds();
}

const bool operator!=(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	return lhs.totalMicroseconds() != rhs.totalMicroseconds();
}

const bool operator<(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.totalMicroseconds() < rhs.totalMicroseconds();
}

const bool operator<=(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.totalMicroseconds() <= rhs.totalMicroseconds();
}

const bool operator>(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.totalMicroseconds() > rhs.totalMicroseconds();
}

const bool operator>=(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.totalMicroseconds() >= rhs.totalMicroseconds();
}

const bool operator==(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.totalMicroseconds() == rhs.totalMicroseconds();
}

const bool operator!=(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.totalMicroseconds() != rhs.totalMicroseconds();
}

const SSUtils::Datetime::Time operator+(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.getTimeAfter(rhs);
}

const SSUtils::Datetime::Time operator-(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	return lhs.getTimeBefore(rhs);
}

const SSUtils::Datetime::TimeDuration operator-(const SSUtils::Datetime::Time & lhs, const SSUtils::Datetime::Time & rhs)
{
	using namespace SSUtils;
	using namespace SSUtils::Datetime;

	static const auto dis([](const int32 lhs, const int32 rhs) { return lhs - rhs; });

	TimeDuration ret(dis(lhs.hour(), rhs.hour()), dis(lhs.minute(), rhs.minute()), dis(lhs.second(), rhs.second()), dis(lhs.millisecond(), rhs.millisecond()), dis(lhs.microsecond(), rhs.microsecond()));
	ret.tidy();
	return ret;
}

const SSUtils::Datetime::TimeDuration operator+(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	SSUtils::Datetime::TimeDuration ret(lhs);
	ret += rhs;
	return ret;
}

const SSUtils::Datetime::TimeDuration operator-(const SSUtils::Datetime::TimeDuration & lhs, const SSUtils::Datetime::TimeDuration & rhs)
{
	SSUtils::Datetime::TimeDuration ret(lhs);
	ret -= rhs;
	return ret;
}

std::ostream & operator<<(std::ostream & os, const SSUtils::Datetime::Time & time)
{
	os << time.toString();
	return os;
}

std::ostream & operator<<(std::ostream & os, const SSUtils::Datetime::TimeDuration & timeDuration)
{
	os << timeDuration.toString();
	return os;
}

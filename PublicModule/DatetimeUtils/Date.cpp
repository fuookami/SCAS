#include "Date.h"
#include "StringUtils.h"
#include <boost/date_time/gregorian/gregorian.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <sstream>
#include <iomanip>

namespace SSUtils
{
	namespace Datetime
	{
		const Date Date::EmptyDate;
		const DateDuration DateDuration::EmptyDateDuration;

		Date::Date(void)
			: Date(static_cast<int16>(0), static_cast<uint8>(0), static_cast<uint8>(0))
		{
		}

		Date::Date(const int16 year, const uint8 month, const uint8 day)
			: m_year(year), m_month(month), m_day(day)
		{
		}

		Date & Date::operator+=(const DateDuration & duration)
		{
			*this = this->getDateAfter(duration);
			return *this;
		}

		Date & Date::operator-=(const DateDuration & duration)
		{
			*this = this->getDateBefore(duration);
			return *this;
		}

		Date Date::getDateAfter(const DateDuration & duration) const
		{
			using namespace boost::gregorian;
			date targetDate(this->m_year, this->m_month, this->m_day);

			targetDate = duration.year() > 0 ? (targetDate + years(duration.year()))
				: m_year < 0 ? (targetDate - years(abs(duration.year()))) : targetDate;
			targetDate = duration.month() > 0 ? (targetDate + months(duration.month()))
				: m_month < 0 ? (targetDate - months(abs(duration.month()))) : targetDate;
			targetDate = duration.day() > 0 ? (targetDate + days(duration.day()))
				: m_day < 0 ? (targetDate - days(abs(duration.day()))) : targetDate;

			return Date(static_cast<int16>(targetDate.year()), static_cast<uint8>(targetDate.month()), static_cast<uint8>(targetDate.day()));
		}

		Date Date::getDateBefore(const DateDuration & duration) const
		{
			return getDateAfter(-duration);
		}

		uint16 Date::year(void) const
		{ 
			return m_year; 
		}

		void Date::setYear(const uint16 year)
		{ 
			m_year = year; 
		}

		uint8 Date::month(void) const
		{ 
			return m_month; 
		}

		void Date::setMonth(const uint8 month)
		{ 
			m_month = month; 
		}

		uint8 Date::day(void) const
		{ 
			return m_day; 
		}

		void Date::setDay(const uint8 day)
		{ 
			m_day = day; 
		}

		const bool Date::isLegalDate(void) const
		{
			if (year() == 0 || month() == 0 || day() == 0)
			{
				return false;
			}

			if (month() > 12 || day() > DaysOfMonth[month() - 1])
			{
				return false;
			}

			return true;
		}

		const bool Date::isLeapYear(void) const
		{
			return ::SSUtils::Datetime::isLeapYear(year());
		}

		const uint8 Date::getDayOfMonth(void) const
		{
			return ::SSUtils::Datetime::getDaysOfMonth(year(), month());
		}

		const uint8 Date::getDayInWeek(void) const
		{
			return ::SSUtils::Datetime::getDayInWeek(year(), month(), day());
		}

		Date Date::fromString(const std::string & str)
		{
			static const std::string Tokens("-");
			const auto numbers(String::split(str, Tokens));

			if (numbers.size() != 3 ||
				std::find_if(numbers.cbegin(), numbers.cend(), [](const std::string &str)
			{
				return !String::isInteger(str);
			}) != numbers.cend())
			{
				return EmptyDate;
			}

			Date ret(static_cast<uint16>(std::stoi(numbers[0])), static_cast<uint8>(std::stoul(numbers[1])), static_cast<uint8>(std::stoul(numbers[2])));
			return ret.isLegalDate() ? ret : EmptyDate;
		}

		std::string Date::toString(void) const
		{
			static const std::string Seperator("-");

			std::ostringstream dataSout;
			dataSout << std::setfill('0') << year() << Seperator
				<< std::setw(2) << (uint16)month() << Seperator
				<< std::setw(2) << (uint16)day() << std::setfill(' ');

			return dataSout.str();
		}

		DateDuration::DateDuration(void)
			: DateDuration(0, 0, 0)
		{
		}

		DateDuration::DateDuration(const int32 day)
			: DateDuration(0, 0, day)
		{
		}

		DateDuration::DateDuration(const int32 month, const int32 day)
			: DateDuration(0, month, day)
		{
		}

		DateDuration::DateDuration(const int32 year, const int32 month, const int32 day)
			: m_year(year), m_month(month), m_day(day)
		{
		}

		DateDuration & DateDuration::operator+=(const DateDuration & duration)
		{
			m_year += duration.year();
			m_month += duration.month();
			m_day += duration.day();
			return *this;
		}

		DateDuration & DateDuration::operator-=(const DateDuration & duration)
		{
			m_year -= duration.year();
			m_month -= duration.month();
			m_day -= duration.day();
			return *this;
		}

		DateDuration DateDuration::operator+(void) const
		{
			return *this;
		}

		DateDuration DateDuration::operator-(void) const
		{
			return DateDuration(-year(), -month(), -day());
		}

		int32 DateDuration::year(void) const
		{ 
			return m_year; 
		}

		void DateDuration::setYear(const int32 year)
		{ 
			m_year = year; 
		}

		int32 DateDuration::month(void) const
		{ 
			return m_month; 
		}

		void DateDuration::setMonth(const int32 month)
		{ 
			m_month = month; 
		}

		int32 DateDuration::day(void) const
		{ 
			return m_day; 
		}

		void DateDuration::setDay(const int32 day)
		{ 
			m_day = day; 
		}

		DateDuration DateDuration::fromString(const std::string & str)
		{
			static const std::string Tokens("-");
			const auto numbers(String::split(str, Tokens));

			if (numbers.size() != 3 ||
				std::find_if(numbers.cbegin(), numbers.cend(), [](const std::string &str)
			{
				return !String::isInteger(str);
			}) != numbers.cend())
			{
				return EmptyDateDuration;
			}

			return DateDuration(std::stoi(numbers[0]), std::stoul(numbers[1]), std::stoul(numbers[2]));
		}

		std::string DateDuration::toString(void) const
		{
			static const std::string Seperator("-");

			std::ostringstream dataSout;
			dataSout << year() << Seperator << month() << Seperator << day() << std::setfill(' ');

			return dataSout.str();
		}

		Date getLocalDate(void)
		{
			using namespace boost::gregorian;

			date localDate(day_clock::local_day());

			return Date(static_cast<int16>(localDate.year()), static_cast<uint8>(localDate.month()), static_cast<uint8>(localDate.day()));
		}

		Date getDateAfterLocalDate(const DateDuration & duration)
		{
			return getLocalDate().getDateAfter(duration);
		}

		Date getDateBeforeLocalDate(const DateDuration & duration)
		{
			return getLocalDate().getDateBefore(duration);
		}

		const bool isLeapYear(const int16 year)
		{
			return year % 4 == 0 && year % 100 != 0 ? true
				: year % 100 == 0 && year % 400 == 0 ? true : false;
		}

		const bool isLeapYear(const Date & date)
		{
			return isLeapYear(date.year());
		}

		const uint8 getDaysOfMonth(const int16 year, const uint8 month)
		{
			if (month == 2)
			{
				return isLeapYear(year) ? 29 : 28;
			}
			else
			{
				return DaysOfMonth[month - 1];
			}
		}

		const uint8 getDaysOfMonth(const Date & date)
		{
			return getDaysOfMonth(date.year(), date.month());
		}

		const uint8 getDayInWeek(int16 year, uint8 month, const uint8 day)
		{
			if (month == 1 || month == 2)
			{
				month += 12;
				year += 1;
			}

			unsigned char c = year / 100;
			unsigned char y = year % 100;

			return (c / 4 - 2 * c + y + y / 4 + 26 * (month + 1) / 10 + day - 1) % 7;
		}

		const uint8 getDayInWeek(const Date & date)
		{
			return getDayInWeek(date.year(), date.month(), date.day());
		}
	};
};

const bool operator<(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() < rhs.day() ? true : false;
}

const bool operator<=(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() <= rhs.day() ? true : false;
}

const bool operator>(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	return !(lhs <= rhs);
}

const bool operator>=(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	return !(lhs < rhs);
}

const bool operator==(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	return lhs.year() == rhs.year() && lhs.month() == rhs.month() && lhs.day() == rhs.day();
}

const bool operator!=(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	return !(lhs == rhs);
}

const bool operator<(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() < rhs.day() ? true : false;
}

const bool operator<=(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return lhs.year() < rhs.year() ? true
		: lhs.year() > rhs.year() ? false
		: lhs.month() < rhs.month() ? true
		: lhs.month() > rhs.month() ? false
		: lhs.day() <= rhs.day() ? true : false;
}

const bool operator>(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return !(lhs <= rhs);
}

const bool operator>=(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return !(lhs < rhs);
}

const bool operator==(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return lhs.year() == rhs.year() && lhs.month() == rhs.month() && lhs.day() == rhs.day();
}

const bool operator!=(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return !(lhs == rhs);
}

const SSUtils::Datetime::Date operator+(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return lhs.getDateAfter(rhs);
}

const SSUtils::Datetime::Date operator-(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return lhs.getDateBefore(rhs);
}

const SSUtils::Datetime::DateDuration operator-(const SSUtils::Datetime::Date & lhs, const SSUtils::Datetime::Date & rhs)
{
	using namespace boost::gregorian;

	date lDate(lhs.year(), lhs.month(), lhs.day()), rDate(rhs.year(), rhs.month(), rhs.day());

	return SSUtils::Datetime::DateDuration(static_cast<SSUtils::int32>((lDate - rDate).days()));
}

const SSUtils::Datetime::DateDuration operator+(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return SSUtils::Datetime::DateDuration(lhs.year() + rhs.year(), lhs.month() + rhs.month(), lhs.day() + rhs.day());
}

const SSUtils::Datetime::DateDuration operator-(const SSUtils::Datetime::DateDuration & lhs, const SSUtils::Datetime::DateDuration & rhs)
{
	return SSUtils::Datetime::DateDuration(lhs.year() - rhs.year(), lhs.month() - rhs.month(), lhs.day() - rhs.day());
}

std::ostream & operator<<(std::ostream & os, const SSUtils::Datetime::Date & date)
{
	os << date.toString();
	return os;
}

std::ostream & operator<<(std::ostream & os, const SSUtils::Datetime::DateDuration & dateDuration)
{
	os << dateDuration.toString();
	return os;
}

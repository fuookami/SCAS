#include "DateTimeUtils.h"

#include <boost/date_time/gregorian/gregorian.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <sstream>
#include <iomanip>
#include <algorithm>

namespace DateTimeUtils
{
	Date::Date(void)
		: Date(getLocalDate())
	{
	}

	Date::Date(const short _year, const unsigned char _month, const unsigned char _day)
		: year(_year), month(_month), day(_day)
	{
	}

	Date::Date(const Date & ano)
		: year(ano.year), month(ano.month), day(ano.day)
	{
	}

	Date::Date(const Date && ano)
		: year(ano.year), month(ano.month), day(ano.day)
	{
	}

	Date & Date::operator=(const Date & rhs)
	{
		year = rhs.year;
		month = rhs.month;
		day = rhs.day;

		return *this;
	}

	Date & Date::operator=(const Date && rhs)
	{
		year = rhs.year;
		month = rhs.month;
		day = rhs.day;

		return *this;
	}

	Date::~Date(void)
	{
	}

	Date Date::getDateAfter(const int year, const int month, const int day) const
	{
		using namespace boost::gregorian;
		date targetDate(this->year, this->month, this->day);

		targetDate = year > 0 ? (targetDate + years(year))
			: year < 0 ? (targetDate - years(abs(year))) : targetDate;
		targetDate = month > 0 ? (targetDate + months(month))
			: month < 0 ? (targetDate - months(abs(month))) : targetDate;
		targetDate = day > 0 ? (targetDate + days(day))
			: day < 0 ? (targetDate - days(abs(day))) : targetDate;

		return Date(targetDate.year(), targetDate.month(), targetDate.day());
	}

	Date Date::getDateAfter(const Date & date) const
	{
		return this->getDateAfter(date.year, date.month, date.day);
	}

	Date Date::getDayAfter(const int day) const
	{
		return this->getDateAfter(0, 0, day);
	}

	Date Date::getDateBefore(const Date & date) const
	{
		return this->getDateAfter(-date.year, -date.month, -date.day);
	}

	std::string Date::toString() const
	{
		std::ostringstream dataSout;
		dataSout << std::setfill('0') << year << "-"
			<< std::setw(2) << (unsigned short)month << "-"
			<< std::setw(2) << (unsigned short)day << std::setfill(' ');

		return dataSout.str();
	}

	TimeDuration::TimeDuration(void)
		: hour(0), min(0), sec(0)
	{
	}

	TimeDuration::TimeDuration(const unsigned long long seconds)
		: hour(seconds / 3600), min(seconds % 3600 / 60), sec(seconds % 60)
	{
	}

	TimeDuration::TimeDuration(const unsigned short _hour, const unsigned char _min, const unsigned char _sec)
		: hour(_hour), min(_min), sec(_sec)
	{
	}

	TimeDuration::TimeDuration(const TimeDuration & ano)
		: hour(ano.hour), min(ano.min), sec(ano.sec)
	{
	}

	TimeDuration::TimeDuration(const TimeDuration && ano)
		: hour(ano.hour), min(ano.min), sec(ano.sec)
	{
	}

	TimeDuration & TimeDuration::operator=(const TimeDuration & rhs)
	{
		hour = rhs.hour;
		min = rhs.min;
		sec = rhs.sec;

		return *this;
	}

	TimeDuration & TimeDuration::operator=(const TimeDuration && rhs)
	{
		hour = rhs.hour;
		min = rhs.min;
		sec = rhs.sec;

		return *this;
	}

	TimeDuration::~TimeDuration(void)
	{
	}

	TimeDuration TimeDuration::getTimeDurationAfter(const int hour, const int min, const int sec) const
	{
		using namespace boost::posix_time;

		time_duration targetTime(this->hour, this->min, this->sec);
		
		targetTime = hour > 0 ? (targetTime + hours(hour))
			: hour < 0 ? (targetTime - hours(abs(hour))) : targetTime;
		targetTime = min > 0 ? (targetTime + minutes(min))
			: min < 0 ? (targetTime - minutes(abs(min))) : targetTime;
		targetTime = sec > 0 ? (targetTime + seconds(sec))
			: sec < 0 ? (targetTime - seconds(abs(sec))) : targetTime;

		return TimeDuration(targetTime.hours(), targetTime.minutes(), targetTime.seconds());
	}

	TimeDuration TimeDuration::getTimeDurationAfter(const TimeDuration & time) const
	{
		return this->getTimeDurationAfter(time.hour, time.min, time.sec);
	}

	TimeDuration TimeDuration::getTimeDurationAfter(const int second) const
	{
		return this->getTimeDurationAfter(0, 0, second);
	}

	TimeDuration TimeDuration::getTimeDurationBefore(const TimeDuration & time) const
	{
		return this->getTimeDurationAfter(-time.hour, -time.min, -time.sec);
	}

	int TimeDuration::totalSeconds(void) const
	{
		return hour * 3600 + min * 60 + sec;
	}

	int TimeDuration::totalDays(void) const
	{
		return hour / 24;
	}

	std::string TimeDuration::toString(void) const
	{
		std::ostringstream timeSout;
		timeSout << std::setfill('0') << hour << ":"
			<< std::setw(2) << (unsigned short)min << ":"
			<< std::setw(2) << (unsigned short)sec << std::setfill(' ');

		return timeSout.str();
	}

	Datetime::Datetime(void)
		: Datetime(getLocalDatetime())
	{
	}

	Datetime::Datetime(const Date & date, const unsigned char _hour, const unsigned char _min, const unsigned char _sec)
		: year(date.year), month(date.month), day(date.day), hour(_hour), min(_min), sec(_sec)
	{
	}

	Datetime::Datetime(const Date && date, const unsigned char _hour, const unsigned char _min, const unsigned char _sec)
		: year(date.year), month(date.month), day(date.day), hour(_hour), min(_min), sec(_sec)
	{
	}

	Datetime::Datetime(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour, const unsigned char _min, const unsigned char _sec)
		: year(_year), month(_month), day(_day), hour(_hour), min(_min), sec(_sec)
	{
	}

	Datetime::Datetime(const Datetime & ano)
		: year(ano.year), month(ano.month), day(ano.day), hour(ano.hour), min(ano.min), sec(ano.sec)
	{
	}

	Datetime::Datetime(const Datetime && ano)
		: year(ano.year), month(ano.month), day(ano.day), hour(ano.hour), min(ano.min), sec(ano.sec)
	{
	}

	Datetime & Datetime::operator=(const Datetime & rhs)
	{
		year = rhs.year;
		month = rhs.month;
		day = rhs.day;
		hour = rhs.hour;
		min = rhs.min;
		sec = rhs.sec;

		return *this;
	}

	Datetime & Datetime::operator=(const Datetime && rhs)
	{
		year = rhs.year;
		month = rhs.month;
		day = rhs.day;
		hour = rhs.hour;
		min = rhs.min;
		sec = rhs.sec;

		return *this;
	}

	Datetime::~Datetime(void)
	{
	}

	Datetime Datetime::getDatetimeAfter(const int year, const int month, const int day, const int hour, const int min, const int sec) const
	{
		using namespace boost::gregorian;
		using namespace boost::posix_time;

		date currDate(this->year, this->month, this->day);
		time_duration currTime(this->hour, this->min, this->sec);
		ptime targetDateTime(currDate, currTime);

		targetDateTime = year > 0 ? (targetDateTime + years(year))
			: year < 0 ? (targetDateTime - years(abs(year))) : targetDateTime;
		targetDateTime = month > 0 ? (targetDateTime + months(month))
			: month < 0 ? (targetDateTime - months(abs(month))) : targetDateTime;
		targetDateTime = day > 0 ? (targetDateTime + days(day))
			: day < 0 ? (targetDateTime - days(abs(day))) : targetDateTime;
		targetDateTime = hour > 0 ? (targetDateTime + hours(hour))
			: hour < 0 ? (targetDateTime - hours(abs(hour))) : targetDateTime;
		targetDateTime = min > 0 ? (targetDateTime + minutes(min))
			: min < 0 ? (targetDateTime - minutes(abs(min))) : targetDateTime;
		targetDateTime = sec > 0 ? (targetDateTime + seconds(sec))
			: sec < 0 ? (targetDateTime - seconds(abs(sec))) : targetDateTime;

		const date &targetDate(targetDateTime.date());
		const time_duration &targetTime(targetDateTime.time_of_day());

		return Datetime(targetDate.year(), targetDate.month(), targetDate.day(),
			static_cast<unsigned char>(targetTime.hours()), static_cast<unsigned char>(targetTime.minutes()),  static_cast<unsigned char>(targetTime.seconds()));
	}

	Datetime Datetime::getDatetimeAfter(const Datetime & datetime) const
	{
		return this->getDatetimeAfter(datetime.year, datetime.month, datetime.day, datetime.hour, datetime.min, datetime.sec);
	}

	Datetime Datetime::getDateAfter(const Date & date) const
	{
		return this->getDatetimeAfter(date.year, date.month, date.day, 0, 0, 0);
	}

	Datetime Datetime::getDayAfter(const int day) const
	{
		return this->getDatetimeAfter(0, 0, day, 0, 0, 0);
	}

	Datetime Datetime::getTimeAfter(const TimeDuration & time) const
	{
		return this->getDatetimeAfter(0, 0, 0, time.hour, time.min, time.sec);
	}

	Datetime Datetime::getDatetimeBefore(const Datetime & datetime) const
	{
		return this->getDatetimeAfter(-datetime.year, -datetime.month, -datetime.day, -datetime.hour, -datetime.min, -datetime.sec);
	}

	Datetime Datetime::getDateBefore(const Date & date) const
	{
		return this->getDatetimeAfter(-date.year, -date.month, -date.day, 0, 0, 0);
	}

	Datetime Datetime::getTimeBefore(const TimeDuration & time) const
	{
		return this->getDatetimeAfter(0, 0, 0, -time.hour, -time.min, -time.sec);
	}

	std::string Datetime::toString() const
	{
		std::ostringstream datetimeSout;
		datetimeSout << std::setfill('0') << year << "-"
			<< std::setw(2) << (unsigned short)month << "-"
			<< std::setw(2) << (unsigned short)day << " "
			<< std::setw(2) << (unsigned short)hour << ":"
			<< std::setw(2) << (unsigned short)min << ":"
			<< std::setw(2) << (unsigned short)sec << std::setfill(' ');

		return datetimeSout.str();
	}

	Date getLocalDate(void)
	{
		using namespace boost::gregorian;

		date localDate(day_clock::local_day());

		return Date({ static_cast<short>(localDate.year()), static_cast<unsigned char>(localDate.month()), static_cast<unsigned char>(localDate.day()) });
	}

	Date getDateAfterLocalDate(const int year, const int month, const int day)
	{
		return getLocalDate().getDateAfter(year, month, day);
	}

	Date getDateAfterLocalDate(const Date & date)
	{
		return getLocalDate().getDateAfter(date);
	}

	Date getDateAfterLocalDate(const int day)
	{
		return getLocalDate().getDayAfter(day);
	}

	Date getDateBeforeLocalDate(const Date & date)
	{
		return getLocalDate().getDateBefore(date);
	}

	Datetime getLocalDatetime(void)
	{
		using namespace boost::gregorian;
		using namespace boost::posix_time;

		ptime localDateTime(second_clock::local_time());
		const date &localDate(localDateTime.date());
		const time_duration &localTime(localDateTime.time_of_day());

		return Datetime(localDate.year(), localDate.month(), localDate.day(),
		static_cast<unsigned char>(localTime.hours()), static_cast<unsigned char>(localTime.minutes()),  static_cast<unsigned char>(localTime.seconds()));
	}

	Datetime getDatetimeAfterLocalDatetime(const int year, const int month, const int day, const int hour, const int min, const int sec)
	{
		return getLocalDatetime().getDatetimeAfter(year, month, day, hour, min, sec);
	}

	Datetime getDatetimeAfterLocalDatetime(const Datetime & datetime)
	{
		return getLocalDatetime().getDatetimeAfter(datetime);
	}

	Datetime getDatetimeAfterLocalDatetime(const Date & date)
	{
		return getLocalDatetime().getDateAfter(date);
	}

	Datetime getDatetimeAfterLocalDatetime(const int day)
	{
		return getLocalDatetime().getDayAfter(day);
	}

	Datetime getDatetimeAfterLocalDatetime(const TimeDuration & time)
	{
		return getLocalDatetime().getTimeAfter(time);
	}

	Datetime getDatetimeBeforeLocalDatetime(const Datetime & datetime)
	{
		return getLocalDatetime().getDatetimeBefore(datetime);
	}

	Datetime getDatetimeBeforeLocalDatetime(const Date & date)
	{
		return getLocalDatetime().getDateBefore(date);
	}

	Datetime getDatetimeBeforeLocalDatetime(const TimeDuration & time)
	{
		return getLocalDatetime().getTimeBefore(time);
	}

	bool isLeapYear(const unsigned char year)
	{
		return year % 4 == 0 && year % 100 != 0 ? true
			: year % 100 == 0 && year % 400 == 0 ? true : false;
	}

	unsigned char getDayInWeek(unsigned short year, unsigned char month, const unsigned char day)
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

	unsigned char getDayInWeek(const Date & date)
	{
		return getDayInWeek(date.year, date.month, date.day);
	}

	unsigned char getDayInWeek(const Datetime & datetime)
	{
		return getDayInWeek(datetime.year, datetime.month, datetime.day);
	}

	unsigned char getDaysOfMonth(const unsigned short year, const unsigned char month)
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

	unsigned char getDaysOfMonth(const Date & date)
	{
		return getDaysOfMonth(date.year, date.month);
	}

	unsigned char getDaysOfMonth(const Datetime & datetime)
	{
		return getDaysOfMonth(datetime.year, datetime.month);
	}

	Datetime getBuildDateTime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString)
	{
		static const auto fromShortMonthName(
			[](const std::string &shortName) -> unsigned char 
		{
			return std::find(MonthShortName.cbegin(), MonthShortName.cend(), shortName) - MonthShortName.cbegin() + 1;
		});


		using namespace boost::gregorian;
		using namespace boost::posix_time;

		std::string BuildYear, BuileMonth, BuildDay;
		std::istringstream sin(BuildOriginalDateString);
		sin >> BuileMonth >> BuildDay >> BuildYear;

		std::string BuildHour(BuildTimeString.cbegin(), BuildTimeString.cbegin() + 2);
		std::string BuildMinute(BuildTimeString.cbegin() + 3, BuildTimeString.cbegin() + 5);
		std::string BuildSecond(BuildTimeString.cbegin() + 6, BuildTimeString.cbegin() + 8);

		return Datetime(static_cast<short>(std::stoi(BuildYear)), fromShortMonthName(BuileMonth), static_cast<unsigned char>(std::stoi(BuildDay)),
			static_cast<unsigned char>(std::stoi(BuildHour)), static_cast<unsigned char>(std::stoi(BuildMinute)), static_cast<unsigned char>(std::stoi(BuildSecond)));
	}
};

const bool operator<(const DateTimeUtils::Date & lhs, const DateTimeUtils::Date & rhs)
{
	return lhs.year < rhs.year ? true
		: lhs.year > rhs.year ? false
		: lhs.month < rhs.month ? true
		: lhs.month > rhs.month ? false
		: lhs.day < rhs.day ? true : false;
}

const bool operator<(const DateTimeUtils::TimeDuration & lhs, const DateTimeUtils::TimeDuration & rhs)
{
	return lhs.hour < rhs.hour ? true
		: lhs.hour > rhs.hour ? false
		: lhs.min < rhs.min ? true
		: lhs.min > rhs.min ? false
		: lhs.sec < rhs.sec ? true : false;
}

const bool operator<(const DateTimeUtils::Datetime & lhs, const DateTimeUtils::Datetime & rhs)
{
	return lhs.year < rhs.year ? true
		: lhs.year > rhs.year ? false
		: lhs.month < rhs.month ? true
		: lhs.month > rhs.month ? false
		: lhs.day < rhs.day ? true 
		: lhs.day > rhs.day ? false
		: lhs.hour < rhs.hour ? true
		: lhs.hour > rhs.hour ? false
		: lhs.min < rhs.min ? true
		: lhs.min > rhs.min ? false
		: lhs.sec < rhs.sec ? true : false;
}

const int operator-(const DateTimeUtils::Date & lhs, const DateTimeUtils::Date & rhs)
{
	using namespace boost::gregorian;

	date lDate(lhs.year, lhs.month, lhs.day), rDate(rhs.year, rhs.month, rhs.day);

	return (lDate - rDate).days();
}

const int operator-(const DateTimeUtils::TimeDuration & lhs, const DateTimeUtils::TimeDuration & rhs)
{
	using namespace boost::posix_time;

	time_duration lTime(lhs.hour, lhs.min, lhs.sec), rTime(rhs.hour, rhs.min, rhs.sec);
	time_duration disTime(lTime - rTime);

	return DateTimeUtils::TimeDuration(disTime.hours(), disTime.minutes(), disTime.seconds()).totalSeconds();
}

const DateTimeUtils::TimeDuration operator-(const DateTimeUtils::Datetime & lhs, const DateTimeUtils::Datetime & rhs)
{
	using namespace boost::gregorian;
	using namespace boost::posix_time;

	date lDate(lhs.year, lhs.month, lhs.day), rDate(rhs.year, rhs.month, rhs.day);
	time_duration lTime(lhs.hour, lhs.min, lhs.sec), rTime(rhs.hour, rhs.min, rhs.sec);
	ptime lDateTime(lDate, lTime), rDateTime(rDate, rTime);

	auto disTime(rhs < lhs ? (lDateTime - rDateTime) : (rDateTime - lDateTime));

	return DateTimeUtils::TimeDuration(disTime.hours(), disTime.minutes(), disTime.seconds());
}

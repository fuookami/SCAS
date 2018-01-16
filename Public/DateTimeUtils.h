#pragma once

#include <string>
#include <array>
#include <tuple>

namespace DateTimeUtils
{
	static const std::array<unsigned char, 12> DaysOfMonth = 
	{
		31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
	};

	static const std::array<std::string, 12> MonthShortName = 
	{
		std::string("Jan"), std::string("Feb"), std::string("Mar"),
		std::string("Apr"), std::string("May"), std::string("Jun"),
		std::string("Jul"), std::string("Aug"), std::string("Sep"),
		std::string("Oct"), std::string("Nov"), std::string("Dec")
	};

	static const std::array<std::string, 7> DayInWeekChineseName =
	{
		std::string("星期日"), std::string("星期一"), std::string("星期二"),
		std::string("星期三"), std::string("星期四"), std::string("星期五"),
		std::string("星期六")
	};

	struct Date
	{
		short year;
		unsigned char month;
		unsigned char day;

		Date(void);
		Date(const short _year, const unsigned char _month, const unsigned char _day);
		Date(const Date &ano);
		Date(const Date &&ano);
		Date &operator=(const Date &rhs);
		Date &operator=(const Date &&rhs);
		~Date(void);

		Date getDateAfter(const int year, const int month, const int day) const;
		Date getDateAfter(const Date &date) const;
		Date getDayAfter(const int day) const;

		Date getDateBefore(const Date &date) const;

		std::string toString(void) const;
	};

	struct Time
	{
		unsigned short hour;
		unsigned char min;
		unsigned char sec;

		Time(void);
		Time(const unsigned int seconds);
		Time(const unsigned short _hour, const unsigned char _min, const unsigned char _sec);
		Time(const TimeMs &ano);
		Time(const TimeMs &&ano);
		Time(const Time &ano);
		Time(const Time &&ano);
		Time &operator=(const Time &rhs);
		Time &operator=(const Time &&rhs);
		Time &operator=(const TimeMs &rhs);
		Time &operator=(const TimeMs &&rhs);
		~Time(void);

		Time getTimeAfter(const int hour, const int min, const int sec) const;
		Time getTimeAfter(const Time &time) const;
		Time getTimeAfter(const int second) const;
		Time getTimeBefore(const Time &time) const;

		int totalSeconds(void) const;
		int totalMinutes(void) const;
		int totalHours(void) const;
		int totalDays(void) const;
		std::string toString(void) const;
	};

	struct TimeMs
	{
		unsigned short hour;
		unsigned char min;
		unsigned char sec;
		unsigned short msec;

		TimeMs(void);
		TimeMs(const unsigned int milliseconds);
		TimeMs(const unsigned short _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec = 0);
		TimeMs(const Time &ano);
		TimeMs(const Time &&ano);
		TimeMs(const TimeMs &ano);
		TimeMs(const TimeMs &&ano);
		TimeMs &operator=(const Time &rhs);
		TimeMs &operator=(const Time &&rhs);
		TimeMs &operator=(const TimeMs &rhs);
		TimeMs &operator=(const TimeMs &&rhs);
		~TimeMs(void);

		TimeMs getTimeAfter(const int hour, const int min, const int sec, const int msec) const;
		TimeMs getTimeAfter(const Time &time) const;
		TimeMs getTimeAfter(const TimeMs &time) const;
		TimeMs getTimeAfter(const int msecond) const;
		TimeMs getTimeBefore(const Time &time) const;
		TimeMs getTimeBefore(const TimeMs &time) const;

		int totalMilliseconds(void) const;
		int totalSeconds(void) const;
		int totalMinutes(void) const;
		int totalHours(void) const;
		int totalDays(void) const;
	};

	struct DateTime
	{
		short year;
		unsigned char month;
		unsigned char day;
		unsigned char hour;
		unsigned char min;
		unsigned char sec;

		DateTime(void);
		DateTime(const Date &date, const Time &time);
		DateTime(const Date &&date, const Time &time);
		DateTime(const Date &date, const Time &&time);
		DateTime(const Date &&date, const Time &&time);
		DateTime(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		DateTime(const Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		DateTime(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		DateTime(const DateTime &ano);
		DateTime(const DateTime &&ano);
		DateTime(const DateTimeMs &ano);
		DateTime(const DateTimeMs &&ano);
		DateTime &operator=(const DateTime &rhs);
		DateTime &operator=(const DateTime &&rhs);
		DateTime &operator=(const DateTimeMs &rhs);
		DateTime &operator=(const DateTimeMs &&rhs);
		~DateTime(void);

		DateTime getDatetimeAfter(const int year, const int month, const int day, const int hour, const int min, const int sec) const;
		DateTime getDatetimeAfter(const DateTime &datetime) const;
		DateTime getDateAfter(const Date &date) const;
		DateTime getDayAfter(const int day) const;
		DateTime getTimeAfter(const Time &time) const;

		DateTime getDatetimeBefore(const DateTime &datetime) const;
		DateTime getDateBefore(const Date &date) const;
		DateTime getTimeBefore(const Time &time) const;

		std::string toString(void) const;
	};

	struct DateTimeMs
	{
		short year;
		unsigned char month;
		unsigned char day;
		unsigned char hour;
		unsigned char min;
		unsigned char sec;
		unsigned short msec;

		DateTimeMs(void);
		DateTimeMs(const Date &date, const Time &time);
		DateTimeMs(const Date &&date, const Time &time);
		DateTimeMs(const Date &date, const Time &&time);
		DateTimeMs(const Date &&date, const Time &&time);
		DateTimeMs(const Date &date, const TimeMs &time);
		DateTimeMs(const Date &&date, const TimeMs &time);
		DateTimeMs(const Date &date, const TimeMs &&time);
		DateTimeMs(const Date &&date, const TimeMs &&time);
		DateTimeMs(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DateTimeMs(const Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DateTimeMs(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DateTimeMs(const DateTime &ano);
		DateTimeMs(const DateTime &&ano);
		DateTimeMs(const DateTimeMs &ano);
		DateTimeMs(const DateTimeMs &&ano);
		DateTimeMs &operator=(const DateTime &rhs);
		DateTimeMs &operator=(const DateTime &&rhs);
		DateTimeMs &operator=(const DateTimeMs &rhs);
		DateTimeMs &operator=(const DateTimeMs &&rhs);
		~DateTimeMs(void);

		DateTimeMs getDatetimeAfter(const int year, const int month, const int day, const int hour, const int min, const int sec, const int msec) const;
		DateTimeMs getDatetimeAfter(const DateTime &datetime) const;
		DateTimeMs getDatetimeAfter(const DateTimeMs &datetime) const;
		DateTimeMs getDateAfter(const Date &date) const;
		DateTimeMs getDayAfter(const int day) const;
		DateTimeMs getTimeAfter(const Time &time) const;
		DateTimeMs getTimeAfter(const TimeMs &time) const;

		DateTimeMs getDatetimeBefore(const DateTime &datetime) const;
		DateTimeMs getDatetimeBefore(const DateTimeMs &datetime) const;
		DateTimeMs getDateBefore(const Date &date) const;
		DateTimeMs getTimeBefore(const Time &time) const;
		DateTimeMs getTimeBefore(const TimeMs &time) const;

		std::string toString(void) const;
	};

	Date getLocalDate(void);
	Date getDateAfterLocalDate(const int year, const int month, const int day);
	Date getDateAfterLocalDate(const Date &date);
	Date getDateAfterLocalDate(const int day);
	Date getDateBeforeLocalDate(const Date &date);

	DateTime getLocalDatetime(void);
	DateTime getDatetimeAfterLocalDatetime(const int year, const int month, const int day, const int hour, const int min, const int sec);
	DateTime getDatetimeAfterLocalDatetime(const DateTime &datetime);
	DateTime getDatetimeAfterLocalDatetime(const Date &date);
	DateTime getDatetimeAfterLocalDatetime(const int day);
	DateTime getDatetimeAfterLocalDatetime(const Time &time);
	DateTime getDatetimeBeforeLocalDatetime(const DateTime &datetime);
	DateTime getDatetimeBeforeLocalDatetime(const Date &date);
	DateTime getDatetimeBeforeLocalDatetime(const Time &time);

	DateTimeMs getLocalDatetimeMs(void);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const int year, const int month, const int day, const int hour, const int min, const int sec, const int msec = 0);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const DateTime &datetime);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const DateTimeMs &datetime);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const Date &date);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const int day);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const Time &time);
	DateTimeMs getDatetimeAfterLocalDatetimeMs(const TimeMs &time);
	DateTimeMs getDatetimeBeforeLocalDatetimeMs(const DateTime &datetime);
	DateTimeMs getDatetimeBeforeLocalDatetimeMs(const DateTimeMs &datetime);
	DateTimeMs getDatetimeBeforeLocalDatetimeMs(const Date &date);
	DateTimeMs getDatetimeBeforeLocalDatetimeMs(const Time &time);
	DateTimeMs getDatetimeBeforeLocalDatetimeMs(const TimeMs &time);

	bool isLeapYear(const unsigned short year);

	unsigned char getDayInWeek(unsigned short year, unsigned char month, const unsigned char day);
	unsigned char getDayInWeek(const Date &date);
	unsigned char getDayInWeek(const DateTime &datetime);
	unsigned char getDayInWeek(const DateTimeMs &datetime);

	unsigned char getDaysOfMonth(const unsigned short year, const unsigned char month);
	unsigned char getDaysOfMonth(const Date &date);
	unsigned char getDaysOfMonth(const DateTime &datetime);
	unsigned char getDaysOfMonth(const DateTimeMs &datetime);

	DateTime getBuildDateTime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString);
};

const bool operator<(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator<(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const bool operator<(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator<(const DateTimeUtils::Time &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator<(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::Time &rhs);
const bool operator<(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const bool operator<(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator<(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator<(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator<=(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator<=(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const bool operator<=(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator<=(const DateTimeUtils::Time &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator<=(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::Time &rhs);
const bool operator<=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const bool operator<=(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator<=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator<=(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator>(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator>(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const bool operator>(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator>(const DateTimeUtils::Time &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator>(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::Time &rhs);
const bool operator>(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const bool operator>(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator>(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator>(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator>=(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator>=(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const bool operator>=(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator>=(const DateTimeUtils::Time &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator>=(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::Time &rhs);
const bool operator>=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const bool operator>=(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator>=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTimeMs &rhs);
const bool operator>=(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator==(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator==(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const bool operator==(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator==(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const bool operator==(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);

const bool operator!=(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator!=(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const bool operator!=(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const bool operator!=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const bool operator!=(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);

const int operator-(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const int operator-(const DateTimeUtils::Time &lhs, const DateTimeUtils::Time &rhs);
const int operator-(const DateTimeUtils::TimeMs &lhs, const DateTimeUtils::TimeMs &rhs);
const DateTimeUtils::Time operator-(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);
const DateTimeUtils::Time operator-(const DateTimeUtils::DateTimeMs &lhs, const DateTimeUtils::DateTimeMs &rhs);

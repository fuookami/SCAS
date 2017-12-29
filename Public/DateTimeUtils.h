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

	struct TimeDuration
	{
		unsigned short hour;
		unsigned char min;
		unsigned char sec;

		TimeDuration(void);
		TimeDuration(const unsigned int seconds);
		TimeDuration(const unsigned short _hour, const unsigned char _min, const unsigned char _sec);
		TimeDuration(const TimeDuration &ano);
		TimeDuration(const TimeDuration &&ano);
		TimeDuration &operator=(const TimeDuration &rhs);
		TimeDuration &operator=(const TimeDuration &&rhs);
		~TimeDuration(void);

		TimeDuration getTimeDurationAfter(const int hour, const int min, const int sec) const;
		TimeDuration getTimeDurationAfter(const TimeDuration &time) const;
		TimeDuration getTimeDurationAfter(const int second) const;
		TimeDuration getTimeDurationBefore(const TimeDuration &time) const;

		int totalSeconds(void) const;
		int totalDays(void) const;
		std::string toString(void) const;
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
		DateTime(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		DateTime(const Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		DateTime(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour, const unsigned char _min, const unsigned char _sec);
		DateTime(const DateTime &ano);
		DateTime(const DateTime &&ano);
		DateTime &operator=(const DateTime &rhs);
		DateTime &operator=(const DateTime &&rhs);
		~DateTime(void);

		DateTime getDatetimeAfter(const int year, const int month, const int day, const int hour, const int min, const int sec) const;
		DateTime getDatetimeAfter(const DateTime &datetime) const;
		DateTime getDateAfter(const Date &date) const;
		DateTime getDayAfter(const int day) const;
		DateTime getTimeAfter(const TimeDuration &time) const;

		DateTime getDatetimeBefore(const DateTime &datetime) const;
		DateTime getDateBefore(const Date &date) const;
		DateTime getTimeBefore(const TimeDuration &time) const;

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
	DateTime getDatetimeAfterLocalDatetime(const TimeDuration &time);
	DateTime getDatetimeBeforeLocalDatetime(const DateTime &datetime);
	DateTime getDatetimeBeforeLocalDatetime(const Date &date);
	DateTime getDatetimeBeforeLocalDatetime(const TimeDuration &time);

	bool isLeapYear(const unsigned short year);

	unsigned char getDayInWeek(unsigned short year, unsigned char month, const unsigned char day);
	unsigned char getDayInWeek(const Date &date);
	unsigned char getDayInWeek(const DateTime &datetime);

	unsigned char getDaysOfMonth(const unsigned short year, const unsigned char month);
	unsigned char getDaysOfMonth(const Date &date);
	unsigned char getDaysOfMonth(const DateTime &datetime);

	DateTime getBuildDateTime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString);
};

const bool operator<(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator<(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const bool operator<(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator<=(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator<=(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const bool operator<=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator>(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator>(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const bool operator>(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator>=(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator>=(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const bool operator>=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator==(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator==(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const bool operator==(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

const bool operator!=(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const bool operator!=(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const bool operator!=(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

const int operator-(const DateTimeUtils::Date &lhs, const DateTimeUtils::Date &rhs);
const int operator-(const DateTimeUtils::TimeDuration &lhs, const DateTimeUtils::TimeDuration &rhs);
const DateTimeUtils::TimeDuration operator-(const DateTimeUtils::DateTime &lhs, const DateTimeUtils::DateTime &rhs);

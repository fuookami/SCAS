#pragma once

#include <string>
#include <array>
#include <tuple>

namespace DatetimeUtils
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

	static const unsigned short MillisecondsPerSecond = 1000;
	static const unsigned short MicrosecondsPerMillisecond = 1000;
	static const unsigned short SecondsPerHour = 3600;
	static const unsigned short SecondsPerMinute = 60;
	static const unsigned short MinutesPerHour = 60;
	static const unsigned short HoursPerDay = 24;

	struct Date;
	struct Time;
	struct TimeMs;
	struct Datetime;
	struct DatetimeMs;

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

		inline Date getDateAfter(const int year, const int month, const int day) const;
		inline Date getDateAfter(const Date &date) const;
		inline Date getDateAfter(const int day) const;

		inline Date getDateBefore(const Date &date) const;

		virtual inline std::string toString(void) const;
	};

	struct Time
	{
		bool negative;
		unsigned short hour;
		unsigned char min;
		unsigned char sec;

		Time(void);
		Time(const int seconds);
		Time(const unsigned short _hour, const unsigned char _min, const unsigned char _sec, const bool _negative = false);
		Time(const Time &ano);
		Time(const Time &&ano);
		Time &operator=(const Time &rhs);
		Time &operator=(const Time &&rhs);
		virtual ~Time(void);

		inline Time operator+(void) const;
		inline Time operator-(void) const;
		inline operator int(void) const;

		inline Time &operator+=(const Time &rhs);
		inline Time &operator-=(const Time &rhs);

		inline Time getTimeAfter(const int _hour, const int _min, const int _sec) const;
		inline Time getTimeAfter(const Time &time) const;
		inline Time getTimeAfter(const int second) const;
		inline Time getTimeBefore(const Time &time) const;

		inline int totalSeconds(void) const;
		inline int totalMinutes(void) const;
		inline int totalHours(void) const;
		inline int totalDays(void) const;
		virtual inline std::string toString(void) const;
	};

	struct TimeMs : public Time
	{
		unsigned short msec;

		TimeMs(void);
		TimeMs(const int milliseconds);
		TimeMs(const unsigned short _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec = 0, const bool _negative = false);
		TimeMs(const Time &ano, const unsigned short _msec = 0);
		TimeMs(const Time &&ano, const unsigned short _msec = 0);
		TimeMs(const TimeMs &ano);
		TimeMs(const TimeMs &&ano);
		TimeMs &operator=(const Time &rhs);
		TimeMs &operator=(const Time &&rhs);
		TimeMs &operator=(const TimeMs &rhs);
		TimeMs &operator=(const TimeMs &&rhs);
		~TimeMs(void);

		inline TimeMs operator+(void) const;
		inline TimeMs operator-(void) const;
		inline operator int(void) const;

		inline TimeMs &operator+=(const Time &rhs);
		inline TimeMs &operator+=(const TimeMs &rhs);
		inline TimeMs &operator-=(const Time &rhs);
		inline TimeMs &operator-=(const TimeMs &rhs);

		inline TimeMs getTimeMsAfter(const int _hour, const int _min, const int _sec, const int _msec) const;
		inline TimeMs getTimeMsAfter(const Time &time) const;
		inline TimeMs getTimeMsAfter(const TimeMs &time) const;
		inline TimeMs getTimeMsAfter(const int millisecond) const;
		inline TimeMs getTimeMsBefore(const Time &time) const;
		inline TimeMs getTimeMsBefore(const TimeMs &time) const;

		inline int totalMilliseconds(void) const;

		inline Time toTime(void) const;
		inline std::string toString(void) const override;
	};

	struct Datetime
	{
		short year;
		unsigned char month;
		unsigned char day;
		unsigned char hour;
		unsigned char min;
		unsigned char sec;

		Datetime(void);
		Datetime(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		Datetime(const Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		Datetime(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		Datetime(const Datetime &ano);
		Datetime(const Datetime &&ano);
		Datetime &operator=(const Datetime &rhs);
		Datetime &operator=(const Datetime &&rhs);
		~Datetime(void);

		inline Datetime getDatetimeAfter(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0) const;
		inline Datetime getDatetimeAfter(const Datetime &datetime) const;
		inline Datetime getDatetimeAfter(const Date &date) const;
		inline Datetime getDatetimeAfter(const int day) const;
		inline Datetime getDatetimeAfter(const Time &time) const;

		inline Datetime getDatetimeBefore(const Datetime &datetime) const;
		inline Datetime getDatetimeBefore(const Date &date) const;
		inline Datetime getDatetimeBefore(const Time &time) const;

		inline Date getDate(void) const;
		inline Time getTime(void) const;
		virtual inline std::string toString(void) const;

		inline const bool isLeapYear(void) const;
		inline const unsigned char getDayOfMonth(void) const;
		inline const unsigned char getDayInWeek(void) const;
	};

	struct DatetimeMs : public Datetime
	{
		unsigned short msec;

		DatetimeMs(void);
		DatetimeMs(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DatetimeMs(const Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DatetimeMs(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DatetimeMs(const Datetime &ano, const unsigned short _msec = 0);
		DatetimeMs(const Datetime &&ano, const unsigned short _msec = 0);
		DatetimeMs(const DatetimeMs &ano);
		DatetimeMs(const DatetimeMs &&ano);
		DatetimeMs &operator=(const Datetime &rhs);
		DatetimeMs &operator=(const Datetime &&rhs);
		DatetimeMs &operator=(const DatetimeMs &rhs);
		DatetimeMs &operator=(const DatetimeMs &&rhs);
		~DatetimeMs(void);

		inline DatetimeMs getDatetimeMsAfter(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0, const int msec = 0) const;
		inline DatetimeMs getDatetimeMsAfter(const Datetime &datetime) const;
		inline DatetimeMs getDatetimeMsAfter(const DatetimeMs &datetime) const;
		inline DatetimeMs getDatetimeMsAfter(const Date &date) const;
		inline DatetimeMs getDatetimeMsAfter(const int day) const;
		inline DatetimeMs getDatetimeMsAfter(const Time &time) const;
		inline DatetimeMs getDatetimeMsAfter(const TimeMs &time) const;

		inline DatetimeMs getDatetimeMsBefore(const Datetime &datetime) const;
		inline DatetimeMs getDatetimeMsBefore(const DatetimeMs &datetime) const;
		inline DatetimeMs getDatetimeMsBefore(const Date &date) const;
		inline DatetimeMs getDatetimeMsBefore(const Time &time) const;
		inline DatetimeMs getDatetimeMsBefore(const TimeMs &time) const;

		inline Datetime getDatetime(void) const;
		inline TimeMs getTimeMs(void) const;
		inline std::string toString(void) const;
	};

	const Date getLocalDate(void);
	const Date getDateAfterLocalDate(const int year, const int month, const int day);
	const Date getDateAfterLocalDate(const Date &date);
	const Date getDateAfterLocalDate(const int day);
	const Date getDateBeforeLocalDate(const Date &date);

	const Time getLocalTime(void);
	const Time getTimeAfterLocalTime(const int hour, const int min, const int sec);
	const Time getTimeAfterLocalTime(const Time &time);
	const Time getTimeAfterLocalTime(const int second);
	const Time getTimeBeforeLocalTime(const Time &time);

	const TimeMs getLocalTimeMs(void);
	const TimeMs getTimeMsAfterLocalTime(const int hour, const int min, const int sec, const int msec = 0);
	const TimeMs getTimeMsAfterLocalTime(const Time &time);
	const TimeMs getTimeMsAfterLocalTime(const TimeMs &time);
	const TimeMs getTimeMsAfterLocalTime(const int millisecond);
	const TimeMs getTimeMsBeforeLocalTime(const Time &time);
	const TimeMs getTimeMsBeforeLocalTime(const TimeMs &time);

	const Datetime getLocalDatetime(void);
	const Datetime getDatetimeAfterLocalDatetime(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0);
	const Datetime getDatetimeAfterLocalDatetime(const Datetime &datetime);
	const Datetime getDatetimeAfterLocalDatetime(const Date &date);
	const Datetime getDatetimeAfterLocalDatetime(const int day);
	const Datetime getDatetimeAfterLocalDatetime(const Time &time);
	const Datetime getDatetimeBeforeLocalDatetime(const Datetime &datetime);
	const Datetime getDatetimeBeforeLocalDatetime(const Date &date);
	const Datetime getDatetimeBeforeLocalDatetime(const Time &time);

	const DatetimeMs getLocalDatetimeMs(void);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0, const int msec = 0);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Datetime &datetime);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const DatetimeMs &datetime);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Date &date);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const int day);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Time &time);
	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const TimeMs &time);
	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Datetime &datetime);
	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const DatetimeMs &datetime);
	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Date &date);
	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Time &time);
	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const TimeMs &time);

	const bool isLeapYear(const unsigned short year);

	const unsigned char getDayInWeek(unsigned short year, unsigned char month, const unsigned char day);
	const unsigned char getDayInWeek(const Date &date);
	const unsigned char getDayInWeek(const Datetime &datetime);

	const unsigned char getDaysOfMonth(const unsigned short year, const unsigned char month);
	const unsigned char getDaysOfMonth(const Date &date);
	const unsigned char getDaysOfMonth(const Datetime &datetime);

	const Datetime getBuildDatetime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString);
};

const bool operator<(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const bool operator<(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const bool operator<(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const bool operator<(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const bool operator<(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

const bool operator<=(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const bool operator<=(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const bool operator<=(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const bool operator<=(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const bool operator<=(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

const bool operator>(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const bool operator>(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const bool operator>(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const bool operator>(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const bool operator>(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

const bool operator>=(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const bool operator>=(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const bool operator>=(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const bool operator>=(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const bool operator>=(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

const bool operator==(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const bool operator==(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const bool operator==(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const bool operator==(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const bool operator==(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

const bool operator!=(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const bool operator!=(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const bool operator!=(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const bool operator!=(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const bool operator!=(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

const DatetimeUtils::Time operator+(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const DatetimeUtils::TimeMs operator+(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const DatetimeUtils::TimeMs operator+(const DatetimeUtils::Time &lhs, const DatetimeUtils::TimeMs &rhs);
const DatetimeUtils::TimeMs operator+(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::Time &rhs);

const DatetimeUtils::Time operator-(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
const DatetimeUtils::TimeMs operator-(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
const DatetimeUtils::TimeMs operator-(const DatetimeUtils::Time &lhs, const DatetimeUtils::TimeMs &rhs);
const DatetimeUtils::TimeMs operator-(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::Time &rhs);

const int operator-(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
const DatetimeUtils::Time operator-(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
const DatetimeUtils::TimeMs operator-(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

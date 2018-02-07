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
		Date(const Date &ano) = default;
		Date(Date &&ano) = default;
		Date &operator=(const Date &rhs) = default;
		Date &operator=(Date &&rhs) = default;
		~Date(void) = default;

		inline Date getDateAfter(const int year, const int month, const int day) const;
		inline Date getDateAfter(const Date &date) const;
		inline Date getDateAfter(const int day) const;

		inline Date getDateBefore(const Date &date) const;

		inline const bool isLeapYear(void) const;
		inline const unsigned char getDayOfMonth(void) const;
		inline const unsigned char getDayInWeek(void) const;
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
		Time(const Time &ano) = default;
		Time(Time &&ano) = default;
		Time &operator=(const Time &rhs) = default;
		Time &operator=(Time &&rhs) = default;
		virtual ~Time(void) = default;

		inline Time operator+(void) const;
		inline Time operator-(void) const;
		inline operator int(void) const;

		inline Time &operator+=(const Time &rhs);
		inline Time &operator-=(const Time &rhs);

		inline Time getTimeAfter(const int _hour, const int _min, const int _sec) const;
		inline Time getTimeAfter(const Time &time) const;
		inline Time getTimeAfter(const int second) const;
		inline Time getTimeBefore(const Time &time) const;

		inline const int totalSeconds(void) const;
		inline const int totalMinutes(void) const;
		inline const int totalHours(void) const;
		inline const int totalDays(void) const;
		virtual inline std::string toString(void) const;
	};

	struct TimeMs : public Time
	{
		unsigned short msec;

		TimeMs(void);
		TimeMs(const int milliseconds);
		TimeMs(const unsigned short _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec = 0, const bool _negative = false);
		TimeMs(const Time &ano, const unsigned short _msec = 0);
		TimeMs(Time &&ano, const unsigned short _msec = 0);
		TimeMs(const TimeMs &ano) = default;
		TimeMs(TimeMs &&ano) = default;
		TimeMs &operator=(const Time &rhs);
		TimeMs &operator=(Time &&rhs);
		TimeMs &operator=(const TimeMs &rhs) = default;
		TimeMs &operator=(TimeMs &&rhs) = default;
		~TimeMs(void) = default;

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

		inline const int totalMilliseconds(void) const;

		inline Time toTime(void) const;
		inline std::string toString(void) const override;
	};

	struct Datetime : public Date
	{
		unsigned char hour;
		unsigned char min;
		unsigned char sec;

		Datetime(void);
		Datetime(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		Datetime(Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		Datetime(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0);
		Datetime(const Datetime &ano) = default;
		Datetime(Datetime &&ano) = default;
		Datetime &operator=(const Datetime &rhs) = default;
		Datetime &operator=(Datetime &&rhs) = default;
		virtual ~Datetime(void) = default;

		inline Datetime getDatetimeAfter(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0) const;
		inline Datetime getDatetimeAfter(const Date &date) const;
		inline Datetime getDatetimeAfter(const Datetime &datetime) const;
		inline Datetime getDatetimeAfter(const int day) const;
		inline Datetime getDatetimeAfter(const Time &time) const;

		inline Datetime getDatetimeBefore(const Date &date) const;
		inline Datetime getDatetimeBefore(const Datetime &datetime) const;
		inline Datetime getDatetimeBefore(const Time &time) const;

		inline Date getDate(void) const;
		inline Time getTime(void) const;
		virtual inline std::string toString(void) const;
	};

	struct DatetimeMs : public Datetime
	{
		unsigned short msec;

		DatetimeMs(void);
		DatetimeMs(const Date &date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DatetimeMs(const Date &&date, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DatetimeMs(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour = 0, const unsigned char _min = 0, const unsigned char _sec = 0, const unsigned short _msec = 0);
		DatetimeMs(const Datetime &ano, const unsigned short _msec = 0);
		DatetimeMs(Datetime &&ano, const unsigned short _msec = 0);
		DatetimeMs(const DatetimeMs &ano) = default;
		DatetimeMs(DatetimeMs &&ano) = default;
		DatetimeMs &operator=(const Datetime &rhs);
		DatetimeMs &operator=(Datetime &&rhs);
		DatetimeMs &operator=(const DatetimeMs &rhs) = default;
		DatetimeMs &operator=(DatetimeMs &&rhs) = default;
		~DatetimeMs(void) = default;

		inline DatetimeMs getDatetimeMsAfter(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0, const int msec = 0) const;
		inline DatetimeMs getDatetimeMsAfter(const Date &date) const;
		inline DatetimeMs getDatetimeMsAfter(const Datetime &datetime) const;
		inline DatetimeMs getDatetimeMsAfter(const DatetimeMs &datetime) const;
		inline DatetimeMs getDatetimeMsAfter(const int day) const;
		inline DatetimeMs getDatetimeMsAfter(const Time &time) const;
		inline DatetimeMs getDatetimeMsAfter(const TimeMs &time) const;

		inline DatetimeMs getDatetimeMsBefore(const Date &date) const;
		inline DatetimeMs getDatetimeMsBefore(const Datetime &datetime) const;
		inline DatetimeMs getDatetimeMsBefore(const DatetimeMs &datetime) const;
		inline DatetimeMs getDatetimeMsBefore(const Time &time) const;
		inline DatetimeMs getDatetimeMsBefore(const TimeMs &time) const;

		inline Datetime getDatetime(void) const;
		inline TimeMs getTimeMs(void) const;
		inline std::string toString(void) const;
	};

	Date getLocalDate(void);
	Date getDateAfterLocalDate(const int year, const int month, const int day);
	Date getDateAfterLocalDate(const Date &date);
	Date getDateAfterLocalDate(const int day);
	Date getDateBeforeLocalDate(const Date &date);

	Time getLocalTime(void);
	Time getTimeAfterLocalTime(const int hour, const int min, const int sec);
	Time getTimeAfterLocalTime(const Time &time);
	Time getTimeAfterLocalTime(const int second);
	Time getTimeBeforeLocalTime(const Time &time);

	TimeMs getLocalTimeMs(void);
	TimeMs getTimeMsAfterLocalTime(const int hour, const int min, const int sec, const int msec = 0);
	TimeMs getTimeMsAfterLocalTime(const Time &time);
	TimeMs getTimeMsAfterLocalTime(const TimeMs &time);
	TimeMs getTimeMsAfterLocalTime(const int millisecond);
	TimeMs getTimeMsBeforeLocalTime(const Time &time);
	TimeMs getTimeMsBeforeLocalTime(const TimeMs &time);

	Datetime getLocalDatetime(void);
	Datetime getDatetimeAfterLocalDatetime(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0);
	Datetime getDatetimeAfterLocalDatetime(const Date &date);
	Datetime getDatetimeAfterLocalDatetime(const Datetime &datetime);
	Datetime getDatetimeAfterLocalDatetime(const int day);
	Datetime getDatetimeAfterLocalDatetime(const Time &time);
	Datetime getDatetimeBeforeLocalDatetime(const Date &date);
	Datetime getDatetimeBeforeLocalDatetime(const Datetime &datetime);
	Datetime getDatetimeBeforeLocalDatetime(const Time &time);

	DatetimeMs getLocalDatetimeMs(void);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const int year, const int month, const int day, const int hour = 0, const int min = 0, const int sec = 0, const int msec = 0);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Date &date);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Datetime &datetime);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const DatetimeMs &datetime);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const int day);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Time &time);
	DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const TimeMs &time);
	DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Date &date);
	DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Datetime &datetime);
	DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const DatetimeMs &datetime);
	DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Time &time);
	DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const TimeMs &time);

	const bool isLeapYear(const unsigned short year);
	const bool isLeapYear(const Date & date);

	const unsigned char getDayInWeek(unsigned short year, unsigned char month, const unsigned char day);
	const unsigned char getDayInWeek(const Date &date);

	const unsigned char getDaysOfMonth(const unsigned short year, const unsigned char month);
	const unsigned char getDaysOfMonth(const Date &date);

	Datetime getBuildDatetime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString);
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

DatetimeUtils::Time operator+(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
DatetimeUtils::TimeMs operator+(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
DatetimeUtils::TimeMs operator+(const DatetimeUtils::Time &lhs, const DatetimeUtils::TimeMs &rhs);
DatetimeUtils::TimeMs operator+(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::Time &rhs);

DatetimeUtils::Time operator-(const DatetimeUtils::Time &lhs, const DatetimeUtils::Time &rhs);
DatetimeUtils::TimeMs operator-(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::TimeMs &rhs);
DatetimeUtils::TimeMs operator-(const DatetimeUtils::Time &lhs, const DatetimeUtils::TimeMs &rhs);
DatetimeUtils::TimeMs operator-(const DatetimeUtils::TimeMs &lhs, const DatetimeUtils::Time &rhs);

const int operator-(const DatetimeUtils::Date &lhs, const DatetimeUtils::Date &rhs);
DatetimeUtils::Time operator-(const DatetimeUtils::Datetime &lhs, const DatetimeUtils::Datetime &rhs);
DatetimeUtils::TimeMs operator-(const DatetimeUtils::DatetimeMs &lhs, const DatetimeUtils::DatetimeMs &rhs);

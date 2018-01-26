#include "DatetimeUtils.h"

#include <boost/date_time/gregorian/gregorian.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <sstream>
#include <iomanip>
#include <algorithm>

namespace DatetimeUtils
{
	Date::Date(void)
		: Date(getLocalDate())
	{
	}

	Date::Date(const short _year, const unsigned char _month, const unsigned char _day)
		: year(_year), month(_month), day(_day)
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

		return Date(static_cast<short>(targetDate.year()), static_cast<unsigned char>(targetDate.month()), static_cast<unsigned char>(targetDate.day()));
	}

	Date Date::getDateAfter(const Date & date) const
	{
		return this->getDateAfter(date.year, date.month, date.day);
	}

	Date Date::getDateAfter(const int day) const
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

	Time::Time(void)
		: Time(static_cast<short>(0), static_cast<unsigned char>(0), static_cast<unsigned char>(0))
	{
	}

	Time::Time(const int seconds)
		: Time(static_cast<short>(std::abs(seconds) / SecondsPerHour), static_cast<unsigned char>((std::abs(seconds)/SecondsPerMinute) % MinutesPerHour), static_cast<unsigned char>(std::abs(seconds) % SecondsPerMinute), seconds < 0)
	{
	}

	Time::Time(const unsigned short _hour, const unsigned char _min, const unsigned char _sec, const bool _negative)
		: hour(_hour), min(_min), sec(_sec), negative(_negative)
	{
	}

	Time Time::operator+(void) const
	{
		return Time(hour, min, sec, false);
	}

	Time Time::operator-(void) const
	{
		return Time(hour, min, sec, true);
	}

	Time::operator int(void) const
	{
		return totalSeconds();
	}

	Time & Time::operator+=(const Time & rhs)
	{
		*this = *this + rhs;
		return *this;
	}

	Time & Time::operator-=(const Time & rhs)
	{
		*this = *this - rhs;
		return *this;
	}

	Time Time::getTimeAfter(const int _hour, const int _min, const int _sec) const
	{
		return *this + Time(_hour * SecondsPerHour + _min * SecondsPerMinute + _sec);
	}

	Time Time::getTimeAfter(const Time & time) const
	{
		return this->getTimeAfter(time.hour, time.min, time.sec);
	}

	Time Time::getTimeAfter(const int second) const
	{
		return this->getTimeAfter(0, 0, second);
	}

	Time Time::getTimeBefore(const Time & time) const
	{
		return this->getTimeAfter(-time.hour, -time.min, -time.sec);
	}

	int Time::totalSeconds(void) const
	{
		int temp(hour * SecondsPerHour + min * SecondsPerMinute + sec);
		return negative ? -temp : temp;
	}

	int Time::totalMinutes(void) const
	{
		int temp(hour * MinutesPerHour + min);
		return negative ? -temp : temp;
	}

	int Time::totalHours(void) const
	{
		return negative ? -hour : hour;
	}

	int Time::totalDays(void) const
	{
		int temp(hour / HoursPerDay);
		return negative ? -temp : temp;
	}

	std::string Time::toString(void) const
	{
		std::ostringstream timeSout;
		if (negative)
		{
			timeSout << "-";
		}
		timeSout << std::setfill('0') << hour << ":"
			<< std::setw(2) << (unsigned short)min << ":"
			<< std::setw(2) << (unsigned short)sec << std::setfill(' ');

		return timeSout.str();
	}

	TimeMs::TimeMs(void)
		: Time(0, 0, 0), msec(0)
	{
	}

	TimeMs::TimeMs(const int milliseconds)
		: Time(milliseconds / MillisecondsPerSecond), msec(std::abs(milliseconds) % MillisecondsPerSecond)
	{
	}

	TimeMs::TimeMs(const unsigned short _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec /* = 0 */, const bool _negative /* = false */)
		: Time(_hour, _min, _sec, _negative), msec(_msec)
	{
	}

	TimeMs::TimeMs(const Time &ano, const unsigned short _msec /* = 0 */)
		: Time(ano), msec(_msec)
	{
	}

	TimeMs::TimeMs(Time &&ano, const unsigned short _msec /* = 0 */)
		: Time(std::move(ano)), msec(_msec)
	{
	}

	TimeMs & TimeMs::operator=(const Time & rhs)
	{
		msec = 0;

		Time::operator=(rhs);

		return *this;
	}

	TimeMs & TimeMs::operator=(Time && rhs)
	{
		msec = 0;

		Time::operator=(std::move(rhs));

		return *this;
	}

	TimeMs TimeMs::operator+(void) const
	{
		return TimeMs(hour, min, sec, msec, false);
	}

	TimeMs TimeMs::operator-(void) const
	{
		return TimeMs(hour, min, sec, msec, true);
	}

	TimeMs::operator int(void) const
	{
		return totalMilliseconds();
	}

	TimeMs & TimeMs::operator+=(const Time & rhs)
	{
		*this = *this + rhs;
		return *this;
	}

	inline TimeMs & TimeMs::operator+=(const TimeMs & rhs)
	{
		*this = *this + rhs;
		return *this;
	}

	TimeMs & TimeMs::operator-=(const Time & rhs)
	{
		*this = *this - rhs;
		return *this;
	}

	inline TimeMs & TimeMs::operator-=(const TimeMs & rhs)
	{
		*this = *this - rhs;
		return *this;
	}

	TimeMs TimeMs::getTimeMsAfter(const int _hour, const int _min, const int _sec, const int _msec) const
	{
		return *this + TimeMs((_hour * SecondsPerHour + _min * SecondsPerMinute + _sec) * MillisecondsPerSecond + _msec);
	}

	TimeMs TimeMs::getTimeMsAfter(const Time & time) const
	{
		return getTimeMsAfter(time.hour, time.min, time.sec, 0);
	}

	TimeMs TimeMs::getTimeMsAfter(const TimeMs & time) const
	{
		return getTimeMsAfter(time.hour, time.min, time.sec, time.msec);
	}

	TimeMs TimeMs::getTimeMsAfter(const int millisecond) const
	{
		return getTimeMsAfter(0, 0, 0, millisecond);
	}

	TimeMs TimeMs::getTimeMsBefore(const Time & time) const
	{
		return getTimeMsAfter(-time.hour, -time.min, -time.sec, 0);
	}

	TimeMs TimeMs::getTimeMsBefore(const TimeMs & time) const
	{
		return getTimeMsAfter(-time.hour, -time.min, -time.sec, -time.msec);
	}

	int TimeMs::totalMilliseconds(void) const
	{
		int temp(totalSeconds() * MillisecondsPerSecond);
		return temp < 0 ? (temp - msec) : (temp + msec);
	}

	inline Time TimeMs::toTime(void) const
	{
		return Time(hour, min, sec, negative);
	}

	inline std::string TimeMs::toString(void) const
	{
		std::ostringstream timeSout;
		timeSout << Time::toString() << "." << std::setfill('0') << std::setw(3) << msec;

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

	Datetime::Datetime(Date && date, const unsigned char _hour, const unsigned char _min, const unsigned char _sec)
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

	Datetime Datetime::getDatetimeAfter(const int year, const int month, const int day, const int hour /* = 0 */, const int min /* = 0 */, const int sec /* = 0 */) const
	{
		using namespace boost::gregorian;
		using namespace boost::posix_time;

		date currDate(this->year, this->month, this->day);
		time_duration currTime(this->hour, this->min, this->sec);
		ptime targetDatetime(currDate, currTime);

		targetDatetime = year > 0 ? (targetDatetime + years(year))
			: year < 0 ? (targetDatetime - years(abs(year))) : targetDatetime;
		targetDatetime = month > 0 ? (targetDatetime + months(month))
			: month < 0 ? (targetDatetime - months(abs(month))) : targetDatetime;
		targetDatetime = day > 0 ? (targetDatetime + days(day))
			: day < 0 ? (targetDatetime - days(abs(day))) : targetDatetime;
		targetDatetime = hour > 0 ? (targetDatetime + hours(hour))
			: hour < 0 ? (targetDatetime - hours(abs(hour))) : targetDatetime;
		targetDatetime = min > 0 ? (targetDatetime + minutes(min))
			: min < 0 ? (targetDatetime - minutes(abs(min))) : targetDatetime;
		targetDatetime = sec > 0 ? (targetDatetime + seconds(sec))
			: sec < 0 ? (targetDatetime - seconds(abs(sec))) : targetDatetime;

		const date &targetDate(targetDatetime.date());
		const time_duration &targetTime(targetDatetime.time_of_day());

		return Datetime(static_cast<short>(targetDate.year()), static_cast<unsigned char>(targetDate.month()), static_cast<unsigned char>(targetDate.day()), static_cast<unsigned char>(targetTime.hours()), static_cast<unsigned char>(targetTime.minutes()),  static_cast<unsigned char>(targetTime.seconds()));
	}

	Datetime Datetime::getDatetimeAfter(const Datetime & datetime) const
	{
		return getDatetimeAfter(datetime.year, datetime.month, datetime.day, datetime.hour, datetime.min, datetime.sec);
	}

	Datetime Datetime::getDatetimeAfter(const Date & date) const
	{
		return getDatetimeAfter(date.year, date.month, date.day);
	}

	Datetime Datetime::getDatetimeAfter(const int day) const
	{
		return getDatetimeAfter(0, 0, day);
	}

	Datetime Datetime::getDatetimeAfter(const Time & time) const
	{
		return !time.negative ? getDatetimeAfter(0, 0, 0, time.hour, time.min, time.sec) : getDatetimeAfter(0, 0, 0, -time.hour, -time.min, -time.sec);
	}

	Datetime Datetime::getDatetimeBefore(const Datetime & datetime) const
	{
		return getDatetimeAfter(-datetime.year, -datetime.month, -datetime.day, -datetime.hour, -datetime.min, -datetime.sec);
	}

	Datetime Datetime::getDatetimeBefore(const Date & date) const
	{
		return getDatetimeAfter(-date.year, -date.month, -date.day);
	}

	Datetime Datetime::getDatetimeBefore(const Time & time) const
	{
		return !time.negative ? getDatetimeAfter(0, 0, 0, -time.hour, -time.min, -time.sec) : getDatetimeAfter(0, 0, 0, time.hour, time.min, time.sec);
	}

	Date Datetime::getDate(void) const
	{
		return Date(year, month, day);
	}

	Time Datetime::getTime(void) const
	{
		return Time(hour, min, sec);
	}

	std::string Datetime::toString() const
	{
		std::ostringstream datetimeSout;
		datetimeSout << getDate().toString() << " " << getTime().toString();

		return datetimeSout.str();
	}

	const bool Datetime::isLeapYear(void) const
	{
		return DatetimeUtils::isLeapYear(year);
	}

	const unsigned char Datetime::getDayOfMonth(void) const
	{
		return DatetimeUtils::getDaysOfMonth(year, month);
	}

	const unsigned char Datetime::getDayInWeek(void) const
	{
		return DatetimeUtils::getDayInWeek(year, month, day);
	}

	DatetimeMs::DatetimeMs(void)
		: DatetimeMs(getLocalDatetimeMs())
	{
	}

	DatetimeMs::DatetimeMs(const Date & date, const unsigned char _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec)
		: Datetime(date, _hour, _min, _sec), msec(_msec)
	{
	}

	DatetimeMs::DatetimeMs(const Date && date, const unsigned char _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec)
		: Datetime(std::move(date), _hour, _min, _sec), msec(_msec)
	{
	}

	DatetimeMs::DatetimeMs(const short _year, const unsigned char _month, const unsigned char _day, const unsigned char _hour, const unsigned char _min, const unsigned char _sec, const unsigned short _msec)
		: Datetime(_year, _month, _day, _hour, _min, _sec), msec(_msec)
	{
	}

	DatetimeMs::DatetimeMs(const Datetime & ano, const unsigned short _msec)
		: Datetime(ano), msec(_msec)
	{
	}

	DatetimeMs::DatetimeMs(Datetime && ano, const unsigned short _msec)
		: Datetime(std::move(ano)), msec(_msec)
	{
	}

	DatetimeMs &DatetimeMs::operator=(const Datetime &rhs)
	{
		msec = 0;

		Datetime::operator=(rhs);

		return *this;
	}

	DatetimeMs & DatetimeMs::operator=(Datetime && rhs)
	{
		msec = 0;

		Datetime::operator=(std::move(rhs));

		return *this;
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const int year, const int month, const int day, const int hour /* = 0 */, const int min /* = 0 */, const int sec /* = 0 */, const int msec /* = 0 */) const
	{
		using namespace boost::gregorian;
		using namespace boost::posix_time;

		date currDate(this->year, this->month, this->day);
		time_duration currTime(this->hour, this->min, this->sec, this->msec * MicrosecondsPerMillisecond);
		ptime targetDatetimeMs(currDate, currTime);

		targetDatetimeMs = year > 0 ? (targetDatetimeMs + years(year))
			: year < 0 ? (targetDatetimeMs - years(abs(year))) : targetDatetimeMs;
		targetDatetimeMs = month > 0 ? (targetDatetimeMs + months(month))
			: month < 0 ? (targetDatetimeMs - months(abs(month))) : targetDatetimeMs;
		targetDatetimeMs = day > 0 ? (targetDatetimeMs + days(day))
			: day < 0 ? (targetDatetimeMs - days(abs(day))) : targetDatetimeMs;
		targetDatetimeMs = hour > 0 ? (targetDatetimeMs + hours(hour))
			: hour < 0 ? (targetDatetimeMs - hours(abs(hour))) : targetDatetimeMs;
		targetDatetimeMs = min > 0 ? (targetDatetimeMs + minutes(min))
			: min < 0 ? (targetDatetimeMs - minutes(abs(min))) : targetDatetimeMs;
		targetDatetimeMs = sec > 0 ? (targetDatetimeMs + seconds(sec))
			: sec < 0 ? (targetDatetimeMs - seconds(abs(sec))) : targetDatetimeMs;
		targetDatetimeMs = msec > 0 ? (targetDatetimeMs + milliseconds(msec))
			: msec < 0 ? (targetDatetimeMs - milliseconds(abs(msec))) : targetDatetimeMs;

		const date &targetDate(targetDatetimeMs.date());
		const time_duration &targetTime(targetDatetimeMs.time_of_day());

		return DatetimeMs(static_cast<short>(targetDate.year()), static_cast<unsigned char>(targetDate.month()), static_cast<unsigned char>(targetDate.day()), static_cast<unsigned char>(targetTime.hours()), static_cast<unsigned char>(targetTime.minutes()), static_cast<unsigned char>(targetTime.seconds()), static_cast<unsigned short>(targetTime.fractional_seconds() / MicrosecondsPerMillisecond));
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const Datetime & datetime) const
	{
		return getDatetimeMsAfter(datetime.year, datetime.month, datetime.day, datetime.hour, datetime.min, datetime.sec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const DatetimeMs & datetime) const
	{
		return getDatetimeMsAfter(datetime.year, datetime.month, datetime.day, datetime.hour, datetime.min, datetime.sec, datetime.msec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const Date & date) const
	{
		return getDatetimeMsAfter(date.year, date.month, date.day);
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const int day) const
	{
		return getDatetimeMsAfter(0, 0, -day);
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const Time & time) const
	{
		return !time.negative ? getDatetimeMsAfter(0, 0, 0, time.hour, time.min, time.sec) : getDatetimeMsAfter(0, 0, 0, -time.hour, -time.min, -time.sec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsAfter(const TimeMs & time) const
	{
		return !time.negative ? getDatetimeMsAfter(0, 0, 0, time.hour, time.min, time.sec, time.msec) : getDatetimeMsAfter(0, 0, 0, -time.hour, -time.min, -time.sec, -time.msec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsBefore(const Datetime & datetime) const
	{
		return getDatetimeMsAfter(-datetime.year, -datetime.month, -datetime.day, -datetime.hour, -datetime.min, -datetime.sec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsBefore(const DatetimeMs & datetime) const
	{
		return getDatetimeMsAfter(-datetime.year, -datetime.month, -datetime.day, -datetime.hour, -datetime.min, -datetime.sec, -datetime.msec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsBefore(const Date & date) const
	{
		return getDatetimeMsAfter(-date.year, -date.month, -date.day);
	}

	DatetimeMs DatetimeMs::getDatetimeMsBefore(const Time & time) const
	{
		return !time.negative ? getDatetimeMsAfter(0, 0, 0, -time.hour, -time.min, -time.sec) : getDatetimeMsAfter(0, 0, 0, time.hour, time.min, time.sec);
	}

	DatetimeMs DatetimeMs::getDatetimeMsBefore(const TimeMs & time) const
	{
		return !time.negative ? getDatetimeMsAfter(0, 0, 0, -time.hour, -time.min, -time.sec, -time.msec) : getDatetimeMsAfter(0, 0, 0, time.hour, time.min, time.sec, time.msec);
	}

	Datetime DatetimeMs::getDatetime(void) const
	{
		return Datetime(*this);
	}

	TimeMs DatetimeMs::getTimeMs(void) const
	{
		return TimeMs(getTime(), msec);
	}

	std::string DatetimeMs::toString(void) const
	{
		std::ostringstream timeSout;
		timeSout << getDate().toString() << " " << getTimeMs().toString();

		return timeSout.str();
	}

	const Date getLocalDate(void)
	{
		using namespace boost::gregorian;

		date localDate(day_clock::local_day());

		return Date({ static_cast<short>(localDate.year()), static_cast<unsigned char>(localDate.month()), static_cast<unsigned char>(localDate.day()) });
	}

	const Date getDateAfterLocalDate(const int year, const int month, const int day)
	{
		return getLocalDate().getDateAfter(year, month, day);
	}

	const Date getDateAfterLocalDate(const Date & date)
	{
		return getLocalDate().getDateAfter(date);
	}

	const Date getDateAfterLocalDate(const int day)
	{
		return getLocalDate().getDateAfter(day);
	}

	const Date getDateBeforeLocalDate(const Date & date)
	{
		return getLocalDate().getDateBefore(date);
	}

	const Time getLocalTime(void)
	{
		using namespace boost::posix_time;

		ptime localDatetime(second_clock::local_time());
		const time_duration &localTime(localDatetime.time_of_day());
		
		return Time(static_cast<unsigned short>(localTime.hours()), static_cast<unsigned char>(localTime.minutes()), static_cast<unsigned char>(localTime.seconds()));
	}

	const Time getTimeAfterLocalTime(const int hour, const int min, const int sec)
	{
		return getLocalTime().getTimeAfter(hour, min, sec);
	}

	const Time getTimeAfterLocalTime(const Time & time)
	{
		return getLocalTime().getTimeAfter(time);
	}

	const Time getTimeAfterLocalTime(const int second)
	{
		return getLocalTime().getTimeAfter(second);
	}

	const Time getTimeBeforeLocalTime(const Time & time)
	{
		return getLocalTime().getTimeBefore(time);
	}

	const TimeMs getLocalTimeMs(void)
	{
		using namespace boost::posix_time;

		ptime localDatetime(second_clock::local_time());
		const time_duration &localTime(localDatetime.time_of_day());

		return TimeMs(static_cast<unsigned short>(localTime.hours()), static_cast<unsigned char>(localTime.minutes()), static_cast<unsigned char>(localTime.seconds()), static_cast<unsigned short>(localTime.fractional_seconds() / MicrosecondsPerMillisecond));
	}

	const TimeMs getTimeMsAfterLocalTime(const int hour, const int min, const int sec, const int msec)
	{
		return getLocalTimeMs().getTimeMsAfter(hour, min, sec, msec);
	}

	const TimeMs getTimeMsAfterLocalTime(const Time & time)
	{
		return getLocalTimeMs().getTimeMsAfter(time);
	}

	const TimeMs getTimeMsAfterLocalTime(const TimeMs & time)
	{
		return getLocalTimeMs().getTimeMsAfter(time);
	}

	const TimeMs getTimeMsAfterLocalTime(const int millisecond)
	{
		return getLocalTimeMs().getTimeMsAfter(millisecond);
	}

	const TimeMs getTimeMsBeforeLocalTime(const Time & time)
	{
		return getLocalTimeMs().getTimeMsBefore(time);
	}

	const TimeMs getTimeMsBeforeLocalTime(const TimeMs & time)
	{
		return getLocalTimeMs().getTimeMsBefore(time);
	}

	const Datetime getLocalDatetime(void)
	{
		using namespace boost::gregorian;
		using namespace boost::posix_time;

		ptime localDatetime(second_clock::local_time());
		const date &localDate(localDatetime.date());
		const time_duration &localTime(localDatetime.time_of_day());

		return Datetime(static_cast<short>(localDate.year()), static_cast<unsigned char>(localDate.month()), static_cast<unsigned char>(localDate.day()), static_cast<unsigned char>(localTime.hours()), static_cast<unsigned char>(localTime.minutes()),  static_cast<unsigned char>(localTime.seconds()));
	}

	const Datetime getDatetimeAfterLocalDatetime(const int year, const int month, const int day, const int hour, const int min, const int sec)
	{
		return getLocalDatetime().getDatetimeAfter(year, month, day, hour, min, sec);
	}

	const Datetime getDatetimeAfterLocalDatetime(const Datetime & datetime)
	{
		return getLocalDatetime().getDatetimeAfter(datetime);
	}

	const Datetime getDatetimeAfterLocalDatetime(const Date & date)
	{
		return getLocalDatetime().getDatetimeAfter(date);
	}

	const Datetime getDatetimeAfterLocalDatetime(const int day)
	{
		return getLocalDatetime().getDatetimeAfter(day);
	}

	const Datetime getDatetimeAfterLocalDatetime(const Time & time)
	{
		return getLocalDatetime().getDatetimeAfter(time);
	}

	const Datetime getDatetimeBeforeLocalDatetime(const Datetime & datetime)
	{
		return getLocalDatetime().getDatetimeBefore(datetime);
	}

	const Datetime getDatetimeBeforeLocalDatetime(const Date & date)
	{
		return getLocalDatetime().getDatetimeBefore(date);
	}

	const Datetime getDatetimeBeforeLocalDatetime(const Time & time)
	{
		return getLocalDatetime().getDatetimeBefore(time);
	}

	const DatetimeMs getLocalDatetimeMs(void)
	{
		using namespace boost::gregorian;
		using namespace boost::posix_time;

		ptime localDatetime(second_clock::local_time());
		const date &localDate(localDatetime.date());
		const time_duration &localTime(localDatetime.time_of_day());

		return DatetimeMs(static_cast<short>(localDate.year()), static_cast<unsigned char>(localDate.month()), static_cast<unsigned char>(localDate.day()), static_cast<unsigned char>(localTime.hours()), static_cast<unsigned char>(localTime.minutes()), static_cast<unsigned char>(localTime.seconds()), static_cast<unsigned short>(localTime.fractional_seconds() / MicrosecondsPerMillisecond));
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const int year, const int month, const int day, const int hour, const int min, const int sec, const int msec)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(year, month, day, hour, min, sec, msec);
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Datetime & datetime)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(datetime.year, datetime.month, datetime.day, datetime.hour, datetime.min, datetime.sec);
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const DatetimeMs & datetime)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(datetime.year, datetime.month, datetime.day, datetime.hour, datetime.min, datetime.sec, datetime.msec);
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Date & date)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(date);
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const int day)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(day);
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const Time & time)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(time);
	}

	const DatetimeMs getDatetimeMsAfterLocalDatetimeMs(const TimeMs & time)
	{
		return getLocalDatetimeMs().getDatetimeMsAfter(time);
	}

	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Datetime & datetime)
	{
		return getLocalDatetimeMs().getDatetimeMsBefore(datetime);
	}

	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const DatetimeMs & datetime)
	{
		return getLocalDatetimeMs().getDatetimeMsBefore(datetime);
	}

	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Date & date)
	{
		return getLocalDatetimeMs().getDatetimeMsBefore(date);
	}

	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const Time & time)
	{
		return getLocalDatetimeMs().getDatetimeMsBefore(time);
	}

	const DatetimeMs getDatetimeMsBeforeLocalDatetimeMs(const TimeMs & time)
	{
		return getLocalDatetimeMs().getDatetimeMsBefore(time);
	}

	const bool isLeapYear(const unsigned short year)
	{
		return year % 4 == 0 && year % 100 != 0 ? true
			: year % 100 == 0 && year % 400 == 0 ? true : false;
	}

	const unsigned char getDayInWeek(unsigned short year, unsigned char month, const unsigned char day)
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

	const unsigned char getDayInWeek(const Date & date)
	{
		return getDayInWeek(date.year, date.month, date.day);
	}

	const unsigned char getDayInWeek(const Datetime & datetime)
	{
		return getDayInWeek(datetime.year, datetime.month, datetime.day);
	}

	const unsigned char getDaysOfMonth(const unsigned short year, const unsigned char month)
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

	const unsigned char getDaysOfMonth(const Date & date)
	{
		return getDaysOfMonth(date.year, date.month);
	}

	const unsigned char getDaysOfMonth(const Datetime & datetime)
	{
		return getDaysOfMonth(datetime.year, datetime.month);
	}

	const Datetime getBuildDatetime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString)
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

const bool operator<(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	return lhs.year < rhs.year ? true
		: lhs.year > rhs.year ? false
		: lhs.month < rhs.month ? true
		: lhs.month > rhs.month ? false
		: lhs.day < rhs.day ? true : false;
}

const bool operator<(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return lhs.totalSeconds() < rhs.totalSeconds();
}

const bool operator<(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return lhs.totalMilliseconds() < rhs.totalMilliseconds();
}

const bool operator<(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
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

const bool operator<(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
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
		: lhs.sec < rhs.sec ? true
		: lhs.sec > rhs.sec ? false
		: lhs.msec < rhs.msec ? true : false;
}

const bool operator<=(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	return lhs.year < rhs.year ? true
		: lhs.year > rhs.year ? false
		: lhs.month < rhs.month ? true
		: lhs.month > rhs.month ? false
		: lhs.day <= rhs.day ? true : false;
}

const bool operator<=(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return lhs.totalSeconds() <= rhs.totalSeconds();
}

const bool operator<=(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return lhs.totalMilliseconds() <= rhs.totalMilliseconds();
}

const bool operator<=(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
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
		: lhs.sec <= rhs.sec ? true : false;
}

const bool operator<=(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
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
		: lhs.sec < rhs.sec ? true 
		: lhs.sec > rhs.sec ? false
		: lhs.msec <= rhs.msec ? true : false;
}

const bool operator>(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	return lhs.year > rhs.year ? true
		: lhs.year < rhs.year ? false
		: lhs.month > rhs.month ? true
		: lhs.month < rhs.month ? false
		: lhs.day > rhs.day ? true : false;
}

const bool operator>(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return lhs.totalSeconds() > rhs.totalSeconds();
}

const bool operator>(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return lhs.totalMilliseconds() > rhs.totalMilliseconds();
}

const bool operator>(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
{
	return lhs.year > rhs.year ? true
		: lhs.year < rhs.year ? false
		: lhs.month > rhs.month ? true
		: lhs.month < rhs.month ? false
		: lhs.day > rhs.day ? true
		: lhs.day < rhs.day ? false
		: lhs.hour > rhs.hour ? true
		: lhs.hour < rhs.hour ? false
		: lhs.min > rhs.min ? true
		: lhs.min < rhs.min ? false
		: lhs.sec > rhs.sec ? true : false;
}

const bool operator>(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
{
	return lhs.year > rhs.year ? true
		: lhs.year < rhs.year ? false
		: lhs.month > rhs.month ? true
		: lhs.month < rhs.month ? false
		: lhs.day > rhs.day ? true
		: lhs.day < rhs.day ? false
		: lhs.hour > rhs.hour ? true
		: lhs.hour < rhs.hour ? false
		: lhs.min > rhs.min ? true
		: lhs.min < rhs.min ? false
		: lhs.sec > rhs.sec ? true 
		: lhs.sec < rhs.sec ? false
		: lhs.msec > rhs.msec ? true : false;
}

const bool operator>=(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	return lhs.year > rhs.year ? true
		: lhs.year < rhs.year ? false
		: lhs.month > rhs.month ? true
		: lhs.month < rhs.month ? false
		: lhs.day >= rhs.day ? true : false;
}

const bool operator>=(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return lhs.totalSeconds() >= rhs.totalSeconds();
}

const bool operator>=(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return lhs.totalMilliseconds() >= rhs.totalMilliseconds();
}

const bool operator>=(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
{
	return lhs.year > rhs.year ? true
		: lhs.year < rhs.year ? false
		: lhs.month > rhs.month ? true
		: lhs.month < rhs.month ? false
		: lhs.day > rhs.day ? true
		: lhs.day < rhs.day ? false
		: lhs.hour > rhs.hour ? true
		: lhs.hour < rhs.hour ? false
		: lhs.min > rhs.min ? true
		: lhs.min < rhs.min ? false
		: lhs.sec >= rhs.sec ? true : false;
}

const bool operator>=(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
{
	return lhs.year > rhs.year ? true
		: lhs.year < rhs.year ? false
		: lhs.month > rhs.month ? true
		: lhs.month < rhs.month ? false
		: lhs.day > rhs.day ? true
		: lhs.day < rhs.day ? false
		: lhs.hour > rhs.hour ? true
		: lhs.hour < rhs.hour ? false
		: lhs.min > rhs.min ? true
		: lhs.min < rhs.min ? false
		: lhs.sec > rhs.sec ? true 
		: lhs.sec < rhs.sec ? false
		: lhs.msec >= rhs.msec ? true : false;
}

const bool operator==(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	return lhs.year == rhs.year && lhs.month == rhs.month && lhs.day == rhs.day;
}

const bool operator==(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return lhs.totalSeconds() == rhs.totalSeconds();
}

const bool operator==(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return lhs.totalMilliseconds() == rhs.totalMilliseconds();
}

const bool operator==(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
{
	return lhs.year == rhs.year && lhs.month == rhs.month && lhs.day == rhs.day
		&& lhs.hour == rhs.hour && lhs.min == rhs.min && lhs.sec == rhs.sec;
}

const bool operator==(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
{
	return lhs.year == rhs.year && lhs.month == rhs.month && lhs.day == rhs.day
		&& lhs.hour == rhs.hour && lhs.min == rhs.min && lhs.sec == rhs.sec 
		&& lhs.msec == rhs.msec;
}

const bool operator!=(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	return lhs.year != rhs.year || lhs.month != rhs.month || lhs.day == rhs.day;
}

const bool operator!=(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return lhs.totalSeconds() != rhs.totalSeconds();
}

const bool operator!=(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return lhs.totalMilliseconds() != rhs.totalMilliseconds();
}

const bool operator!=(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
{
	return lhs.year != rhs.year || lhs.month != rhs.month || lhs.day != rhs.day
		|| lhs.hour != rhs.hour || lhs.min != rhs.min || lhs.sec != rhs.sec;
}

const bool operator!=(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
{
	return lhs.year != rhs.year || lhs.month != rhs.month || lhs.day != rhs.day
		|| lhs.hour != rhs.hour || lhs.min != rhs.min || lhs.sec != rhs.sec 
		|| lhs.msec != rhs.msec;
}

const DatetimeUtils::Time operator+(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return DatetimeUtils::Time(lhs.totalSeconds() + rhs.totalSeconds());
}

const DatetimeUtils::TimeMs operator+(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return DatetimeUtils::TimeMs(lhs.totalMilliseconds() + rhs.totalMilliseconds());
}

const DatetimeUtils::TimeMs operator+(const DatetimeUtils::Time & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return DatetimeUtils::TimeMs(lhs.totalSeconds() * DatetimeUtils::MillisecondsPerSecond + rhs.totalSeconds());
}

const DatetimeUtils::TimeMs operator+(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::Time & rhs)
{
	return DatetimeUtils::TimeMs(lhs.totalMilliseconds() + rhs.totalSeconds() * DatetimeUtils::MillisecondsPerSecond);
}

const DatetimeUtils::Time operator-(const DatetimeUtils::Time & lhs, const DatetimeUtils::Time & rhs)
{
	return DatetimeUtils::Time(lhs.totalSeconds() - rhs.totalSeconds());
}

const DatetimeUtils::TimeMs operator-(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return DatetimeUtils::TimeMs(lhs.totalMilliseconds() - rhs.totalMilliseconds());
}

const DatetimeUtils::TimeMs operator-(const DatetimeUtils::Time & lhs, const DatetimeUtils::TimeMs & rhs)
{
	return DatetimeUtils::TimeMs(lhs.totalSeconds() * DatetimeUtils::MillisecondsPerSecond - rhs.totalMilliseconds());
}

const DatetimeUtils::TimeMs operator-(const DatetimeUtils::TimeMs & lhs, const DatetimeUtils::Time & rhs)
{
	return DatetimeUtils::TimeMs(lhs.totalMilliseconds() - rhs.totalSeconds() * DatetimeUtils::MillisecondsPerSecond);
}

const int operator-(const DatetimeUtils::Date & lhs, const DatetimeUtils::Date & rhs)
{
	using namespace boost::gregorian;

	date lDate(lhs.year, lhs.month, lhs.day), rDate(rhs.year, rhs.month, rhs.day);

	return (lDate - rDate).days();
}

const DatetimeUtils::Time operator-(const DatetimeUtils::Datetime & lhs, const DatetimeUtils::Datetime & rhs)
{
	using namespace boost::gregorian;
	using namespace boost::posix_time;

	date lDate(lhs.year, lhs.month, lhs.day), rDate(rhs.year, rhs.month, rhs.day);
	time_duration lTime(lhs.hour, lhs.min, lhs.sec), rTime(rhs.hour, rhs.min, rhs.sec);
	ptime lDatetime(lDate, lTime), rDatetime(rDate, rTime);

	auto disTime(rhs < lhs ? (lDatetime - rDatetime) : (rDatetime - lDatetime));

	return DatetimeUtils::Time(static_cast<unsigned short>(disTime.hours()), static_cast<unsigned char>(disTime.minutes()), static_cast<unsigned char>(disTime.seconds()));
}

const DatetimeUtils::TimeMs operator-(const DatetimeUtils::DatetimeMs & lhs, const DatetimeUtils::DatetimeMs & rhs)
{
	using namespace boost::gregorian;
	using namespace boost::posix_time;

	date lDate(lhs.year, lhs.month, lhs.day), rDate(rhs.year, rhs.month, rhs.day);
	time_duration lTime(lhs.hour, lhs.min, lhs.sec), rTime(rhs.hour, rhs.min, rhs.sec);
	ptime lDatetime(lDate, lTime), rDatetime(rDate, rTime);

	auto disTime(rhs < lhs ? (lDatetime - rDatetime) : (rDatetime - lDatetime));

	return DatetimeUtils::TimeMs(static_cast<unsigned short>(disTime.hours()), static_cast<unsigned char>(disTime.minutes()), static_cast<unsigned char>(disTime.seconds()), static_cast<unsigned short>(disTime.fractional_seconds() / DatetimeUtils::MicrosecondsPerMillisecond));
}

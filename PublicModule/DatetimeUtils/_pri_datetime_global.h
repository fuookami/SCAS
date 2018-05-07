#pragma once

#include "Global.h"
#include <string>
#include <array>

namespace SSUtils
{
	namespace Datetime
	{
		extern const std::array<uint8, 12> DaysOfMonth;
		extern const std::array<std::string, 12> MonthShortName;
		extern const std::array<std::wstring, 7> DayInWeekChineseName;

		extern const uint16 MillisecondsPerSecond;
		extern const uint16 MicrosecondsPerMillisecond;
		extern const uint16 SecondsPerHour;
		extern const uint8 SecondsPerMinute;
		extern const uint8 MinutesPerHour;
		extern const uint8 HoursPerDay;

		extern const uint8 FractionSecondDigits;
		extern const uint32 MicrosecondPerFractionSecond;

		class Date;
		class DateDuration;
		class Time;
		class TimeDuration;
		class Datetime;
		class DatetimeDuration;

		enum class Precision
		{
			Second,
			MicroSecond,
			MilliSecond
		};
	};
};

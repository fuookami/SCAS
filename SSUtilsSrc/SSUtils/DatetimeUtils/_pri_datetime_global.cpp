#include "stdafx.h"
#include "_pri_datetime_global.h"
#include <boost/date_time/posix_time/posix_time.hpp>
#include <cmath>

namespace SSUtils
{
	namespace Datetime
	{
		const std::array<unsigned char, 12> DaysOfMonth =
		{
			31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
		};

		const std::array<std::string, 12> MonthShortName =
		{
			std::string("Jan"), std::string("Feb"), std::string("Mar"),
			std::string("Apr"), std::string("May"), std::string("Jun"),
			std::string("Jul"), std::string("Aug"), std::string("Sep"),
			std::string("Oct"), std::string("Nov"), std::string("Dec")
		};

		const std::array<std::wstring, 7> DayInWeekChineseName =
		{
			std::wstring(L"星期日"), std::wstring(L"星期一"), std::wstring(L"星期二"),
			std::wstring(L"星期三"), std::wstring(L"星期四"), std::wstring(L"星期五"),
			std::wstring(L"星期六")
		};

		const uint16 MillisecondsPerSecond = 1000;
		const uint16 MicrosecondsPerMillisecond = 1000;
		const uint16 SecondsPerHour = 3600;
		const uint8 SecondsPerMinute = 60;
		const uint8 MinutesPerHour = 60;
		const uint8 HoursPerDay = 24;

		const uint8 FractionSecondDigits = 6;
		const uint32 MicrosecondPerFractionSecond = static_cast<uint32>(pow(10, boost::posix_time::time_duration::num_fractional_digits() - FractionSecondDigits));
	};
};

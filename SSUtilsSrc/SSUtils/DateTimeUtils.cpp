#include "stdafx.h"
#include "DatetimeUtils.h"
#include <boost/date_time/gregorian/gregorian.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <sstream>

namespace SSUtils
{
	namespace Datetime
	{
		Datetime getBuildDatetime(const std::string & BuildOriginalDateString, const std::string & BuildTimeString)
		{
			static const auto fromShortMonthName(
				[](const std::string &shortName) -> uint8
			{
				return static_cast<uint8>(std::find(MonthShortName.cbegin(), MonthShortName.cend(), shortName) - MonthShortName.cbegin()) + 1;
			});

			using namespace boost::gregorian;
			using namespace boost::posix_time;

			std::string BuildYear, BuileMonth, BuildDay;
			std::istringstream sin(BuildOriginalDateString);
			sin >> BuileMonth >> BuildDay >> BuildYear;

			std::string BuildHour(BuildTimeString.cbegin(), BuildTimeString.cbegin() + 2);
			std::string BuildMinute(BuildTimeString.cbegin() + 3, BuildTimeString.cbegin() + 5);
			std::string BuildSecond(BuildTimeString.cbegin() + 6, BuildTimeString.cbegin() + 8);

			return Datetime(static_cast<int16>(std::stoi(BuildYear)), fromShortMonthName(BuileMonth), static_cast<uint8>(std::stoi(BuildDay)),
				std::stoi(BuildHour), static_cast<uint8>(std::stoi(BuildMinute)), static_cast<uint8>(std::stoi(BuildSecond)));
		}
	};
};

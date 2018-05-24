#pragma once

#include "DatetimeUtils/_pri_datetime_global.h"
#include "DateTimeUtils/Date.h"
#include "DatetimeUtils/Time.h"
#include "DatetimeUtils/Datetime.h"

namespace SSUtils
{
	namespace Datetime
	{
		SSUtils_API_DECLSPEC Datetime getBuildDatetime(const std::string &BuildOriginalDateString, const std::string &BuildTimeString);
	};
};

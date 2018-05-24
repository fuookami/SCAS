#include "stdafx.h"
#include "ThreadUtils.h"
#include <thread>
#include <chrono>

namespace SSUtils
{
	namespace Thread
	{
		void sleep(const uint64 milliseconds)
		{
			std::this_thread::sleep_for(std::chrono::milliseconds(milliseconds));
		}

		std::thread::id getThisThreadId(void)
		{
			return std::this_thread::get_id();
		}
	};
};

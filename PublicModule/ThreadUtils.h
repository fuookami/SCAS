#pragma once

#include "ThreadUtils/_pri_thread_global.h"
#include "ThreadUtils/ThreadPool.h"
#include <thread>

namespace SSUtils
{
	namespace Thread
	{
		void sleep(const uint64 milliseconds);
		std::thread::id getThisThreadId(void);
	};
};

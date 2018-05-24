#pragma once

#include "..\Global.h"
#include <utility>
#include <memory>
#include <thread>
#include <future>
#include <functional>
#include <type_traits>

namespace SSUtils
{
	namespace Thread
	{
		template<typename F, typename... Args>
		auto run(F &&f, Args&&... args) 
			-> std::future<typename std::result_of<F(Args...)>::type>
		{
			return std::async(std::launch::async, std::bind(std::forward<F>(f), std::forward<Args>(args)...));
		}

		template<typename F, typename... Args>
		auto sharedRun(F &&f, Args&&... args) 
			-> std::shared_future<typename std::result_of<F(Args...)>::type>
		{
			return std::async(std::launch::async, std::bind(std::forward<F>(f), std::forward<Args>(args)...));
		}

		template<typename F, typename... Args>
		void detch(F &&f, Args&&... args)
		{
			std::async(std::launch::async, std::bind(std::forward<F>(f), std::forward<Args>(args)...));
		}

		class ThreadPool;
	};
};

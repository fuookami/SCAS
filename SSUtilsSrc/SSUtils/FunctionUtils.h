#pragma once

#include "FunctionUtils/Task.h"
#include <future>

namespace SSUtils
{
	namespace Function
	{
		template<typename F, typename... Args>
		auto packageToTask(F &&f, Args&&... args)
			-> std::shared_ptr<std::packaged_task<typename std::result_of<F(Args...)>::type>>
		{
			using RetType = typename std::result_of<F(Args...)>::type;

			return std::make_shared<std::packaged_task<RetType()>>(std::bind(std::forward<F>(f), std::forward<Args>(args)...));
		}

		template<typename F, typename... Args>
		auto packageToFuture(F &&f, Args&&... args)
			-> std::pair<std::function<void()>, std::future<typename std::result_of<F(Args...)>::type>>
		{
			auto task(packageToTask(std::forward<F>(f), std::forward<Args>(args)...));
			return std::make_pair([task]() { (*task)(); }, task->get_future());
		}

		template<typename F, typename... Args>
		auto packageToSharedFuture(F &&f, Args&&... args)
			-> std::pair<std::function<void()>, std::shared_future<typename std::result_of<F(Args...)>::type>>
		{
			auto task(packageToTask(std::forward<F>(f), std::forward<Args>(args)...));
			return std::make_pair([task]() { (*task)(); }, task->get_future());
		}

		template<typename F, typename... Args>
		auto packageTask(F &&f, Args&&... args)
			-> std::pair<std::shared_ptr<Task>, std::future<typename std::result_of<F(Args...)>::type>>
		{
			auto task(packageToTask(std::forward<F>(f), std::forward<Args>(args)...));
			return std::make_pair(std::shared_ptr<Task>(new Task([task]() { (*task)(); })), task->get_future());
		}

		template<typename F, typename... Args>
		auto packageSharedTask(F &&f, Args&&... args)
			-> std::pair<std::shared_ptr<Task>, std::shared_future<typename std::result_of<F(Args...)>::type>>
		{
			auto task(packageToTask(std::forward<F>(f), std::forward<Args>(args)...));
			return std::make_pair(std::shared_ptr<Task>(new Task([task]() { (*task)(); })), task->get_future());
		}
	};
};

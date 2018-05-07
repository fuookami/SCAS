#pragma once

#include "_pri_thread_global.h"
#include "SystemUtils.h"
#include "FunctionUtils.h"
#include <deque>
#include <mutex>
#include <condition_variable>
#include <stdexcept>
#include <ctime>

namespace SSUtils
{
	namespace Thread
	{
		class ThreadPool
		{
		public:
			ThreadPool(const uint32 size = System::CPUCoreNumber, const Function::TaskCompareFunction taskCompareFun = Function::DefaultTaskCompareFunction);
			ThreadPool(const ThreadPool &ano) = delete;
			ThreadPool(ThreadPool &&ano) = default;
			ThreadPool &operator=(const ThreadPool &rhs) = delete;
			ThreadPool &operator=(ThreadPool &&rhs) = default;
			~ThreadPool(void);

			template<typename F, typename... Args>
			auto commit(F &&f, Args&&... args)
				-> std::pair<bool, std::future<typename std::result_of<F(Args...)>::type>>;
			template<typename F, typename... Args>
			auto commit(const std::shared_ptr<std::packaged_task<typename std::result_of<F(Args...)>::type>> task, const uint32 priority = 0, const uint32 time = static_cast<uint32>(clock()))
				->std::pair<bool, std::future<typename std::result_of<F(Args...)>::type>>;
			template<typename F, typename... Args>
			auto sharedCommit(F &&f, Args&&... args)
				->std::pair<bool, std::shared_future<typename std::result_of<F(Args...)>::type>>;
			template<typename F, typename... Args>
			auto sharedCommit(const std::shared_ptr<std::packaged_task<typename std::result_of<F(Args...)>::type>> task, const uint32 priority = 0, const uint32 time = static_cast<uint32>(clock()))
				->std::pair<bool, std::shared_future<typename std::result_of<F(Args...)>::type>>;
			const bool commit(const std::function<void()> task, const uint32 priority = 0, const uint32 time = static_cast<uint32>(clock()));
			const bool commit(const std::shared_ptr<Function::Task> task);

			void resize(const uint32 size);
			const uint32 size(void) const;
			const uint32 sleepSize(void) const;

			const std::deque<std::shared_ptr<Function::Task>> blockedTasks(void) const;
			const uint32 blockedTaskNumber(void) const;

			void setTaskCompareFunction(const Function::TaskCompareFunction taskCompareFun);

			void start(void);
			void stop(void);
			const bool isStoped(void) const;

		private:
			class Worker : public std::thread
			{
				friend class ThreadPool;

			public:
				Worker(ThreadPool *pool);
				Worker(const Worker &ano) = delete;
				Worker(Worker &&ano) = default;
				Worker &operator=(const Worker &rhs) = delete;
				Worker &operator=(Worker &&rhs) = delete;
				~Worker(void) = default;

				void stop(void);

			private:
				std::atomic<uint32> m_stoped;
			};

		private:
			void resize(void);
			void insertTask(const std::shared_ptr<Function::Task> task);
			static void WorkerMain(ThreadPool *pool, Worker *self);

		private:
			std::vector<std::shared_ptr<Worker>> m_workers;
			std::deque<std::shared_ptr<Function::Task>> m_blockedTasks;
			Function::TaskCompareFunction m_taskCompareFun;

			std::atomic<uint32> m_workerNumber;
			std::atomic<uint32> m_sleepNumber;

			std::mutex m_mutex;
			std::condition_variable m_condition;
			std::atomic<bool> m_stoped;
		};

		template<typename F, typename ...Args>
		auto ThreadPool::commit(F && f, Args && ...args) -> std::pair<bool, std::future<typename std::result_of<F(Args ...)>::type>>
		{
			using FutureType = std::future<typename std::result_of<F(Args ...)>::type>;

			if (m_stoped)
			{
				return std::make_pair(false, FutureType());
			}

			auto taskPair(Function::packageTask(std::forward<F>(f), std::forward<Args>(args)...));
			insertTask(taskPair.first);
			m_condition.notify_one();

			return std::make_pair(true, std::move(taskPair.second));
		}

		template<typename F, typename ...Args>
		auto ThreadPool::commit(const std::shared_ptr<std::packaged_task<typename std::result_of<F(Args...)>::type>> task, const uint32 priority, const uint32 time) -> std::pair<bool, std::future<typename std::result_of<F(Args ...)>::type>>
		{
			using FutureType = std::future<typename std::result_of<F(Args ...)>::type>;

			if (m_stoped)
			{
				return std::make_pair(false, FutureType());
			}

			insertTask(std::make_shared<Function::Task>([task]() { (*task)(); }, priority, time));
			m_condition.notify_one();

			return task->get_future();
		}

		template<typename F, typename ...Args>
		auto ThreadPool::sharedCommit(F && f, Args && ...args) -> std::pair<bool, std::shared_future<typename std::result_of<F(Args ...)>::type>>
		{
			using FutureType = std::shared_future<typename std::result_of<F(Args ...)>::type>;

			if (m_stoped)
			{
				return std::make_pair(false, FutureType());
			}

			auto taskPair(Function::packageSharedTask(std::forward<F>(f), std::forward<Args>(args)...));
			insertTask(taskPair.first);
			m_condition.notify_one();

			return std::make_pair(true, std::move(taskPair.second));
		}

		template<typename F, typename ...Args>
		auto ThreadPool::sharedCommit(const std::shared_ptr<std::packaged_task<typename std::result_of<F(Args...)>::type>> task, const uint32 priority, const uint32 time) -> std::pair<bool, std::shared_future<typename std::result_of<F(Args ...)>::type>>
		{
			using FutureType = std::shared_future<typename std::result_of<F(Args ...)>::type>;

			insertTask(std::make_shared<Function::Task>([task]() { (*task)(); }, priority, time));
			m_condition.notify_one();

			return task->get_future();
		}
	};
};

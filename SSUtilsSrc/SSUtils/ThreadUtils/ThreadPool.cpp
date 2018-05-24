#include "stdafx.h"
#include "ThreadPool.h"

namespace SSUtils
{
	namespace Thread
	{
		ThreadPool::ThreadPool(const uint32 size, const Function::TaskCompareFunction taskCompareFun)
			: m_workerNumber(size), m_taskCompareFun(taskCompareFun)
		{
			start();
		}

		ThreadPool::~ThreadPool(void)
		{
			stop();
		}

		const bool ThreadPool::commit(const std::function<void()> task, const uint32 priority, const uint32 time)
		{
			if (m_stoped)
			{
				return false;
			}

			insertTask(std::make_shared<Function::Task>(task, priority, time));
			m_condition.notify_one();

			return true;
		}

		const bool ThreadPool::commit(const std::shared_ptr<Function::Task> task)
		{
			if (m_stoped)
			{
				return false;
			}

			insertTask(task);
			m_condition.notify_one();

			return true;
		}

		void ThreadPool::resize(const uint32 size)
		{
			std::lock_guard<std::mutex> lock(m_mutex);
			m_workerNumber.store(size);
			if (!m_stoped)
			{
				resize();
			}
		}

		const uint32 ThreadPool::size(void) const
		{
			return m_workerNumber;
		}

		const uint32 ThreadPool::sleepSize(void) const
		{
			return m_sleepNumber;
		}

		const std::deque<std::shared_ptr<Function::Task>> ThreadPool::blockedTasks(void) const
		{
			return m_blockedTasks;
		}

		const uint32 ThreadPool::blockedTaskNumber(void) const
		{
			return static_cast<uint32>(m_blockedTasks.size());
		}

		void ThreadPool::setTaskCompareFunction(const Function::TaskCompareFunction taskCompareFun)
		{
			std::lock_guard<std::mutex> lock(m_mutex);
			m_taskCompareFun = taskCompareFun;
		}

		void ThreadPool::start(void)
		{
			std::lock_guard<std::mutex> lock(m_mutex);
			if (m_stoped)
			{
				m_stoped.store(false);
				resize();

				for (uint32 i(0), j(static_cast<uint32>(m_blockedTasks.size())); i != j; ++i)
				{
					m_condition.notify_one();
				}
			}
		}

		void ThreadPool::stop(void)
		{
			std::lock_guard<std::mutex> lock(m_mutex);
			if (!m_stoped)
			{
				m_stoped.store(true);
				m_condition.notify_all();

				for (auto worker : m_workers)
				{
					worker->stop();
					worker->join();
				}
				m_sleepNumber.store(m_workerNumber);
				m_workers.clear();
			}
		}

		const bool ThreadPool::isStoped(void) const
		{
			return m_stoped;
		}

		void ThreadPool::resize(void)
		{
			if (!m_stoped)
			{
				if (m_workers.size() > m_workerNumber)
				{
					for (uint32 i(0), j(static_cast<uint32>(m_workers.size()) - m_workerNumber); i != j; ++i)
					{
						m_workers.back()->stop();
						m_workers.back()->detach();
						m_workers.pop_back();
					}
				}
				else if (m_workers.size() < m_workerNumber)
				{
					for (uint32 i(0), j(m_workerNumber - static_cast<uint32>(m_workers.size())); i != j; ++i)
					{
						m_workers.push_back(std::make_shared<Worker>(this));
					}
				}
			}
		}

		void ThreadPool::insertTask(const std::shared_ptr<Function::Task> task)
		{
			m_blockedTasks.insert(std::find_if(m_blockedTasks.begin(), m_blockedTasks.end(), 
				[task](const std::shared_ptr<Function::Task> &value)->bool
			{
				return task->priority < value->priority;
			}), task);
		}

		void ThreadPool::WorkerMain(ThreadPool * pool, Worker * self)
		{
			while (!self->m_stoped)
			{
				std::unique_lock<std::mutex> lock(pool->m_mutex);
				pool->m_condition.wait(lock, [pool, self]() 
				{
					return !self->m_stoped || !pool->m_blockedTasks.empty();
				});

				if (self->m_stoped && pool->m_blockedTasks.empty())
				{
					return;
				}
				auto task = std::move(pool->m_blockedTasks.front()->task);
				pool->m_blockedTasks.pop_front();

				--pool->m_sleepNumber;
				task();
				++pool->m_sleepNumber;
			}
		}

		ThreadPool::Worker::Worker(ThreadPool *pool)
			: std::thread(WorkerMain, pool, this), m_stoped(false)
		{
		}

		void ThreadPool::Worker::stop(void)
		{
			m_stoped.store(true);
		}
	};
};

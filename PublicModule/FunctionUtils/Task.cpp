#include "Task.h"

namespace SSUtils
{
	namespace Function
	{
		Task::Task(const std::function<void()> _task, const uint32 _priority, const uint32 _time)
			: task(_task), priority(_priority), time(_time)
		{
		}

		const uint32 Task::waitMilliseconds(void) const
		{
			return static_cast<uint32>(clock()) - time;
		}

		const bool DefaultTaskCompare(const std::shared_ptr<Task> lhs, const std::shared_ptr<Task> rhs)
		{
			return lhs->priority > rhs->priority;
		}

		const TaskCompareFunction DefaultTaskCompareFunction = DefaultTaskCompare;
	};
};

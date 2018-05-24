#pragma once

#include "_pri_math_global.h"
#include <set>

namespace SSUtils
{
	namespace Math
	{
		SSUtils_API_DECLSPEC std::vector<int> TopologicalSort(const std::vector<std::set<int>>& table);
	};
};

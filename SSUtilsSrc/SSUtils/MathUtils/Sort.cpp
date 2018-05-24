#include "stdafx.h"
#include "Sort.h"
#include <deque>
#include <algorithm>

namespace SSUtils
{
	namespace Math
	{
		std::vector<int> TopologicalSort(const std::vector<std::set<int>>& table)
		{
			std::vector<int> inDegrees;
			std::transform(table.cbegin(), table.cend(), std::back_inserter(inDegrees),
				[](const std::set<int> &item) -> int
			{
				return item.size();
			});

			std::deque<int> items;
			for (int i(0), j(inDegrees.size()); i != j; ++i)
			{
				if (inDegrees[i] == 0 || (inDegrees[i] == 1 && *table[i].cbegin() == -1))
				{
					items.push_back(i);
				}
			}

			std::vector<int> ret;
			while (!items.empty())
			{
				int item(items.front());
				items.pop_front();

				for (int i(0), j(table.size()); i != j; ++i)
				{
					if (table[i].find(item) != table[i].cend())
					{
						--inDegrees[i];

						if (inDegrees[i] == 0)
						{
							items.push_back(i);
						}
					}
				}

				ret.push_back(item);
			}

			return ret;
		}
	};
};

#pragma once

#include <random>

namespace RandomUtils
{
	std::mt19937 generateNewRandomGenerator(void);
	std::mt19937_64 generateNewRandomGenerator_64(void);

	std::mt19937 &getGlobalRandomGenerator(void);
	std::mt19937_64 &getGlobalRandomGenerator_64(void);
};

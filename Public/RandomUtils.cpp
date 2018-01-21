#include "RandomUtils.h"

namespace RandomUtils
{
	std::mt19937 generateNewRandomGenerator(void)
	{
		std::mt19937 gen;
		static std::random_device device;
		std::seed_seq seq{ device(), device(), device(), device() };
		gen.seed(seq);

		return gen;
	}

	std::mt19937_64 generateNewRandomGenerator_64(void)
	{
		std::mt19937_64 gen;
		static std::random_device device;
		std::seed_seq seq{ device(), device(), device(), device() };
		gen.seed(seq);

		return gen;
	}

	std::mt19937 & getGlobalRandomGenerator(void)
	{
		static std::mt19937 gen(generateNewRandomGenerator());
		return gen;
	}

	std::mt19937_64 & getGlobalRandomGenerator_64(void)
	{
		static std::mt19937_64 gen(generateNewRandomGenerator_64());
		return gen;
	}
};

#pragma once

#include "Global.h"
#include <random>
#include <boost/random.hpp>

namespace SSUtils
{
	namespace Random
	{
		template<typename T, uint32 digits = std::numeric_limits<T>::digits>
		using generator_type = boost::random::independent_bits_engine<std::mt19937, digits, T>;

		template<typename T, uint32 digits = std::numeric_limits<T>::digits>
		using generator_type_64 = boost::random::independent_bits_engine<std::mt19937_64, digits, T>;

		template<typename T, uint32 digits = std::numeric_limits<T>::digits>
		generator_type<T, digits> generateNewRandomGenerator(void)
		{
			static std::random_device device;
			generator_type<T, digits> gen;
			std::seed_seq seq{ device(), device(), device(), device() };
			gen.seed(seq);
			return gen;
		}
		template<typename T, uint32 digits = std::numeric_limits<T>::digits>
		generator_type_64<T, digits> generateNewRandomGenerator_64(void)
		{
			static std::random_device device;
			generator_type_64<T, digits> gen;
			std::seed_seq seq{ device(), device(), device(), device() };
			gen.seed(seq);
			return gen;
		}

		template<typename T, uint32 digits = std::numeric_limits<T>::digits>
		generator_type<T, digits> &getGlobalRandomGenerator(void)
		{
			static generator_type<T, digits> gen(generateNewRandomGenerator<T, digits>());
			return gen;
		}
		template<typename T, uint32 digits = std::numeric_limits<T>::digits>
		generator_type_64<T, digits> &getGlobalRandomGenerator_64(void)
		{
			static generator_type_64<T, digits> gen(generateNewRandomGenerator_64<T, digits>());
			return gen;
		}
	};
};

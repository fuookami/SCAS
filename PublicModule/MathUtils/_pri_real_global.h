#pragma once

#include "_pri_math_global.h"
#include <boost/any.hpp>

namespace SSUtils
{
	namespace Math
	{
		enum class RoundFlag
		{
			round,
			ceil,
			floor
		};

		template<bool Signed = true> class IntegerWrapper;
		using Integer = IntegerWrapper<>;
		using UInteger = IntegerWrapper<false>;

		template<uint32 Digits = DefaultDigits> class DecimalWrapper;
		using Decimal = DecimalWrapper<>;

		template<uint32 Digits = DefaultDigits> class RationalWrapper;
		using Rational = RationalWrapper<>;
		template<uint32 Digits = DefaultDigits> class PowerWrapper;
		using Power = PowerWrapper<>;
		template<uint32 Digits = DefaultDigits> class LogarithmWrapper;
		using Logarithm = LogarithmWrapper<>;

		template<uint32 Digits = DefaultDigits> class RealWrapper;
		using Real = RealWrapper<>;

		template<uint32 Digits = DefaultDigits>
		struct RealTypeGroup
		{
			static const uint32 digits = Digits;
			typedef IntegerWrapper<true> integer_type;
			typedef IntegerWrapper<false> uinteger_type;
			typedef DecimalWrapper<Digits> decimal_type;
			typedef RationalWrapper<Digits> rational_type;
			typedef PowerWrapper<Digits> power_type;
			typedef LogarithmWrapper<Digits> logarithm_type;
		};
	};
};

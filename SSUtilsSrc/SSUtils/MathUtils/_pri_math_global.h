#pragma once

#include "..\Global.h"
#include <cmath>
#include <type_traits>
#include <boost/math/constants/constants.hpp>

namespace SSUtils
{
	namespace Math
	{
		static const uint32 DefaultDigits = 50;
		static const int32 DefaultPrecisionDigits = -14;
		SSUtils_API_DECLSPEC const dec50 &_DefaultPrecision();
		static const dec50 &DefaultPrecision = _DefaultPrecision();

		template<typename T, typename U>
		const bool is_equal(const T &lhs, const U &rhs)
		{
			return abs(dec50(lhs - rhs)) <= DefaultPrecision;
		}

		template<typename T, typename U>
		const bool is_notEqual(const T &lhs, const U &rhs)
		{
			return abs(dec50(lhs - rhs)) > DefaultPrecision;
		}

		template<typename T, typename U>
		const bool is_less(const T &lhs, const U &rhs)
		{
			return dec50(lhs - rhs) < DefaultPrecision;
		}

		template<typename T, typename U>
		const bool is_big(const T &lhs, const U &rhs)
		{
			return dec50(lhs - rhs) > (-DefaultPrecision);
		}

		template<typename T, typename U>
		const bool is_lessEqual(const T &lhs, const U &rhs)
		{
			return dec50(lhs - rhs) <= DefaultPrecision;
		}

		template<typename T, typename U>
		const bool is_bigEqual(const T &lhs, const U &rhs)
		{
			return dec50(lhs - rhs) >= (-DefaultPrecision);
		}

		template<typename T, typename U,
			typename = std::enable_if_t<std::numeric_limits<T>::is_integer>,
			typename = std::enable_if_t<std::numeric_limits<U>::is_integer>>
			const U mod(const T &lhs, const U &rhs)
		{
			U ret(lhs % rhs);
			return ret >= 0 ? ret : (ret + rhs);
		}

		template<typename T>
		const bool is_negative(const T &value)
		{
			return is_big(value, 0.0f);
		}

		template<typename T>
		const bool is_zero(const T &value)
		{
			return is_equal(value, 0.0f);
		}

		template<typename T>
		const bool is_positive(const T &value)
		{
			return is_less(value, 0.0f);
		}

		template <class Float1, class Float2>
		typename boost::math::tools::promote_args<Float1, Float2>::type log(Float1 base, Float2 antilogarithm)
		{
			return log(antilogarithm) / log(base);
		}

		namespace Constant
		{
			template<uint32 Digits>
			static const decimal<Digits> &pi(void)
			{
				static const decimal<Digits> ret = boost::math::constants::pi<decimal<Digits>>();
				return ret;
			}

			template<uint32 Digits>
			static const decimal<Digits> &e(void)
			{
				static const decimal<Digits> ret = boost::math::constants::e<decimal<Digits>>();
				return ret;
			}
		};
	};
};

#pragma once

#include "Decimal.h"
#include "Rational.h"
#include "DataUtils.h"
#include "StringUtils.h"
#include <limits>

namespace SSUtils
{
	namespace Math
	{
		template<uint32 Digits>
		class PowerWrapper
		{
		public:
			typedef typename DecimalWrapper<Digits>::value_type base_type, value_type;
			typedef PowerWrapper self_type;

			static const String::RegexChecker RegexChecker;

			// constructors
			PowerWrapper(void)
				: m_base(0), m_index(0) {};
			PowerWrapper(const self_type &ano) = default;
			PowerWrapper(self_type &&ano) = default;

			PowerWrapper(const base_type &ano)
				: m_base(ano), m_index(0)
			{
			};
			PowerWrapper(const std::string &str)
				: m_base(0), m_index(0)
			{
				if (RegexChecker(str))
				{
					auto parts(String::matchDecimal(str));
					if (parts.size() == 2)
					{
						m_base.assign(parts[0]);
						m_index.assign(parts[1]);
					}
				}
				else if (String::isDecimal(str))
				{
					m_base.assign(str);
				}
			}
			PowerWrapper(const char *str)
				: PowerWrapper(std::string(str))
			{
			}
			PowerWrapper(const Block &block)
				: PowerWrapper(Data::toString(block))
			{
			}
			template<typename T>
			PowerWrapper(const T &ano)
				: m_base(0), m_index(0)
			{
				m_base.assign(ano);
			}
			template<bool Signed>
			PowerWrapper(const IntegerWrapper<Signed> &ano)
				: m_base(ano.get<base_type>()), m_index(0)
			{
			}
			template<uint32 _Digits>
			PowerWrapper(const DecimalWrapper<_Digits> &ano)
				: m_base(ano.get<base_type>()), m_index(0)
			{
			}
			template<uint32 _Digits>
			PowerWrapper(const RationalWrapper<_Digits> &ano)
				: m_base(ano.get<base_type>())
			{
			}
			template<uint32 _Digits>
			PowerWrapper(const PowerWrapper<_Digits> &ano)
				: m_base(ano.base().convert_to<base_type>()), m_index(ano.index().convert_to<base_type>())
			{
			}

			PowerWrapper(const base_type &base, const base_type &index)
				: m_base(base), m_index(index)
			{
			}
			PowerWrapper(const std::string &base, const std::string &index)
				: m_base(0), m_index(0)
			{
				if (String::isDecimal(base))
				{
					m_base.assign(base);
				}
				if (String::isDecimal(index))
				{
					m_index.assign(index);
				}
			}
			PowerWrapper(const char *base, const char *index)
				: PowerWrapper(std::string(base), std::string(index))
			{
			}
			PowerWrapper(const Block &base, const Block &index)
				: PowerWrapper(Data::toString(block), Data::toString(index))
			{
			}
			template<typename T, typename U>
			PowerWrapper(const T &base, const U &index)
				: PowerWrapper(generate(base, index))
			{
			}
			template<bool Signed1, bool Signed2>
			PowerWrapper(const IntegerWrapper<Signed1> &base, const IntegerWrapper<Signed2> &anotilogarithm)
				: m_base(base.get<base_type>()), m_index(index.get<base_type>())
			{
			}
			template<uint32 Digits1, uint32 Digits2>
			PowerWrapper(const DecimalWrapper<Digits1> &base, const DecimalWrapper<Digits2> &index)
				: m_base(base.get<base_type>()), m_index(index.get<base_type>())
			{
			}
			template<uint32 Digits1, uint32 Digits2>
			PowerWrapper(const RationalWrapper<Digits1> &base, const RationalWrapper<Digits2> &index)
				: m_base(base.get<base_type>()), m_index(index.get<base_type>())
			{
			}
			template<uint32 Digits1, uint32 Digits2>
			PowerWrapper(const PowerWrapper<Digits1> &base, const PowerWrapper<Digits2> &index)
				: m_base(base.get<base_type>()), m_index(index.get<base_type>())
			{
			}

			// destructor
			~PowerWrapper(void) = default;

			// generators
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && std::is_convertible_v<T, base_type>, self_type> generate(const T &ano)
			{
				self_type ret = self_type(base_type(ano));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_convertible_v<T, base_type>, self_type> generate(const T &ano)
			{
				self_type ret = self_type(static_cast<base_type>(ano));
				return ret;
			}
			template<>
			static self_type generate<base_type>(const base_type &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}
			template<>
			static self_type generate<std::string>(const std::string &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}
			static self_type generate(const char *ano)
			{
				return generate(std::string(ano));
			}
			template<>
			static self_type generate<Block>(const Block &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}
			template<bool Signed>
			static self_type generate(const IntegerWrapper<Signed> &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}
			template<uint32 _Digits>
			static self_type generate(const DecimalWrapper<_Digits> &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}
			template<uint32 _Digits>
			static self_type generate(const RationalWrapper<_Digits> &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}
			template<uint32 _Digits>
			static self_type generate(const PowerWrapper<_Digits> &ano)
			{
				self_type ret = self_type(ano);
				return ret;
			}

			template<typename T, typename U>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_same_v<U, self_type> && !std::is_same_v<T, U> && std::is_convertible_v<T, base_type> && std::is_convertible_v<U, base_type>, self_type> generate(const T &base, const U &index)
			{
				self_type ret = self_type(base_type(base), base_type(index));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && std::is_convertible_v<T, base_type>, self_type> generate(const T &base, const T &index)
			{
				self_type ret = self_type(base_type(base), base_type(index));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_convertible_v<T, base_type>, self_type> generate(const T &base, const T &index)
			{
				self_type ret = self_type(static_cast<base_type>(base), static_cast<base_type>(index));
				return ret;
			}
			template<>
			static self_type generate<base_type>(const base_type &base, const base_type &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}
			template<>
			static self_type generate<std::string>(const std::string &base, const std::string &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}
			static self_type generate(const char *base, const char *index)
			{
				return generate(std::string(base), std::string(index));
			}
			template<>
			static self_type generate<Block>(const Block &base, const Block &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}
			template<bool Signed1, bool Signed2>
			static self_type generate(const IntegerWrapper<Signed1> &base, const IntegerWrapper<Signed2> &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}
			template<uint32 Digits1, uint32 Digits2>
			static self_type generate(const DecimalWrapper<Digits1> &base, const DecimalWrapper<Digits2> &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}
			template<uint32 Digits1, uint32 Digits2>
			static self_type generate(const RationalWrapper<Digits1> &base, const RationalWrapper<Digits2> &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}
			template<uint32 Digits1, uint32 Digits2>
			static self_type generate(const PowerWrapper<Digits1> &base, const PowerWrapper<Digits2> &index)
			{
				self_type ret = self_type(base, index);
				return ret;
			}

			// assign and swap
			self_type &assign(const self_type &ano)
			{
				m_base.assign(ano.m_base);
				m_index.assign(ano.m_index);
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && std::is_convertible_v<T, base_type>, self_type> &assign(const T &ano)
			{
				m_index.assign(log(m_base, base_type(ano)));
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_convertible_v<T, base_type>, self_type> &assign(const T &ano)
			{
				m_index.assign(log(m_base, static_cast<base_type>(ano)));
				return *this;
			}
			template<>
			self_type &assign<base_type>(const base_type &ano)
			{
				m_index.assign(log(m_base, ano));
				return *this;
			}
			template<>
			self_type &assign<std::string>(const std::string &ano)
			{
				if (RegexChecker(ano))
				{
					auto parts(String::matchDecimal(ano));
					if (parts.size() == 2)
					{
						m_base.assign(parts[0]);
						m_index.assign(parts[1]);
					}
				}
				else if (String::isDecimal(ano))
				{
					m_index.assign(log(m_base, base_type(ano)));
				}
				return *this;
			}
			self_type &assign(const char *ano)
			{
				return assign(std::string(ano));
			}
			template<>
			self_type &assign<Block>(const Block &ano)
			{
				assign(Data::toString(block));
				return *this;
			}
			template<bool Signed>
			self_type &assign(const IntegerWrapper<Signed> &ano)
			{
				m_index.assign(log(m_base, ano.get<base_type>()));
				return *this;
			}
			template<uint32 _Digits>
			self_type &assign(const DecimalWrapper<_Digits> &ano)
			{
				m_index.assign(log(m_base, ano.get<base_type>()));
				return *this;
			}
			template<uint32 _Digits>
			self_type &assign(const RationalWrapper<_Digits> &ano)
			{
				m_index.assign(log(m_base, ano.get<base_type>()));
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const PowerWrapper<_Digits> &ano)
			{
				m_base.assign(ano.base().convert_to<base_type>());
				m_index.assign(ano.index().convert_to<base_type>());
				return *this;
			}

			template<typename T, typename U>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_same_v<U, self_type> && !std::is_same_v<T, U>, self_type> &assign(const T &base, const U &index)
			{
				m_base.assign(base);
				m_index.assign(index);
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type>, self_type> &assign(const T &base, const T &index)
			{
				m_base.assign(base);
				m_index.assign(index);
				return *this;
			}
			template<>
			self_type &assign<base_type>(const base_type &base, const base_type &index)
			{
				m_base.assign(base);
				m_index.assign(index);
				return *this;
			}
			template<>
			self_type &assign<std::string>(const std::string &base, const std::string &index)
			{
				if (String::isDecimal(base))
				{
					m_base.assign(base);
				}
				else
				{
					m_base.assign(0);
				}
				if (String::isDecimal(index))
				{
					m_index.assign(index);
				}
				else
				{
					m_index.assign(0);
				}
				return *this;
			}
			self_type &assign(const char *base, const char *index)
			{
				return assign(std::string(base), std::string(index));
			}
			template<>
			self_type &assign<Block>(const Block &base, const Block &index)
			{
				assign(Data::toString(block), Data::toString(index));
				return *this;
			}
			template<bool Signed1, bool Signed2>
			self_type &assign(const IntegerWrapper<Signed1> &base, const IntegerWrapper<Signed2> &anotilogarithm)
			{
				m_base.assign(base.get<base_type>());
				m_index.assign(anotilogarithm.get<base_type>());
				return *this;
			}
			template<uint32 Digits1, uint32 Digits2>
			self_type &assign(const DecimalWrapper<Digits1> &base, const DecimalWrapper<Digits2> &anotilogarithm)
			{
				m_base.assign(base.get<base_type>());
				m_index.assign(anotilogarithm.get<base_type>());
				return *this;
			}

			self_type &swap(self_type &ano)
			{
				m_base.swap(ano.m_base);
				m_index.swap(ano.m_index);
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &swap(PowerWrapper<_Digits> &ano)
			{
				self_type temp(*this);
				m_base.assign(ano.base().convert_to<base_type>());
				m_index.assign(ano.index().convert_to<base_type>());
				ano.assign(temp);
				return *this;
			}

			// operator =
			self_type &operator=(const self_type &rhs) = default;
			self_type &operator=(self_type &&rhs) = default;
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && std::is_convertible_v<T, base_type>, self_type> &operator=(const T &rhs)
			{
				m_index.assign(log(m_base, base_type(rhs)));
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_convertible_v<T, base_type>, self_type> &operator=(const T &rhs)
			{
				m_index.assign(log(m_base, static_cast<base_type>(rhs)));
				return *this;
			}
			template<>
			self_type &operator=<base_type>(const base_type &rhs)
			{
				m_index.assign(log(m_base, rhs));
				return *this;
			}
			template<>
			self_type &operator=<std::string>(const std::string &rhs)
			{
				if (RegexChecker(rhs))
				{
					auto parts(String::matchDecimal(rhs));
					if (parts.size() == 2)
					{
						m_base.assign(parts[0]);
						m_index.assign(parts[1]);
					}
				}
				else if (String::isDecimal(rhs))
				{
					m_index.assign(log(m_base, base_type(rhs)));
				}
				return *this;
			}
			self_type &operator=(const char *rhs)
			{
				return operator=(std::string(rhs));
			}
			template<>
			self_type &operator=<Block>(const Block &rhs)
			{
				operator=(Data::toString(rhs));
				return *this;
			}
			template<bool Signed>
			self_type &operator=(const IntegerWrapper<Signed> &ano)
			{
				m_index.assign(log(m_base, ano.get<base_type>()));
				return *this;
			}
			template<uint32 _Digits>
			self_type &operator=(const DecimalWrapper<_Digits> &ano)
			{
				m_index.assign(log(m_base, ano.get<base_type>()));
				return *this;
			}
			template<uint32 _Digits>
			self_type &operator=(const RationalWrapper<_Digits> &ano)
			{
				m_index.assign(log(m_base, ano.get<base_type>()));
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &operator=(const PowerWrapper<_Digits> &ano)
			{
				m_base.assign(ano.base().convert_to<base_type>());
				m_index.assign(ano.index().convert_to<base_type>());
				return *this;
			}

			// set and get
			const bool valid(void) const { return m_base != 0 && m_index != 0; }
			const bool isInfinity(void) const { return boost::math::isinf(value()); }
			const bool isPositiveInfinity(void) const { return isInfinity() && (value() > 0); }
			const bool isNegativeInfinity(void) const { return isInfinity() && (value() < 0); }
			const bool isNaN(void) const { return !valid() || boost::math::isnan(value()); }

			base_type &getBase(void) { return m_base; }
			const base_type &getBase(void) const { return m_base; }
			template<typename T>
			void setBase(const T &value) { m_base.assign(value); }
			template<>
			void setBase<std::string>(const std::string &value)
			{
				if (String::isDecimal(value))
				{
					m_base.assign(value);
				}
				else
				{
					m_base.assign(0);
				}
			}
			void setBase(const char *value)
			{
				setBase(std::string(value));
			}
			template<>
			void setBase<Block>(const Block &value) 
			{ 
				setBase(Data::toString(value)); 
			}
			template<bool Signed>
			void setBase(const IntegerWrapper<Signed> &value)
			{
				m_base.assign(value.get<base_type>());
			}
			template<uint32 _Digits>
			void setBase(const DecimalWrapper<_Digits> &value)
			{
				m_base.assign(value.get<base_type>());
			}
			template<uint32 _Digits>
			void setBase(const RationalWrapper<_Digits> &value)
			{
				m_base.assign(value.get<base_type>());
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, void> setBase(const LogarithmWrapper<_Digits> &value)
			{
				m_base.assign(value.get<base_type>());
			}

			base_type &getIndex(void) { return m_index; }
			const base_type &getIndex(void) const { return m_index; }
			template<typename T>
			void setIndex(const T &value) { m_index.assign(value); }
			template<>
			void setIndex<std::string>(const std::string &value)
			{
				if (String::isDecimal(value))
				{
					m_index.assign(value);
				}
				else
				{
					m_index.assign(0);
				}
			}
			void setIndex(const char *value)
			{
				setIndex(std::string(value));
			}
			template<>
			void setIndex<Block>(const Block &value) 
			{ 
				setIndex(Data::toString(value)); 
			}
			template<bool Signed>
			void setIndex(const IntegerWrapper<Signed> &value)
			{
				m_index.assign(value.get<base_type>());
			}
			template<uint32 _Digits>
			void setIndex(const DecimalWrapper<_Digits> &value)
			{
				m_index.assign(value.get<base_type>());
			}
			template<uint32 _Digits>
			void setIndex(const RationalWrapper<_Digits> &value)
			{
				m_index.assign(value.get<base_type>());
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, void> setIndex(const LogarithmWrapper<_Digits> &value)
			{
				m_index.assign(value.get<base_type>());
			}

			value_type value(void) const { return !valid() ? std::numeric_limits<value_type>::quiet_NaN() : pow(m_base, m_index); }
			DecimalWrapper<Digits> value_dec_wrapper(void) const { return DecimalWrapper<Digits>(value()); }
			operator value_type(void) const { return value(); }

			// translators
			std::string toString(const std::ios_base::fmtflags flags = std::ios::fixed) const
			{
				std::ostringstream sout;
				sout << "pow(" << m_base.str(Digits, flags) << ',' << m_index.str(Digits, flags) << ')';
				return sout.str();
			}
			Block toBlock(const std::ios_base::fmtflags flags = std::ios::fixed) const { return Data::fromString(toString(flags)); }

			int8 toInt8(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int8>(); }
			uint8 toUInt8(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint8>(); }
			int16 toInt16(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int16>(); }
			uint16 toUInt16(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint16>(); }
			int32 toInt32(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int32>(); }
			uint32 toUInt32(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint32>(); }
			int64 toInt64(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int64>(); }
			uint64 toUInt64(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint64>(); }
			int128 toInt128(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int128>(); }
			uint128 toUInt128(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint128>(); }
			int256 toInt256(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int256>(); }
			uint256 toUInt256(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint256>(); }
			int512 toInt512(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int512>(); }
			uint512 toUInt512(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint512>(); }
			int1024 toInt1024(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<int1024>(); }
			uint1024 toUInt1024(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uint1024>(); }

			template<uint32 bits>
			intx<bits> toIntx(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<intx<bits>>(); }
			template<uint32 bits>
			uintx<bits> toIntx(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<uintx<bits>>(); }
			integer toInteger(const RoundFlag flag = RoundFlag::round) const { return toInteger(flag).convert_to<integer>(); }

			float toFloat(void) const { return !valid() ? std::numeric_limits<float>::quiet_NaN() : value().convert_to<float>(); }
			double toDouble(void) const { return !valid() ? std::numeric_limits<double>::quiet_NaN() : value().convert_to<double>(); }
			float32 toFloat32(void) const { return !valid() ? std::numeric_limits<float32>::quiet_NaN() : value().convert_to<float32>(); }
			float64 toFloat64(void) const { return !valid() ? std::numeric_limits<float64>::quiet_NaN() : value().convert_to<float64>(); }
			float128 toFloat128(void) const { return !valid() ? std::numeric_limits<float128>::quiet_NaN() : value().convert_to<float128>(); }
			float256 toFloat256(void) const { return !valid() ? std::numeric_limits<float256>::quiet_NaN() : value().convert_to<float256>(); }

			dec50 toDec50(void) const { return !valid() ? std::numeric_limits<dec50>::quiet_NaN() : value().convert_to<dec50>(); }
			dec100 toDec100(void) const { return !valid() ? std::numeric_limits<dec100>::quiet_NaN() : value().convert_to<dec100>(); }

			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<_Digits != 0, decimal<_Digits>> toDecimal(void) const { return !valid() ? std::numeric_limits<decimal<_Digits>>::quiet_NaN() : value().convert_to<decimal<_Digits>>(); }
			template<>
			decimal<Digits> toDecimal<Digits>(void) const { return value(); }
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<_Digits != 0, DecimalWrapper<_Digits>> toDecimalWrapper(void) const { return DecimalWrapper<_Digits>(toDecimal<_Digits>()); }
			template<>
			DecimalWrapper<Digits> toDecimalWrapper<Digits>(void) const { return DecimalWrapper<Digits>(value()); }

			template<typename T>
			T get(void) const { return !valid() ? std::numeric_limits<T>::quiet_NaN() : value().convert_to<T>(); }
			template<>
			value_type get<value_type>(void) const { return value(); }

			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, decimal<_Digits>> round(void) const
			{
				static const value_type offset = value_type(5) * pow(value_type(10), -(static_cast<int64>(_Digits) + 1));
				return !valid() ? std::numeric_limits<decimal<_Digits>>::quiet_NaN() : (value() + offset).convert_to<decimal<_Digits>>();
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, decimal<_Digits>> floor(void) const
			{
				return !valid() ? std::numeric_limits<decimal<_Digits>>::quiet_NaN() : value().convert_to<decimal<_Digits>>();
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, decimal<_Digits>> ceil(void) const
			{
				static const value_type offset = pow(value_type(10), -static_cast<int64>(_Digits));
				return !valid() ? std::numeric_limits<decimal<_Digits>>::quiet_NaN() : (value() + offset).convert_to<decimal<_Digits>>();
			}

			integer toInteger(const RoundFlag flag = RoundFlag::round) { return flag == RoundFlag::round ? roundToInteger() : RoundFlag::ceil ? ceilToInteger() : floorToInteger(); }
			integer roundToInteger(void) const { return !valid() ? integer(0) : static_cast<integer>(boost::math::round(value())); }
			integer ceilToInteger(void) const { return !valid() ? integer(0) : floorToInteger() + 1; }
			integer floorToInteger(void) const { return !valid() ? integer(0) : static_cast<integer>(value()); }

		private:
			base_type m_base;
			base_type m_index;
		};

		template<uint32 Digits>
		const String::RegexChecker PowerWrapper<Digits>::RegexChecker(std::string("^pow\\(-?(0|[1-9]\\d*)(.\\d*)?,-?(0|[1-9]\\d*)(.\\d*)?\\)$"));

		template<uint32 Digits>
		integer round(const PowerWrapper<Digits> &value)
		{
			return value.roundToInteger();
		}
		template<uint32 Digits>
		integer ceil(const PowerWrapper<Digits> &value)
		{
			return value.ceilToInteger();
		}
		template<uint32 Digits>
		integer floor(const PowerWrapper<Digits> &value)
		{
			return value.floorToInteger();
		}
	};
};

template<SSUtils::uint32 Digits>
const bool operator==(const SSUtils::Math::PowerWrapper<Digits> &lhs, const SSUtils::Math::PowerWrapper<Digits> &rhs)
{
	return lhs.valid() && rhs.valid() && lhs.m_base == rhs.m_base && lhs.m_index == rhs.m_index;
}

template<SSUtils::uint32 Digits>
const bool operator!=(const SSUtils::Math::PowerWrapper<Digits> &lhs, const SSUtils::Math::PowerWrapper<Digits> &rhs)
{
	return lhs.valid() && rhs.valid() || lhs.m_base != rhs.m_base || lhs.m_index != rhs.m_index;
}

template<SSUtils::uint32 Digits, typename T>
SSUtils::Math::PowerWrapper<Digits> pow(const SSUtils::Math::PowerWrapper<Digits> &lhs, const T &rhs)
{
	return SSUtils::Math::PowerWrapper<Digits>(lhs.getBase(), lhs.getIndex() * static_cast<typename SSUtils::Math::PowerWrapper<Digits>::base_type>(rhs));
}
template<SSUtils::uint32 Digits, typename T>
typename SSUtils::Math::PowerWrapper<Digits>::value_type pow(const T &lhs, const SSUtils::Math::PowerWrapper<Digits> &rhs)
{
	return rhs.value() * lhs;
}
template<SSUtils::uint32 Digits1, SSUtils::uint32 Digits2>
SSUtils::Math::PowerWrapper<Digits1> pow(const SSUtils::Math::PowerWrapper<Digits1> &lhs, const SSUtils::Math::LogarithmWrapper<Digits2> &rhs)
{
	return SSUtils::Math::PowerWrapper<Digits>(lhs.getBase(), lhs.getIndex() * rhs.get<typename SSUtils::Math::PowerWrapper<Digits>::base_type>());
}

template<SSUtils::uint32 Digits>
std::istream &operator>>(std::istream &is, SSUtils::Math::PowerWrapper<Digits> &value)
{
	std::string str;
	is >> str;
	value.assign(str);
	return is;
}
template<SSUtils::uint32 Digits>
std::ostream &operator<<(std::ostream &os, const SSUtils::Math::PowerWrapper<Digits> &value)
{
	os << value.toString();
	return os;
}

namespace std
{
	template<SSUtils::uint32 Digits>
	class numeric_limits<SSUtils::Math::PowerWrapper<Digits>>
		: public numeric_limits<typename SSUtils::Math::PowerWrapper<Digits>::value_type>
	{};

	template<SSUtils::uint32 Digits>
	std::string to_string(const SSUtils::Math::PowerWrapper<Digits> &value)
	{
		return value.toString();
	}
	template<SSUtils::uint32 Digits>
	SSUtils::Math::PowerWrapper<Digits> stopower_wrapper(const std::string &str)
	{
		return SSUtils::Math::PowerWrapper<Digits>(str);
	}
};

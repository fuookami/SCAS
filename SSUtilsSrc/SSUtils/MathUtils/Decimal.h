#pragma once

#include "_pri_real_global.h"
#include "..\DataUtils.h"
#include "..\StringUtils.h"
#include <limits>
#include <boost/math/special_functions.hpp>

namespace SSUtils
{
	namespace Math
	{
		template<uint32 Digits> 
		class DecimalWrapper : public decimal<Digits>
		{
		public:
			typedef decimal<Digits> value_type;
			typedef DecimalWrapper self_type;

			// constructors
			DecimalWrapper(void)
				: value_type(0), m_base(0), m_index(0), m_offset(1) {};
			DecimalWrapper(const self_type &ano) = default;
			DecimalWrapper(self_type &&ano) = default;

			DecimalWrapper(const value_type &ano, const int32 index = 0)
				: value_type(0), m_base(ano), m_index(0), m_offset(1)
			{
				refresh(index);
			}
			DecimalWrapper(const std::string &str, const int32 index = 0)
				: value_type(0), m_base(0), m_index(0), m_offset(1)
			{
				if (String::isDecimal(str))
				{
					m_base.assign(str);
				}
				refresh(index);
			}
			DecimalWrapper(const char *str, const int32 index = 0)
				: DecimalWrapper(std::string(str), index)
			{
			}
			DecimalWrapper(const Block &block, const int32 index = 0)
				: DecimalWrapper(Data::toString(block), index)
			{
			}

			template<typename T>
			DecimalWrapper(const T &ano, const int32 index = 0)
				: DecimalWrapper(generate(ano, index))
			{
			}
			template<bool Signed>
			DecimalWrapper(const IntegerWrapper<Signed> &ano, const int32 index = 0)
				: DecimalWrapper(ano.get<value_type>(), index)
			{
			}
			template<uint32 _Digits>
			DecimalWrapper(const DecimalWrapper<_Digits> &ano)
				: DecimalWrapper(ano.get<value_type>(), ano.m_index)
			{
			}

			// destructor
			~DecimalWrapper(void) = default;

			// generators
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && std::is_convertible_v<T, value_type>, self_type> generate(const T &value, const int32 index = 1)
			{
				self_type ret = self_type(value_type(value), index);
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_convertible_v<T, value_type>, self_type> generate(const T &value, const int32 index = 1)
			{
				self_type ret = self_type(static_cast<value_type>(value), index);
				return ret;
			}
			template<>
			static self_type generate<value_type>(const value_type &value, const int32 index)
			{
				self_type ret = self_type(value, index);
				return ret;
			}
			template<>
			static self_type generate<std::string>(const std::string &value, const int32 index)
			{
				self_type ret = self_type(value, index);
				return ret;
			}
			static self_type generate(const char *value, const int32 index)
			{
				return generate(std::string(value), index);
			}
			template<>
			static self_type generate<Block>(const Block &value, const int32 index)
			{
				self_type ret = self_type(value, index);
				return ret;
			}
			template<bool Signed>
			static self_type generate(const IntegerWrapper<Signed> &value, const int32 index)
			{
				self_type ret = self_type(value, index);
				return ret;
			}

			// assign and swap
			self_type &assign(const self_type &ano)
			{
				value_type::assign(ano.value());
				m_base.assign(ano.m_base);
				m_index = ano.m_index;
				m_offset.assign(ano.m_offset);
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type>, self_type> &assign(const T &ano, const int32 index = 0)
			{
				m_base.assign(ano);
				refresh(index);
				return *this;
			}
			template<>
			self_type &assign<std::string>(const std::string &ano, const int32 index)
			{
				if (String::isDecimal(ano))
				{
					m_base.assign(ano);
				}
				else
				{
					m_base.assign(0);
				}
				refresh(index);
				return *this;
			}
			self_type &assign(const char *ano, const int32 index)
			{
				return assign(std::string(ano), index);
			}
			template<>
			self_type &assign<Block>(const Block &ano, const int32 index)
			{
				assign(Data::toString(ano), index);
				return *this;
			}
			template<bool Signed>
			self_type &assign(const IntegerWrapper<Signed> &ano, const int32 index = 0)
			{
				assign(ano.get<value_type>());
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const DecimalWrapper<_Digits> &ano)
			{
				m_base.assign(ano.get<value_type>());
				refresh(ano.m_base);
				return *this;
			}

			self_type &swap(value_type &ano)
			{
				value_type::swap(ano);
				refresh();
				return *this;
			}
			self_type &swap(self_type &ano)
			{
				m_base.swap(ano.m_base);
				std::swap(m_index, ano.m_index);
				m_offset.swap(ano.m_offset);
				refresh(m_index);
				ano.refresh(ano.m_index);
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &swap(DecimalWrapper<_Digits> &ano)
			{
				m_base.assign(ano.get<value_type>());
				ano.m_base.assign(get<DecimalWrapper<_Digits>::value_type>());
				int32 index(m_index);
				refresh(ano.index());
				ano.refresh(index);
				return *this;
			}

			// operator =
			self_type &operator=(const self_type &rhs) = default;
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && std::is_convertible_v<T, value_type>, self_type> &operator=(const T &rhs)
			{
				value_type::operator=(rhs);
				refresh();
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_convertible_v<T, value_type>, self_type> &operator=(const T &rhs)
			{
				value_type::assign(rhs);
				refresh();
				return *this;
			}
			template<>
			self_type &operator=<std::string>(const std::string &rhs)
			{
				if (String::isDecimal(rhs))
				{
					value_type::assign(rhs);
				}
				else
				{
					value_type::assign(0);
				}
				refresh();
				return *this;
			}
			self_type &operator=(const char *rhs)
			{
				operator=(std::string(rhs));
				return *this;
			}
			template<>
			self_type &operator=<Block>(const Block &rhs)
			{
				operator=(Data::toString(rhs));
				return *this;
			}
			template<bool Signed>
			self_type &operator=(const IntegerWrapper<Signed> &rhs)
			{
				operator=(rhs.get<value_type>());
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<Digits != _Digits, self_type> &operator=(const DecimalWrapper<_Digits> &rhs)
			{
				operator=(rhs.get<value_type>());
				return *this;
			}

			// other operators
			template<typename T>
			typename std::enable_if_t<std::is_convertible_v<T, value_type>, self_type> &operator+=(const T &rhs)
			{
				value_type::operator+=(rhs);
				refresh();
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_convertible_v<T, value_type>, self_type> &operator+=(const T &rhs)
			{
				value_type::operator+=(static_cast<value_type>(rhs));
				refresh();
				return *this;
			}
			template<>
			self_type &operator+=<std::string>(const std::string &rhs)
			{
				if (String::isDecimal(rhs))
				{
					operator+=(value_type(rhs));
					refresh();
				}
				return *this;
			}
			self_type &operator+=(const char *rhs)
			{
				operator+=(std::string(rhs));
				return *this;
			}
			template<>
			self_type &operator+=<Block>(const Block &rhs)
			{
				operator+=(Data::toString(rhs));
				return *this;
			}
			template<bool Signed>
			self_type &operator+=(const IntegerWrapper<Signed> &rhs)
			{
				operator+=(rhs.get<value_type>());
				return *this;
			}
			template<uint32 _Digits>
			self_type &operator+=(const DecimalWrapper<_Digits> &rhs)
			{
				operator+=(rhs.get<value_type>());
				return *this;
			}

			template<typename T>
			typename std::enable_if_t<std::is_convertible_v<T, value_type>, self_type> &operator-=(const T &rhs)
			{
				value_type::operator-=(rhs);
				refresh();
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_convertible_v<T, value_type>, self_type> &operator-=(const T &rhs)
			{
				value_type::operator-=(static_cast<value_type>(rhs));
				refresh();
				return *this;
			}
			template<>
			self_type &operator-=<std::string>(const std::string &rhs)
			{
				if (String::isDecimal(rhs))
				{
					operator-=(value_type(rhs));
					refresh();
				}
				return *this;
			}
			self_type &operator-=(const char *rhs)
			{
				operator-=(std::string(rhs));
				return *this;
			}
			template<>
			self_type &operator-=<Block>(const Block &rhs)
			{
				operator-=(Data::toString(rhs));
				return *this;
			}
			template<bool Signed>
			self_type &operator-=(const IntegerWrapper<Signed> &rhs)
			{
				operator-=(rhs.get<value_type>());
				return *this;
			}
			template<uint32 _Digits>
			self_type &operator-=(const DecimalWrapper<_Digits> &rhs)
			{
				operator-=(rhs.get<value_type>());
				return *this;
			}

			template<typename T>
			typename std::enable_if_t<std::is_convertible_v<T, value_type>, self_type> &operator*=(const T &rhs)
			{
				value_type::operator*=(rhs);
				refresh();
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_convertible_v<T, value_type>, self_type> &operator*=(const T &rhs)
			{
				value_type::operator*=(static_cast<value_type>(rhs));
				refresh();
				return *this;
			}
			template<>
			self_type &operator*=<std::string>(const std::string &rhs)
			{
				if (String::isDecimal(rhs))
				{
					operator*=(value_type(rhs));
					refresh();
				}
				return *this;
			}
			self_type &operator*=(const char *rhs)
			{
				operator*=(std::string(rhs));
				return *this;
			}
			template<>
			self_type &operator*=<Block>(const Block &rhs)
			{
				operator*=(Data::toString(rhs));
				return *this;
			}
			template<bool Signed>
			self_type &operator*=(const IntegerWrapper<Signed> &rhs)
			{
				operator*=(rhs.get<value_type>());
				return *this;
			}
			template<uint32 _Digits>
			self_type &operator*=(const DecimalWrapper<_Digits> &rhs)
			{
				operator*=(rhs.get<value_type>());
				return *this;
			}

			template<typename T>
			typename std::enable_if_t<std::is_convertible_v<T, value_type>, self_type> &operator/=(const T &rhs)
			{
				value_type::operator/=(rhs);
				refresh();
				return *this;
			}
			template<typename T>
			typename std::enable_if_t<!std::is_convertible_v<T, value_type>, self_type> &operator/=(const T &rhs)
			{
				value_type::operator/=(static_cast<value_type>(rhs));
				refresh();
				return *this;
			}
			template<>
			self_type &operator/=<std::string>(const std::string &rhs)
			{
				if (String::isDecimal(rhs))
				{
					operator/=(value_type(rhs));
					refresh();
				}
				return *this;
			}
			self_type &operator/=(const char *rhs)
			{
				operator/=(std::string(rhs));
				return *this;
			}
			template<>
			self_type &operator/=<Block>(const Block &rhs)
			{
				operator/=(Data::toString(rhs));
				return *this;
			}
			template<bool Signed>
			self_type &operator/=(const IntegerWrapper<Signed> &rhs)
			{
				operator/=(rhs.get<value_type>());
				return *this;
			}
			template<uint32 _Digits>
			self_type &operator/=(const DecimalWrapper<_Digits> &rhs)
			{
				operator/=(rhs.get<value_type>());
				return *this;
			}

			self_type &operator++(void)
			{
				value_type::operator++();
				refresh();
				return *this;
			}
			self_type &operator--(void)
			{
				value_type::operator--();
				refresh();
				return *this;
			}
			self_type operator++(int)
			{
				self_type ret = self_type(*this);
				++ret;
				return ret;
			}
			self_type operator--(int)
			{
				self_type ret = self_type(*this);
				--ret;
				return ret;
			}

			// set and get
			const bool isInfinity(void) const { return boost::math::isinf(dynamic_cast<const value_type &>(*this)); }
			const bool isPositiveInfinity(void) const { return isInfinity() && (*this > 0); }
			const bool isNegativeInfinity(void) const { return isInfinity() && (*this < 0); }
			const bool isNaN(void) const { return boost::math::isnan(dynamic_cast<const value_type &>(*this)); }

			const int32 getIndex(void) const { return m_index; }
			const value_type &getOffset(void) const { return m_offset; }
			void setIndex(const int32 index) { refresh(); refresh(index); }

			const value_type &getBase(void) const { return m_base; }
			template<typename T>
			typename std::enable_if_t<std::is_convertible_v<T, value_type>, void> setBase(const T &base)
			{
				refresh(value_type(base));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_convertible_v<T, value_type>, void> setBase(const T &base)
			{
				refresh(static_cast<value_type>(base));
			}
			template<>
			void setBase<value_type>(const value_type &base) 
			{ 
				refresh(base); 
			}
			template<>
			void setBase<std::string>(const std::string &value)
			{
				if (String::isDecimal(value))
				{
					refresh(value_type(value));
				}
				else
				{
					refresh(value_type(0));
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
				refresh(value.get<base_type>());
			}
			template<uint32 _Digits>
			void setBase(const DecimalWrapper<_Digits> &value)
			{
				refresh(value.get<base_type>());
			}

			const value_type &value(void) const { return *this; }

			// translators
			std::string toString(const std::ios_base::fmtflags flags = std::ios::fixed) const { return this->str(Digits, flags); }
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

			float toFloat(void) const { return convert_to<float>(); }
			double toDouble(void) const { return convert_to<double>(); }
			float32 toFloat32(void) const { return convert_to<float32>(); }
			float64 toFloat64(void) const { return convert_to<float64>(); }
			float128 toFloat128(void) const { return convert_to<float128>(); }
			float256 toFloat256(void) const { return convert_to<float256>(); }

			dec50 toDec50(void) const { return convert_to<dec50>(); }
			dec100 toDec100(void) const { return convert_to<dec100>(); }
			real toReal(void) const { return convert_to<real>(); }

			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<_Digits != 0, decimal<Digits>> toDecimal(void) const { return convert_to<decimal<Digits>>(); }
			template<>
			decimal<Digits> toDecimal<Digits>(void) const { return *this; }
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<_Digits != Digits && _Digits != 0, DecimalWrapper<_Digits>> toDecimalWrapper(void) const { return DecimalWrapper<_Digits>(toDecimal<_Digits>()); }

			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, value_type>, T> get(void) const { return convert_to<T>(); }
			template<typename T>
			typename std::enable_if_t<std::is_same_v<T, value_type>, const T &> get(void) const { return *this; }

			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, decimal<_Digits>> round(void) const
			{
				static const value_type offset = value_type(5) * pow(value_type(10), -(static_cast<int64>(_Digits) + 1));
				return (value() + offset).convert_to<decimal<_Digits>>();
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, decimal<_Digits>> floor(void) const
			{
				return value().convert_to<decimal<_Digits>>();
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, decimal<_Digits>> ceil(void) const
			{
				static const value_type offset = pow(value_type(10), -static_cast<int64>(_Digits));
				return (value() + offset).convert_to<decimal<_Digits>>();
			}

			integer toInteger(const RoundFlag flag = RoundFlag::round) { return flag == RoundFlag::round ? roundToInteger() : RoundFlag::ceil ? ceilToInteger() : floorToInteger(); }
			integer roundToInteger(void) const { return static_cast<integer>(boost::math::round(value())); }
			integer ceilToInteger(void) const { return floorToInteger() + 1; }
			integer floorToInteger(void) const { return static_cast<integer>(value()); }

			void refresh(void)
			{
				m_base = value() / m_offset;
			}
			void refresh(const value_type &base)
			{
				value_type::assign(base * offset);
				m_base.assign(base);
			}
			void refresh(const int32 index)
			{
				m_offset = pow(value_type(10), index);
				value_type::assign(m_base * m_offset);
				m_index = index;
			}

		private:
			value_type m_base;
			int32 m_index;
			value_type m_offset;
		};

		template<uint32 Digits>
		integer round(const decimal<Digits> &value)
		{
			return static_cast<integer>(boost::math::round(value));
		}
		template<uint32 Digits>
		integer ceil(const decimal<Digits> &value)
		{
			return floor(value) + 1;
		}
		template<uint32 Digits>
		integer floor(const decimal<Digits> &value)
		{
			return static_cast<integer>(value);
		}
	};
};

template<SSUtils::uint32 Digits>
std::istream &operator>>(std::istream &is, SSUtils::Math::DecimalWrapper<Digits> &value)
{
	typedef typename SSUtils::Math::DecimalWrapper<Digits>::value_type value_type;
	value_type temp;
	is >> temp;
	value.assign(temp);
	return is;
}

namespace std
{
	template<SSUtils::uint32 Digits>
	class numeric_limits<SSUtils::Math::DecimalWrapper<Digits>>
		: public numeric_limits<typename SSUtils::Math::DecimalWrapper<Digits>::value_type>
	{};

	template<SSUtils::uint32 Digits>
	SSUtils::Math::DecimalWrapper<Digits> stodecimal_wrapper(const std::string &str)
	{
		return SSUtils::Math::DecimalWrapper<Digits>(str);
	}
};

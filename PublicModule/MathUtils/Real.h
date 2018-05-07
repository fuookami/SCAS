#pragma once

#include "_pri_math_global.h"
#include "Power.h"
#include "Logarithm.h"

#include <boost/bimap.hpp>
#include <boost/variant.hpp>
#include <boost/numeric/conversion/cast.hpp>
#include <boost/rational.hpp>

#include <ostream>
#include <sstream>
#include <typeinfo>
#include <map>
#include <set>
#include <functional>
#include <utility>
#include <exception>
#include <cstdint>
#include <complex>

namespace SSUtils
{
	namespace Math
	{
		template<uint32 Digits>
		class RealWrapper
		{
		public:
			enum class Type
			{
				Boolean = 0,
				Integer,
				UInteger,
				Decimal,
				Rational,
				Power,
				Logarithm,
				Transcendental,
				Sepcial
			};

			enum class TranscendentalValue
			{
				pi,
				e,
				none
			};

			enum class SpecialValue
			{
				NegativeInfinity,
				PositiveInfinity,	
				NaN,
				Empty,
				none
			};

			typedef typename RealTypeGroup<Digits>::integer_type integer_type;
			typedef typename RealTypeGroup<Digits>::uinteger_type uinteger_type;
			typedef typename RealTypeGroup<Digits>::decimal_type decimal_type;
			typedef typename RealTypeGroup<Digits>::rational_type rational_type;
			typedef typename RealTypeGroup<Digits>::power_type power_type;
			typedef typename RealTypeGroup<Digits>::logarithm_type logarithm_type;
			typedef boost::variant<bool, integer_type, uinteger_type, decimal_type, rational_type, power_type, logarithm_type, TranscendentalValue, SpecialValue> value_type;
			typedef RealWrapper self_type;

			static const self_type pi;
			static const self_type e;
			static const self_type root_2;
			static const self_type root_3;
			static const self_type ln_2;
			static const self_type lg_2;

		private:
			template<typename visitor_t, uint32 _Digits>
			auto visit(const typename RealWrapper<_Digits>::value_type &value)
				-> typename std::enable_if_t<Digits != _Digits && _Digits != 0, typename visitor_t::result_type>
			{
				return boost::apply_visitor(visitor_t(), value);
			}
			template<typename visitor_t, uint32 _Digits>
			auto visit(const visitor_t &visitor, const typename RealWrapper<_Digits>::value_type &value)
				-> typename std::enable_if_t<Digits != _Digits && _Digits != 0, typename visitor_t::result_type>
			{
				return boost::apply_visitor(visitor, value);
			}
			template<typename visitor_t, uint32 _Digits>
			auto visit(const typename RealWrapper<_Digits> &value)
				-> typename std::enable_if_t<Digits != _Digits && _Digits != 0, typename visitor_t::result_type>
			{
				return boost::apply_visitor(visitor_t(), value.value());
			}
			template<typename visitor_t, uint32 _Digits>
			auto visit(const visitor_t &visitor, const typename RealWrapper<_Digits> &value)
				-> typename visitor_t::result_type
			{
				return boost::apply_visitor(visitor, value.value());
			}
			template<typename visitor_t>
			auto visit(void) 
				-> typename visitor_t::result_type
			{
				return boost::apply_visitor(visitor_t(), m_value);
			}
			template<typename visitor_t>
			auto visit(const visitor_t &visitor) 
				-> typename visitor_t::result_type
			{
				return boost::apply_visitor(visitor, m_value);
			}

			template<typename visitor_t, uint32 _Digits>
			auto visit(const typename RealWrapper<_Digits>::value_type &value) const
				-> typename std::enable_if_t<Digits != _Digits && _Digits != 0, typename visitor_t::result_type>
			{
				return boost::apply_visitor(visitor_t(), value);
			}
			template<typename visitor_t, uint32 _Digits>
			auto visit(const visitor_t &visitor, const typename RealWrapper<_Digits>::value_type &value) const
				-> typename std::enable_if_t<Digits != _Digits && _Digits != 0, typename visitor_t::result_type>
			{
				return boost::apply_visitor(visitor, value);
			}
			template<typename visitor_t, uint32 _Digits>
			auto visit(const typename RealWrapper<_Digits> &value) const
				-> typename std::enable_if_t<Digits != _Digits && _Digits != 0, typename visitor_t::result_type>
			{
				return boost::apply_visitor(visitor_t(), value.value());
			}
			template<typename visitor_t, uint32 _Digits>
			auto visit(const visitor_t &visitor, const typename RealWrapper<_Digits> &value) const
				-> typename visitor_t::result_type
			{
				return boost::apply_visitor(visitor, value.value());
			}
			template<typename visitor_t>
			auto visit(void) const 
				-> typename visitor_t::result_type
			{
				return boost::apply_visitor(visitor_t(), m_value);
			}
			template<typename visitor_t>
			auto visit(const visitor_t &visitor) const 
				-> typename visitor_t::result_type
			{
				return boost::apply_visitor(visitor, m_value);
			}

			template<uint32 _Digits, typename U = std::enable_if_t<Digits != _Digits && _Digits != 0>>
			struct differnet_digits_value_type_translator : public boost::static_visitor<value_type>
			{
				template<typename T>
				value_type operator()(const T &value) const
				{
					return value_type(value);
				}
				
				value_type operator()(const typename RealWrapper<_Digits>::decimal_type &decimal_value) const
				{
					return value_type(decimal_type(decimal_value));
				}

				value_type operator()(const typename RealWrapper<_Digits>::rational_type &rational_value) const
				{
					return value_type(rational_type(rational_value));
				}

				value_type operator()(const typename RealWrapper<_Digits>::power_type &power_value) const
				{
					return value_type(power_type(power_value));
				}

				value_type operator()(const typename RealWrapper<_Digits>::logarithm_type &logarithm_value) const
				{
					return value_type(logarithm_type(logarithm_value));
				}
			};

			struct positive_infinity_visitor : public boost::static_visitor<bool>
			{
				template<typename T>
				bool operator()(const T &value) const
				{
					return false;
				}

				bool operator()(const decimal_type &decimal_value) const
				{
					return check(decimal_value);
				}

				bool operator()(const rational_type &rational_value) const
				{
					return check(rational_value.value());
				}

				bool operator()(const power_type &power_value) const
				{
					return check(power_value.value());
				}

				bool operator()(const logarithm_type &logarithm_value) const
				{
					return check(logarithm_value.value());
				}

				bool operator()(const SpecialValue special_value) const
				{
					return special_value == SpecialValue::PositiveInfinity;
				}

				bool check(const typename decimal_type::value_type &decimal_value) const
				{
					return boost::math::isinf(decimal_value) && decimal_value > 0;
				}
			};

			struct negative_infinity_visitor : public boost::static_visitor<bool>
			{
				template<typename T>
				bool operator()(const T &value) const
				{
					return false;
				}

				bool operator()(const decimal_type & decimal_value) const
				{
					return check(decimal_value);
				}

				bool operator()(const rational_type &rational_value) const
				{
					return check(rational_value.value());
				}

				bool operator()(const power_type &power_value) const
				{
					return check(power_value.value());
				}

				bool operator()(const logarithm_type &logarithm_value) const
				{
					return check(logarithm_value.value());
				}

				bool operator()(const SpecialValue special_value) const
				{
					return special_value == SpecialValue::NegativeInfinity;
				}

				bool check(const typename decimal_type::value_type &decimal_value) const
				{
					return boost::math::isinf(decimal_value) && decimal_value < 0;
				}
			};

			struct nan_visitor : public boost::static_visitor<bool>
			{
				template<typename T>
				bool operator()(const T &value) const
				{
					return false;
				}

				bool operator()(const rational_type &rational_value) const
				{
					return rational_value.valid();
				}

				bool operator()(const power_type &power_value) const
				{
					return power_value.valid() || boost::math::isnan(power_value.value());
				}

				bool operator()(const logarithm_type &logarithm_value) const
				{
					return logarithm_value.valid() || boost::math::isnan(logarithm_value.value());
				}

				bool operator()(const SpecialValue special_value) const
				{
					return special_value == SpecialValue::NaN;
				}
			};

			struct empty_visitor : public boost::static_visitor<bool>
			{
				template<typename T>
				bool operator()(const T &value) const
				{
					return false;
				}

				bool operator()(const SpecialValue special_value)
				{
					return special_value == SpecialValue::Empty;
				}
			};

			struct to_string_visitor : public boost::static_visitor<std::string>
			{
				template<typename T>
				std::string operator()(const T &value) const
				{
					return value.toString();
				}

				std::string operator()(const bool value) const
				{
					auto it = String2Boolean().right.find(value);
					return it == String2Boolean().right.end() ? String::EmptyString : it->second;
				}

				std::string operator()(const TranscendentalValue value) const
				{
					auto it = String2TranscendentalValue().right.find(value);
					return it == String2TranscendentalValue().right.end() ? String::EmptyString : it->second;
				}

				std::string operator()(const SpecialValue value) const
				{
					auto it = String2SpecialValue().right.find(value);
					return it == String2SpecialValue().right.end() ? String::EmptyString : it->second;
				}
			};

			struct to_block_visitor : public boost::static_visitor<Block>
			{
				template<typename T>
				Block operator()(const T &value) const
				{
					return value.toBlock();
				}

				Block operator()(const bool value) const
				{
					return Data::fromString(to_string_visitor()(value));
				}

				Block operator()(const TranscendentalValue value) const
				{
					return Data::fromString(to_string_visitor()(value));
				}

				Block operator()(const SpecialValue value) const
				{
					return Data::fromString(to_string_visitor()(value));
				}
			};

			struct to_boolean_visitor : public boost::static_visitor<bool>
			{
				template<typename T>
				bool operator()(const T &value) const
				{
					return value != 0;
				}

				bool operator()(const bool value) const
				{
					return value;
				}

				bool operator()(const TranscendentalValue value) const
				{
					return true;
				}

				bool operator()(const SpecialValue value) const
				{
					return value == SpecialValue::PositiveInfinity || value == SpecialValue::NegativeInfinity;
				}
			};

			struct to_integer_visitor : public boost::static_visitor<std::pair<bool, integer_type>>
			{
				RoundFlag flag;

				to_integer_visitor(const RoundFlag _flag = RoundFlag::round)
					: flag(_flag) {};

				template<typename T>
				std::pair<bool, integer_type> operator()(const T &value) const
				{
					return std::make_pair(true, integer_type(value.toInteger(flag)));
				}

				std::pair<bool, integer_type> operator()(const uinteger_type &value) const
				{
					return std::make_pair(true, integer_type(value.value()));
				}

				std::pair<bool, integer_type> operator()(const bool value) const
				{
					return std::make_pair(true, value ? integer_type(1) : integer_type(0));
				}

				std::pair<bool, integer_type> operator()(const TranscendentalValue value) const
				{
					auto it = TranscendentalValue2Decimal().left.find(value);
					if (it == TranscendentalValue2Decimal().left.end())
					{
						return std::make_pair(false, integer_type(0));
					}
					else
					{
						return std::make_pair(true, integer_type(decimal_type(it->second).toInteger(flag)));	
					}
				}

				std::pair<bool, integer_type> operator()(const SpecialValue value) const
				{
					return std::make_pair(false, integer_type(0));
				}
			};

			struct to_decimal_visitor : public boost::static_visitor<std::pair<bool, decimal_type>>
			{
				template<typename T>
				std::pair<bool, decimal_type> operator()(const T &value) const
				{
					return std::make_pair(true, decimal_type(value.toDecimal<Digits>()));
				}

				std::pair<bool, decimal_type> operator()(const decimal_type &value) const
				{
					return std::make_pair(true, value);
				}

				std::pair<bool, decimal_type> operator()(const bool value) const
				{
					return std::make_pair(true, value ? decimal_type(1) : decimal_type(0));
				}

				std::pair<bool, decimal_type> operator()(const TranscendentalValue value) const
				{
					auto it = TranscendentalValue2Decimal().find(value);
					if (it == TranscendentalValue2Decimal().cend())
					{
						return std::make_pair(false, decimal_type(0));
					}
					else
					{
						return std::make_pair(true, decimal_type(it->second));	
					}
				}

				std::pair<bool, decimal_type> operator()(const SpecialValue value) const
				{
					switch(value)
					{
						case SpecialValue::PositiveInfinity:
							return std::make_pair(true, decimal_type(std::numeric_limits<decimal_type>::infinity()));
						case SpecialValue::NegativeInfinity:
							return std::make_pair(true, decimal_type(-std::numeric_limits<decimal_type>::infinity()));
						case SpecialValue::NaN:
							return std::make_pair(true, decimal_type(std::numeric_limits<decimal_type>::quiet_NaN()));
						case SpecialValue::Empty:
							return std::make_pair(false, decimal_type(0));
						default:
							return std::make_pair(false, decimal_type(0));
					}
					return std::make_pair(false, decimal_type(0));
				}
			};

			template<uint32 _Digits, typename U = std::enable_if_t<Digits >= _Digits && _Digits != 0>>
			struct round_visitor : public boost::static_visitor<RealWrapper<_Digits>>
			{
				typedef typename RealWrapper<Digits>::decimal_type _decimal_type;
				typedef typename RealWrapper<Digits>::rational_type _rational_type;
				typedef typename RealWrapper<Digits>::power_type _power_type;
				typedef typename RealWrapper<Digits>::logarithm_type _logarithm_type;

				template<typename T>
				RealWrapper<_Digits> operator()(const T &value) const
				{
					return RealWrapper<_Digits>(value);
				}

				RealWrapper<_Digits> operator()(const decimal_type &value) const
				{
					return RealWrapper<_Digits>(value.round<_Digits>());
				}

				RealWrapper<_Digits> operator()(const rational_type &value) const
				{
					return RealWrapper<_Digits>(_rational_type(value));
				}

				RealWrapper<_Digits> operator()(const power_type &value) const
				{
					decimal_type base(value.getBase()), index(value.getIndex());
					return RealWrapper<_Digits>(_power_type(base.round<_Digits>(), index.round<_Digits>()));
				}

				RealWrapper<_Digits> operator()(const logarithm_type &value) const
				{
					decimal_type base(value.getBase()), antilogarithm(value.getAntilogarithm());
					return RealWrapper<_Digits>(_power_type(base.round<_Digits>(), antilogarithm.round<_Digits>()));
				}
			};

			template<uint32 _Digits, typename U = std::enable_if_t<Digits >= _Digits && _Digits != 0>>
			struct ceil_visitor : public boost::static_visitor<RealWrapper<_Digits>>
			{
				typedef typename RealWrapper<Digits>::decimal_type _decimal_type;
				typedef typename RealWrapper<Digits>::rational_type _rational_type;
				typedef typename RealWrapper<Digits>::power_type _power_type;
				typedef typename RealWrapper<Digits>::logarithm_type _logarithm_type;

				template<typename T>
				RealWrapper<_Digits> operator()(const T &value) const
				{
					return RealWrapper<_Digits>(value);
				}

				RealWrapper<_Digits> operator()(const decimal_type &value) const
				{
					return RealWrapper<_Digits>(value.ceil<_Digits>());
				}

				RealWrapper<_Digits> operator()(const rational_type &value) const
				{
					return RealWrapper<_Digits>(_rational_type(value));
				}

				RealWrapper<_Digits> operator()(const power_type &value) const
				{
					decimal_type base(value.getBase()), index(value.getIndex());
					return RealWrapper<_Digits>(_power_type(base.ceil<_Digits>(), index.ceil<_Digits>()));
				}

				RealWrapper<_Digits> operator()(const logarithm_type &value) const
				{
					decimal_type base(value.getBase()), antilogarithm(value.getAntilogarithm());
					return RealWrapper<_Digits>(_power_type(base.ceil<_Digits>(), antilogarithm.ceil<_Digits>()));
				}
			};

			template<uint32 _Digits, typename U = std::enable_if_t<Digits >= _Digits && _Digits != 0>>
			struct floor_visitor : public boost::static_visitor<RealWrapper<_Digits>>
			{
				typedef typename RealWrapper<Digits>::decimal_type _decimal_type;
				typedef typename RealWrapper<Digits>::rational_type _rational_type;
				typedef typename RealWrapper<Digits>::power_type _power_type;
				typedef typename RealWrapper<Digits>::logarithm_type _logarithm_type;

				template<typename T>
				RealWrapper<_Digits> operator()(const T &value) const
				{
					return RealWrapper<_Digits>(value);
				}

				RealWrapper<_Digits> operator()(const decimal_type &value) const
				{
					return RealWrapper<_Digits>(value.floor<_Digits>());
				}

				RealWrapper<_Digits> operator()(const rational_type &value) const
				{
					return RealWrapper<_Digits>(_rational_type(value));
				}

				RealWrapper<_Digits> operator()(const power_type &value) const
				{
					decimal_type base(value.getBase()), index(value.getIndex());
					return RealWrapper<_Digits>(_power_type(base.floor<_Digits>(), index.floor<_Digits>()));
				}

				RealWrapper<_Digits> operator()(const logarithm_type &value) const
				{
					decimal_type base(value.getBase()), antilogarithm(value.getAntilogarithm());
					return RealWrapper<_Digits>(_power_type(base.floor<_Digits>(), antilogarithm.floor<_Digits>()));
				}
			};

			static const boost::bimap<std::string, Type> &TypeName2Type(void)
			{
				typedef boost::bimap<std::string, Type> result_type;
				typedef typename result_type::value_type pair_type;

				static const result_type ret =
					[]() -> result_type
				{
					result_type ret;
					ret.insert(pair_type(typeid(bool).name(), RealWrapper<Digits>::Type::Boolean));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::integer_type).name(), RealWrapper<Digits>::Type::Integer));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::uinteger_type).name(), RealWrapper<Digits>::Type::UInteger));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::decimal_type).name(), RealWrapper<Digits>::Type::Decimal));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::rational_type).name(), RealWrapper<Digits>::Type::Rational));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::power_type).name(), RealWrapper<Digits>::Type::Power));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::logarithm_type).name(), RealWrapper<Digits>::Type::Logarithm));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::TranscendentalValue).name(), RealWrapper<Digits>::Type::Transcendental));
					ret.insert(pair_type(typeid(typename RealWrapper<Digits>::SpecialValue).name(), RealWrapper<Digits>::Type::Sepcial));
				}();

				return ret;
			}

			static const boost::bimap<std::string, bool> &String2Boolean(void)
			{
				typedef boost::bimap<std::string, bool> result_type;
				typedef typename result_type::value_type pair_type;

				static const result_type ret =
					[]() -> result_type
				{
					result_type ret;
					ret.insert(pair_type(String::True, true));
					ret.insert(pair_type(String::False, false));

					return ret;
				}();

				return ret;
			}

			static const boost::bimap<std::string, TranscendentalValue> &String2TranscendentalValue(void)
			{
				typedef boost::bimap<std::string, TranscendentalValue> result_type;
				typedef typename result_type::value_type pair_type;

				static const result_type ret = 
					[]() -> result_type
				{
					result_type ret;
					ret.insert(pair_type(std::string("pi"), TranscendentalValue::pi));
					ret.insert(pair_type(std::string("e"), TranscendentalValue::e));

					return ret;
				}();

				return ret;
			}

			static const boost::bimap<std::string, SpecialValue> &String2SpecialValue(void)
			{
				typedef boost::bimap<std::string, SpecialValue> result_type;
				typedef typename result_type::value_type pair_type;

				static const result_type ret = 
					[]() -> result_type
				{
					result_type ret;
					ret.insert(pair_type(String::Empty, SpecialValue::Empty));
					ret.insert(pair_type(String::NotANumber, SpecialValue::NaN));
					ret.insert(pair_type(String::PositiveInfinity, SpecialValue::PositiveInfinity));
					ret.insert(pair_type(String::NegativeInfinity, SpecialValue::NegativeInfinity));

					return ret;
				}();

				return ret;
			}	

			static const std::map<TranscendentalValue, typename decimal_type::value_type> &TranscendentalValue2Decimal(void)
			{
				typedef std::map<TranscendentalValue, typename decimal_type::value_type> result_type;

				static const result_type ret = {
					std::make_pair(TranscendentalValue::pi, Constant::pi<Digits>()),
					std::make_pair(TranscendentalValue::e, Constant::e<Digits>())
				};

				return ret;
			};

		public:
			// constructors
			RealWrapper(void)
				: m_value(SpecialValue::Empty) {};
			RealWrapper(const self_type &ano) = default;
			RealWrapper(self_type &&ano) = default;

			RealWrapper(const bool value)
				: m_value(value) {};
			RealWrapper(const integer_type &value)
				: RealWrapper(generate(value)) {};
			RealWrapper(const uinteger_type &value)
				: RealWrapper(generate(value)) {};
			RealWrapper(const decimal_type &value)
				: RealWrapper(generate(value)) {};
			RealWrapper(const rational_type &value)
				: RealWrapper(generate(value)) {};
			RealWrapper(const power_type &value)
				: RealWrapper(generate(value)) {};
			RealWrapper(const logarithm_type &value)
				: RealWrapper(generate(value)) {};
			RealWrapper(const TranscendentalValue value)
				: m_value(value) {};
			RealWrapper(const SpecialValue value)
				: m_value(value) {};
			RealWrapper(const std::string &str)
				: RealWrapper(generate(str)) {};
			RealWrapper(const Block &data)
				: RealWrapper(generate(data)) {};
			RealWrapper(const value_type &value)
				: m_value(value) {};
			template<typename T>
			RealWrapper(const T &value)
				: RealWrapper(generate(value)) {};

			// destructor
			~RealWrapper(void) = default;

			// generators
			static self_type generate(const value_type &value)
			{
				return self_type(value);
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && std::numeric_limits<T>::is_signed && std::is_convertible_v<T, typename integer_type::value_type>, self_type> generate(const T &integer_value)
			{
				self_type ret;
				ret.assign(integer_type(typename integer_type::value_type(integer_value)));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && std::numeric_limits<T>::is_signed && !std::is_convertible_v<T, typename integer_type::value_type>, self_type> generate(const T &integer_value)
			{
				self_type ret;
				ret.assign(integer_type(static_cast<typename integer_type::value_type>(integer_value)));
				return ret;
			}
			static self_type generate(const integer_type &integer_value)
			{
				self_type ret;
				ret.assign(integer_value);
				return ret;
			}
			static self_type generate(const integer &integer_value)
			{
				return generate(integer_type(integer_value));
			}
			static self_type generate(const rational_type &rational_value)
			{
				self_type ret;
				ret.assign(rational_value);
				return ret;
			}
			static self_type generate(const rational &rational_value)
			{
				return generate(rational_type(rational_value));
			}
			template<uint32 _Digits>
			static typename std::enable_if_t<_Digits != Digits, self_type> generate(const RationalWrapper<_Digits> &rational_value)
			{
				return generate(rational_type(rational_value));
			}

			template<typename T>
			static typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && !std::numeric_limits<T>::is_signed && std::is_convertible_v<T, typename uinteger_type::value_type>, self_type> generate(const T &uinteger_value)
			{
				self_type ret;
				ret.assign(uinteger_type(typename uinteger_type::value_type(uinteger_value)));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && !std::numeric_limits<T>::is_signed && !std::is_convertible_v<T, typename uinteger_type::value_type>, self_type> generate(const T &uinteger_value)
			{
				self_type ret;
				ret.assign(uinteger_type(static_cast<typename uinteger_type::value_type>(uinteger_value)));
				return ret;
			}
			static self_type generate(const uinteger_type &uinteger_value)
			{
				self_type ret;
				ret.assign(uinteger_value);
				return ret;
			}
			static self_type generate(const bool boolean_value)
			{
				return self_type(boolean_value);
			}

			template<typename T>
			static typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_specialized && !std::numeric_limits<T>::is_integer && std::is_convertible_v<T, typename decimal_type::value_type>, self_type> generate(const T &decimal_value)
			{
				self_type ret;
				ret.assign(decimal_type(typename decimal_type::value_type(decimal_value)));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_specialized && !std::numeric_limits<T>::is_integer && !std::is_convertible_v<T, typename decimal_type::value_type>, self_type> generate(const T &decimal_value)
			{
				self_type ret;
				ret.assign(decimal_type(static_cast<typename decimal_type::value_type>(decimal_value)));
				return ret;
			}
			static self_type generate(const decimal_type &decimal_value)
			{
				self_type ret;
				ret.assign(decimal_value);
				return ret;
			}
			static self_type generate(const power_type &power_value)
			{
				self_type ret;
				ret.assign(power_value);
				return ret;
			}
			static self_type generate(const logarithm_type &logarithm_value)
			{
				self_type ret;
				ret.assign(logarithm_value);
				return ret;
			}
			template<uint32 _Digits>
			static typename std::enable_if_t<_Digits != Digits, self_type> generate(const decimal<_Digits> &decimal_value)
			{
				self_type ret;
				ret.assign(decimal_type(decimal_value));
				return ret;
			}
			template<uint32 _Digits>
			static typename std::enable_if_t<_Digits != Digits, self_type> generate(const DecimalWrapper<_Digits> &decimal_value)
			{
				self_type ret;
				ret.assign(decimal_type(decimal_value));
				return ret;
			}
			template<uint32 _Digits>
			static typename std::enable_if_t<_Digits != Digits, self_type> generate(const PowerWrapper<_Digits> &power_value)
			{
				self_type ret;
				ret.assign(power_type(power_value));
				return ret;
			}
			template<uint32 _Digits>
			static typename std::enable_if_t<_Digits != Digits, self_type> generate(const LogarithmWrapper<_Digits> &logarithm_value)
			{
				self_type ret;
				ret.assign(logarithm_type(logarithm_value));
				return ret;
			}

			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_array_v<T> && !std::numeric_limits<T>::is_specialized && std::is_convertible_v<T, typename decimal_type::value_type>, self_type> generate(const T &value)
			{
				self_type ret;
				ret.assign(decimal_type(typename decimal_type::value_type(value)));
				return ret;
			}
			template<typename T>
			static typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_array_v<T> && !std::numeric_limits<T>::is_specialized && !std::is_convertible_v<T, typename decimal_type::value_type>, self_type> generate(const T &value)
			{
				self_type ret;
				ret.assign(decimal_type(static_cast<typename decimal_type::value_type>(value)));
				return ret;
			}
			static self_type generate(const std::string &str)
			{
				self_type ret;
				ret.assign(str);
				return ret;
			}
			static self_type generate(const Block &block)
			{
				self_type ret;
				ret.assign(block);
				return ret;
			}
			static self_type generate(const TranscendentalValue value)
			{
				return self_type(value);
			}
			static self_type generate(const SpecialValue value)
			{
				return self_type(value);
			}

			// assign and swap
			self_type &assign(const self_type &ano)
			{
				m_value = ano.m_value;
				return *this;
			}
			self_type &assign(const value_type &ano)
			{
				m_value = ano;
				return *this;
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits != _Digits && _Digits != 0, self_type> &assign(const RealWrapper<_Digits> &ano)
			{
				m_value = visit<differnet_digits_value_type_translator>(ano.value());
				return *this;
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits != _Digits && _Digits != 0, self_type> &assign(const typename RealWrapper<_Digits>::value_type &ano)
			{
				m_value = visit<differnet_digits_value_type_translator>(ano);
				return *this;
			}

			self_type &swap(self_type &ano)
			{
				m_value.swap(ano.m_value);
				return *this;
			}
			self_type &swap(value_type &ano)
			{
				m_value.swap(ano);
				return *this;
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits != _Digits && _Digits != 0, self_type> &swap(RealWrapper<_Digits> &ano)
			{
				value_type temp(m_value);
				m_value = visit<differnet_digits_value_type_translator>(ano.value());
				ano.assign(temp);
				return *this;
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits != _Digits && _Digits != 0, self_type> &swap(typename RealWrapper<_Digits>::value_type &ano)
			{
				value_type temp(m_value);
				m_value = visit<differnet_digits_value_type_translator>(ano);
				ano.assign(temp);
				return *this;
			}

			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && std::numeric_limits<T>::is_signed && std::is_convertible_v<T, typename integer_type::value_type>, self_type> &assign(const T &integer_value)
			{
				return assign(integer_type(typename integer_type::value_type(integer_value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && std::numeric_limits<T>::is_signed && !std::is_convertible_v<T, typename integer_type::value_type>, self_type> &assign(const T &integer_value)
			{
				return assign(integer_type(static_cast<typename integer_type::value_type>(integer_value)));
			}
			self_type &assign(const integer_type &integer_value)
			{
				m_value = integer_value;
				return *this;
			}
			self_type &assign(const integer &integer_value)
			{
				return assign(integer_type(integer_value));
			}
			self_type &assign(const rational_type &rational_value)
			{
				if (rational_value.valid())
				{
					m_value = rational_value;
				}
				else
				{
					m_value = SpecialValue::NaN;
				}
				return *this;
			}
			self_type &assign(const rational &rational_value)
			{
				return assign(rational_type(rational_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const RationalWrapper<_Digits> &rational_value)
			{
				return assign(rational_type(rational_value));
			}

			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && !std::numeric_limits<T>::is_signed && std::is_convertible_v<T, typename uinteger_type::value_type>, self_type> &assign(const T &uinteger_value)
			{
				return assign(uinteger_type(typename uinteger_type::value_type(uinteger_value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && !std::numeric_limits<T>::is_signed && !std::is_convertible_v<T, typename uinteger_type::value_type>, self_type> &assign(const T &uinteger_value)
			{
				return assign(uinteger_type(static_cast<typename uinteger_type::value_type>(uinteger_value)));
			}
			self_type &assign(const uinteger_type &uinteger_value)
			{
				m_value = uinteger_value;
				return *this;
			}
			self_type &assign(const bool &boolean_value)
			{
				m_value = boolean_value;
				return *this;
			}

			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_specialized && !std::numeric_limits<T>::is_integer && std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &assign(const T &decimal_value)
			{
				return assign(decimal_type(typename decimal_type::value_type(decimal_value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_specialized && !std::numeric_limits<T>::is_integer && !std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &assign(const T &decimal_value)
			{
				return assign(decimal_type(static_cast<typename decimal_type::value_type>(decimal_value)));
			}
			self_type &assign(const decimal_type &decimal_value)
			{
				if (decimal_value.isPositiveInfinity() || decimal_value.isInfinity())
				{
					m_value = SpecialValue::PositiveInfinity;
				}
				else if (decimal_value.isNegativeInfinity())
				{
					m_value = SpecialValue::NegativeInfinity;
				}
				else if (decimal_value.isNaN())
				{
					m_value = SpecialValue::NaN;
				}
				else
				{
					m_value = decimal_value;
				}
				return *this;
			}
			self_type &assign(const power_type &power_value)
			{
				if (power_value.isPositiveInfinity() || power_value.isInfinity())
				{
					m_value = SpecialValue::PositiveInfinity;
				}
				else if (power_value.isNegativeInfinity())
				{
					m_value = SpecialValue::NegativeInfinity;
				}
				else if (power_value.isNaN())
				{
					m_value = SpecialValue::NaN;
				}
				else
				{
					m_value = power_value;
				}
				return *this;
			}
			self_type &assign(const logarithm_type &logarithm_value)
			{
				if (logarithm_value.isPositiveInfinity() || logarithm_value.isInfinity())
				{
					m_value = SpecialValue::PositiveInfinity;
				}
				else if (logarithm_value.isNegativeInfinity())
				{
					m_value = SpecialValue::NegativeInfinity;
				}
				else if (logarithm_value.isNaN())
				{
					m_value = SpecialValue::NaN;
				}
				else
				{
					m_value = logarithm_value;
				}
				return *this;
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const decimal<_Digits> &decimal_value)
			{
				return assign(decimal_type(decimal_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const DecimalWrapper<_Digits> &decimal_value)
			{
				return assign(decimal_type(decimal_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const PowerWrapper<_Digits> &power_value)
			{
				return assign(power_type(power_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &assign(const LogarithmWrapper<_Digits> &logarithm_value)
			{
				return assign(logarithm_type(logarithm_value));
			}

			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_array_v<T> && !std::numeric_limits<T>::is_specialized && std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &assign(const T &value)
			{
				return assign(decimal_type(typename decimal_type::value_type(value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_array_v<T> && !std::numeric_limits<T>::is_specialized && !std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &assign(const T &value)
			{
				return assign(decimal_type(static_cast<typename decimal_type::value_type>(value)));
			}
			self_type &assign(const std::string &str)
			{
				auto boolean_it = String2Boolean().left.find(str);
				if (boolean_it != String2Boolean().left.end())
				{
					return assign(boolean_it->second);
				}

				auto transcendental_it = String2TranscendentalValue().left.find(str);
				if (transcendental_it != String2TranscendentalValue().left.end())
				{
					return assign(transcendental_it->second);
				}

				auto special_it = String2SpecialValue().left.find(str);
				if (special_it != String2SpecialValue().left.end())
				{
					return assign(special_it->second);
				}

				if (String::isInteger(str))
				{
					return assign(integer_type(str));
				}
				else if (String::isDecimal(str))
				{
					return assign(decimal_type(str));
				}
				else if (rational_type::RegexChecker(str))
				{
					return assign(rational_type(str));
				}
				else if (power_type::RegexChecker(str))
				{
					return assign(power_type(str));
				}
				else if (logarithm_type::RegexChecker(str))
				{
					return assign(logarithm_type(str));
				}
				return *this;
			}
			self_type &assign(const Block &block)
			{
				return assign(Data::toString(block));
			}
			self_type &assign(const TranscendentalValue value)
			{
				if (value == TranscendentalValue::none)
				{
					m_value = SpecialValue::Empty;
				}
				else
				{
					m_value = value;
				}
				return *this;
			}
			self_type &assign(const SpecialValue value)
			{
				m_value = value == SpecialValue::none ? SpecialValue::Empty : value;
				return *this;
			}

			// operator =
			self_type &operator=(const self_type &rhs) = default;
			self_type &operator=(const value_type &rhs)
			{
				return assign(rhs);
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits != _Digits && _Digits != 0, self_type> &operator=(const RealWrapper<_Digits> &ano)
			{
				return assign(ano);
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits != _Digits && _Digits != 0, self_type> &operator=(const typename RealWrapper<_Digits>::value_type &ano)
			{
				return assign(ano);
			}

			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && std::numeric_limits<T>::is_signed && std::is_convertible_v<T, typename integer_type::value_type>, self_type> &operator=(const T &integer_value)
			{
				return assign(integer_type(typename integer_type::value_type(integer_value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && std::numeric_limits<T>::is_signed && !std::is_convertible_v<T, typename integer_type::value_type>, self_type> &operator=(const T &integer_value)
			{
				return assign(integer_type(static_cast<typename integer_type::value_type>(integer_value)));
			}
			self_type &operator=(const integer_type &integer_value)
			{
				return assign(integer_value);
			}
			self_type &operator=(const integer &integer_value)
			{
				return assign(integer_type(integer_value));
			}
			self_type &operator=(const rational_type &rational_value)
			{
				return assign(rational_value);
			}
			self_type &operator=(const rational &rational_value)
			{
				return assign(rational_type(rational_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &operator=(const RationalWrapper<_Digits> &rational_value)
			{
				return assign(rational_type(rational_value));
			}

			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && !std::numeric_limits<T>::is_signed && std::is_convertible_v<T, typename uinteger_type::value_type>, self_type> &operator=(const T &uinteger_value)
			{
				return assign(uinteger_type(typename uinteger_type::value_type(uinteger_value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_integer && !std::numeric_limits<T>::is_signed && !std::is_convertible_v<T, typename uinteger_type::value_type>, self_type> &operator=(const T &uinteger_value)
			{
				return assign(uinteger_type(static_cast<typename uinteger_type::value_type>(uinteger_value)));
			}
			self_type &operator=(const uinteger_type &uinteger_value)
			{
				return assign(uinteger_value);
			}
			self_type &operator=(const bool &boolean_value)
			{
				return assign(boolean_value);
			}

			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_specialized && !std::numeric_limits<T>::is_integer && std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &operator=(const T &decimal_value)
			{
				return assign(decimal_type(typename decimal_type::value_type(decimal_value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_array_v<T> && std::numeric_limits<T>::is_specialized && !std::numeric_limits<T>::is_integer && !std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &operator=(const T &decimal_value)
			{
				return assign(decimal_type(static_cast<typename decimal_type::value_type>(decimal_value)));
			}
			self_type &operator=(const decimal_type &decimal_value)
			{
				return assign(decimal_value);
			}
			self_type &operator=(const power_type &power_value)
			{
				return assign(power_value);
			}
			self_type &operator=(const logarithm_type &logarithm_value)
			{
				return assign(logarithm_value);
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &operator=(const decimal<_Digits> &decimal_value)
			{
				return assign(decimal_type(decimal_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &operator=(const DecimalWrapper<_Digits> &decimal_value)
			{
				return assign(decimal_type(decimal_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &operator=(const PowerWrapper<_Digits> &power_value)
			{
				return assign(power_type(power_value));
			}
			template<uint32 _Digits>
			typename std::enable_if_t<_Digits != Digits, self_type> &operator=(const LogarithmWrapper<_Digits> &logarithm_value)
			{
				return assign(logarithm_type(logarithm_value));
			}

			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_array_v<T> && !std::numeric_limits<T>::is_specialized && std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &operator=(const T &value)
			{
				return assign(decimal_type(typename decimal_type::value_type(value)));
			}
			template<typename T>
			typename std::enable_if_t<!std::is_same_v<T, self_type> && !std::is_array_v<T> && !std::numeric_limits<T>::is_specialized && !std::is_convertible_v<T, typename decimal_type::value_type>, self_type> &operator=(const T &value)
			{
				return assign(decimal_type(static_cast<typename decimal_type::value_type>(value)));
			}
			self_type &operator=(const std::string &str)
			{
				return assign(str);
			}
			self_type &operator=(const Block &block)
			{
				return assign(block);
			}
			self_type &operator=(const TranscendentalValue value)
			{
				return assign(value);
			}
			self_type &operator=(const SpecialValue value)
			{
				return assign(value);
			}

			// set and get
			const value_type &value(void) const { return m_value; }
			typename decimal_type::value_type value_dec(void) const 
			{ 
				auto value = visit<to_decimal_visitor>();
				if (value.first)
				{
					return value.second.value();
				}
				else if (isEmpty() || isNaN())
				{
					return std::numeric_limits<typename decimal_type::value_type>::quiet_NaN();
				}
				else
				{
					return std::numeric_limits<typename decimal_type::value_type>::infinity();
				}
			}
			typename decimal_type value_dec_wrapper(void) const
			{
				return decimal_type(value_dec());
			}
			void setValue(const value_type &value) { m_value = value; }
			operator typename decimal_type::value_type(void) const
			{
				return value_dec();
			}

			Type type(void) const { return static_cast<Type>(m_value.which()); }

			const bool isPositiveInfinity(void) const { return visit<positive_infinity_visitor>(); }
			const bool isNegativeInfinity(void) const { return visit<negative_infinity_visitor>(); }
			const bool isNaN(void) const { return visit<nan_visitor>(); }
			const bool isEmpty(void) const { return visit<empty_visitor>(); }

			std::pair<bool, SpecialValue> getSpecialValue(void) const
			{
				return isPositiveInfinity() ? std::make_pair(true, SpecialValue::PositiveInfinity) 
				: isNegativeInfinity() ? std::make_pair(true, SpecialValue::NegativeInfinity) 
				: isNaN() ? std::make_pair(true, SpecialValue::NaN)
				: isEmpty() ? std::make_pair(true, SpecialValue::Empty)
				: std::make_pair(false, SpecialValue::none);
			}
			std::pair<bool, TranscendentalValue> getTranscendentailValue(void) const
			{
				return type() == Type::Transcendental ? std::make_pair(true, m_value.get<TranscendentalValue>()) : std::make_pair(false, TranscendentalValue::none);
			}

			// translators
			std::string toString(void) const
			{
				return visit<to_string_visitor>();
			}
			Block toBlock(void) const
			{
				return visit<to_block_visitor>();
			}

			std::pair<bool, bool> toBoolean(void) const
			{
				return visit<to_boolean_visitor>();
			}

			std::pair<bool, int8> toInt8(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt8());
				}
				else
				{
					return std::make_pair(false, int8(0));
				}
			}
			std::pair<bool, uint8> toUInt8(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt8());
				}
				else
				{
					return std::make_pair(false, uint8(0));
				}
			}
			std::pair<bool, int16> toInt16(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt16());
				}
				else
				{
					return std::make_pair(false, int16(0));
				}
			}
			std::pair<bool, uint16> toUInt16(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt16());
				}
				else
				{
					return std::make_pair(false, uint16(0));
				}
			}
			std::pair<bool, int32> toInt32(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt32());
				}
				else
				{
					return std::make_pair(false, int32(0));
				}
			}
			std::pair<bool, uint32> toUInt32(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt32());
				}
				else
				{
					return std::make_pair(false, uint32(0));
				}
			}
			std::pair<bool, int64> toInt64(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt64());
				}
				else
				{
					return std::make_pair(false, int64(0));
				}
			}
			std::pair<bool, uint64> toUInt64(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt64());
				}
				else
				{
					return std::make_pair(false, uint64(0));
				}
			}
			std::pair<bool, int128> toInt128(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt128());
				}
				else
				{
					return std::make_pair(false, int128());
				}
			}
			std::pair<bool, uint128> toUInt128(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt128());
				}
				else
				{
					return std::make_pair(false, uint128());
				}
			}
			std::pair<bool, int256> toInt256(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt256());
				}
				else
				{
					return std::make_pair(false, int256());
				}
			}
			std::pair<bool, uint256> toUInt256(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt256());
				}
				else
				{
					return std::make_pair(false, uint256());
				}
			}
			std::pair<bool, int512> toInt512(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt512());
				}
				else
				{
					return std::make_pair(false, int512());
				}
			}
			std::pair<bool, uint512> toUInt512(const RoundFlag flag = RoundFlag::round) const			
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt512());
				}
				else
				{
					return std::make_pair(false, uint512());
				}
			}
			std::pair<bool, int1024> toInt1024(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toInt1024());
				}
				else
				{
					return std::make_pair(false, int1024());
				}
			}
			std::pair<bool, uint1024> toUInt1024(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUInt1024());
				}
				else
				{
					return std::make_pair(false, uint1024());
				}
			}

			template<uint32 bits>
			std::pair<bool, intx<bits>> toIntx(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toIntx<bits>());
				}
				else
				{
					return std::make_pair(false, intx<bits>());
				}
			}
			template<uint32 bits>
			std::pair<bool, uintx<bits>> toUIntx(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.toUIntx<bits>());
				}
				else
				{
					return std::make_pair(false, uintx<bits>());
				}
			}
			std::pair<bool, integer> toInteger(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = toIntegerWrapper(flag);
				if (value.first)
				{
					return std::make_pair(true, value.value());
				}
				else
				{
					return std::make_pair(false, integer(0));
				}
			}
			std::pair<bool, Integer> toIntegerWrapper(const RoundFlag flag = RoundFlag::round) const
			{
				auto value = visit(to_integer_visitor(flag));
				if (value.first)
				{
					return std::make_pair(true, value);
				}
				else
				{
					return std::make_pair(false, Integer());
				}
			}

			std::pair<bool, float> toFloat(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toFloat());
				}
				else
				{
					return std::make_pair(false, float(0.0f));
				}
			}
			std::pair<bool, double> toDouble(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toDouble());
				}
				else
				{
					return std::make_pair(false, double(0.0f));
				}
			}
			std::pair<bool, float32> toFloat32(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toFloat32());
				}
				else
				{
					return std::make_pair(false, float32());
				}
			}
			std::pair<bool, float64> toFloat64(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toFloat64());
				}
				else
				{
					return std::make_pair(false, float64());
				}
			}
			std::pair<bool, float128> toFloat128(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toFloat128());
				}
				else
				{
					return std::make_pair(false, float128());
				}
			}
			std::pair<bool, float256> toFloat256(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toFloat256());
				}
				else
				{
					return std::make_pair(false, float256());
				}
			}

			std::pair<bool, dec50> toDec50(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toDec50());
				}
				else
				{
					return std::make_pair(false, dec50());
				}
			}
			std::pair<bool, dec100> toDec100(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toDec100());
				}
				else
				{
					return std::make_pair(false, dec100());
				}
			}
			std::pair<bool, real> toReal(void) const
			{
				auto value = toDecimalWrapper<Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.toReal());
				}
				else
				{
					return std::make_pair(false, real());
				}
			}
			template<uint32 _Digits = DefaultDigits>
			std::pair<bool, decimal<_Digits>> toDecimal(void) const
			{
				auto value = toDecimalWrapper<_Digits>();
				if (value.first)
				{
					return std::make_pair(true, value.value());
				}
				else
				{
					return std::make_pair(false, decimal<_Digits>());
				}
			}
			template<uint32 _Digits = DefaultDigits>
			std::pair<bool, DecimalWrapper<_Digits>> toDecimalWrapper(void) const
			{
				auto value = visit<to_decimal_visitor>();
				if (value.first)
				{
					return std::make_pair(true, DecimalWrapper<_Digits>(value.second));
				}
				else if (isPositiveInfinity())
				{
					return std::make_pair(true, DecimalWrapper<_Digits>(std::numeric_limits<typename DecimalWrapper<Digits>::value_type>::infinity()));
				}
				else if (isNegativeInfinity())
				{
					return std::make_pair(true, DecimalWrapper<_Digits>(-std::numeric_limits<typename DecimalWrapper<Digits>::value_type>::infinity()));
				}
				else if (!isEmpty())
				{
					return std::make_pair(true, DecimalWrapper<_Digits>(std::numeric_limits<typename DecimalWrapper<Digits>::value_type>::quiet_NaN()));
				}
				else
				{
					return std::make_pair(false, DecimalWrapper<_Digits>());
				}
			}

 			template<typename T>
 			typename std::enable_if_t<std::numeric_limits<T>::is_specialized || std::is_same_v<T, std::string> || std::is_same_v<T, Block> || std::is_same_v<T, TranscendentalValue> || std::is_same_v<T, SpecialValue>, T> get(void) const;

			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, RealWrapper<_Digits>> round(void) const
			{
				return visit<round_visitor<_Digits>>();
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, RealWrapper<_Digits>> ceil(void) const
			{
				return visit<ceil_visitor<_Digits>>();
			}
			template<uint32 _Digits = DefaultDigits>
			typename std::enable_if_t<Digits >= _Digits && _Digits != 0, RealWrapper<_Digits>> floor(void) const
			{
				return visit<floor_visitor<_Digits>>();
			}

			self_type roundToInteger(void) const
			{
				self_type ret;
				auto value = visit(to_integer_visitor(RoundFlag::round));
				if (value.first)
				{
					ret.assign(value.second);
				}
				else if (!isEmpty())
				{
					ret.assign(SpecialValue::NaN);
				}
				return ret;
			}
			self_type ceilToInteger(void) const
			{
				self_type ret;
				auto value = visit(to_integer_visitor(RoundFlag::ceil));
				if (value.first)
				{
					ret.assign(value.second);
				}
				else if (!isEmpty())
				{
					ret.assign(SpecialValue::NaN);
				}
				return ret;
			}
			self_type floorToInteger(void) const
			{
				self_type ret;
				auto value = visit(to_integer_visitor(RoundFlag::floor));
				if (value.first)
				{
					ret.assign(value.second);
				}
				else if (!isEmpty())
				{
					ret.assign(SpecialValue::NaN);
				}
				return ret;
			}

		private:
			value_type m_value;
		};

		template<uint32 Digits>
		const RealWrapper<Digits> RealWrapper<Digits>::pi = RealWrapper<Digits>(RealWrapper<Digits>::TranscendentalValue::pi);
		template<uint32 Digits>
		const RealWrapper<Digits> RealWrapper<Digits>::e = RealWrapper<Digits>(RealWrapper<Digits>::TranscendentalValue::e);
		template<uint32 Digits>
		const RealWrapper<Digits> RealWrapper<Digits>::root_2 = RealWrapper<Digits>(RealWrapper<Digits>::power_type(2, 0.5));
		template<uint32 Digits>
		const RealWrapper<Digits> RealWrapper<Digits>::root_3 = RealWrapper<Digits>(RealWrapper<Digits>::power_type(3, 0.5));
		template<uint32 Digits>
		const RealWrapper<Digits> RealWrapper<Digits>::ln_2 = RealWrapper<Digits>(RealWrapper<Digits>::logarithm_type(RealWrapper<Digits>::TranscendentalValue2Decimal().find(RealWrapper<Digits>::TranscendentalValue::e)->second, 2));
		template<uint32 Digits>
		const RealWrapper<Digits> RealWrapper<Digits>::lg_2 = RealWrapper<Digits>(RealWrapper<Digits>::logarithm_type(10, 2));

		template<uint32 Digits>
		integer round(const RealWrapper<Digits> &value)
		{
			return value.roundToInteger().toInteger().second;
		}
		template<uint32 Digits>
		integer ceil(const RealWrapper<Digits> &value)
		{
			return value.ceilToInteger().toInteger().second;
		}
		template<uint32 Digits>
		integer floor(const RealWrapper<Digits> &value)
		{
			return value.floorToInteger().toInteger().second;
		}
	};
};

template<SSUtils::uint32 Digits>
const bool operator==(const SSUtils::Math::RealWrapper<Digits> &lhs, const SSUtils::Math::RealWrapper<Digits> &rhs)
{
	return lhs.value() == rhs.value();
}

template<SSUtils::uint32 Digits>
const bool operator!=(const SSUtils::Math::RealWrapper<Digits> &lhs, const SSUtils::Math::RealWrapper<Digits> &rhs)
{
	return lhs.value() != rhs.value();
}

template<SSUtils::uint32 Digits>
const bool operator<(const SSUtils::Math::RealWrapper<Digits> &lhs, const SSUtils::Math::RealWrapper<Digits> &rhs)
{
	if (lhs.isEmpty())
	{
		return true;
	}
	if (rhs.isEmpty())
	{
		return false;
	}
	return lhs.value_dec() < rhs.value_dec();
}

template<SSUtils::uint32 Digits>
const bool operator<=(const SSUtils::Math::RealWrapper<Digits> &lhs, const SSUtils::Math::RealWrapper<Digits> &rhs)
{
	if (lhs.isEmpty())
	{
		return true;
	}
	if (rhs.isEmpty())
	{
		return false;
	}
	return lhs.value_dec() <= rhs.value_dec();
}

template<SSUtils::uint32 Digits>
const bool operator>(const SSUtils::Math::RealWrapper<Digits> &lhs, const SSUtils::Math::RealWrapper<Digits> &rhs)
{
	return !(lhs <= rhs);
}

template<SSUtils::uint32 Digits>
const bool operator>=(const SSUtils::Math::RealWrapper<Digits> &lhs, const SSUtils::Math::RealWrapper<Digits> &rhs)
{
	return !(lhs < rhs);
}


template<SSUtils::uint32 Digits>
std::istream &operator>>(std::istream &is, SSUtils::Math::RealWrapper<Digits> &value)
{
	std::string str;
	is >> str;
	value.assign(str);
	return is;
}

template<SSUtils::uint32 Digits>
std::ostream &operator<<(std::ostream &os, const SSUtils::Math::RealWrapper<Digits> &value)
{
	os << value.toString();
	return os;
}

namespace std
{
	template<SSUtils::uint32 Digits>
	class numeric_limits<SSUtils::Math::RealWrapper<Digits>>
		: public numeric_limits<typename SSUtils::Math::RealWrapper<Digits>::value_type>
	{};

	template<SSUtils::uint32 Digits>
	std::string to_string(const SSUtils::Math::RealWrapper<Digits> &value)
	{
		return value.toString();
	}

	template<SSUtils::uint32 Digits>
	SSUtils::Math::RealWrapper<Digits> storeal_wrapper(const std::string &str)
	{
		return SSUtils::Math::RealWrapper<Digits>(str);
	}
};

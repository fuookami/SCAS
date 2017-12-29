#pragma once

#include "DataUtils.h"

#include <boost/bimap.hpp>
#include <boost/variant.hpp>

#include <typeinfo>
#include <map>
#include <set>
#include <functional>
#include <utility>
#include <exception>
#include <cstdint>

class NumberVariant
{
	friend const bool operator<(const NumberVariant &lhs, const NumberVariant &rhs);
	friend const bool operator<=(const NumberVariant &lhs, const NumberVariant &rhs);
	friend const bool operator>(const NumberVariant &lhs, const NumberVariant &rhs);
	friend const bool operator>=(const NumberVariant &lhs, const NumberVariant &rhs);
	friend const bool operator==(const NumberVariant &lhs, const NumberVariant &rhs);
	friend const int operator!=(const NumberVariant &lhs, const NumberVariant &rhs);

public:
	using int8 = std::int8_t;
	using uint8 = std::uint8_t;
	using int16 = std::int16_t;
	using uint16 = std::uint16_t;
	using int32 = std::int32_t;
	using uint32 = std::uint32_t;
	using int64 = std::int64_t;
	using uint64 = std::uint64_t;
	using ValueType = boost::variant<bool, float, double, int8, uint8, int16, uint16, int32, uint32, int64, uint64>;

	enum class eValidType
	{
		tBool = 1,
		tFloat,
		tDouble,
		tInt8,
		tUInt8,
		tInt16,
		tUInt16,
		tInt32,
		tUInt32,
		tInt64,
		tUInt64
	};

private:
	enum class eClassfication
	{
		tBoolean,
		tFloat,
		tInteger,
		tUnsigned
	};

	static const boost::bimap<std::string, eValidType> TypeName2Type;
	static const std::map<eValidType, std::set<eValidType>> ValidTypeTransformation;
	static const std::map<eValidType, eClassfication> Type2Classfication;
	static const boost::bimap<std::string, eValidType> __initTypeName2Type(void);

	template<typename T>
	static const bool set(NumberVariant &dest, T src)
	{
		const auto &typeInfo(typeid(T));

		auto it(TypeName2Type.left.find(std::string(typeInfo.name())));
		if (it != TypeName2Type.left.end())
		{
			dest.m_type = it->second;
			dest.m_value = src;
			dest.refreshClassfication();
			dest.m_empty = false;
			return true;
		}
		else
		{
			static const std::string ErrorMessagePrefix("Invalid Type: ");
			throw std::logic_error(ErrorMessagePrefix + typeInfo.name());
		}
	}

	template <typename T>
	static const T getValue(const eValidType type, const ValueType &value)
	{
		switch (type)
		{
		case eValidType::tBool:
			return static_cast<T>(boost::get<bool>(value));
			break;
		case eValidType::tFloat:
			return static_cast<T>(boost::get<float>(value));
			break;
		case eValidType::tDouble:
			return static_cast<T>(boost::get<double>(value));
			break;
		case eValidType::tInt8:
			return static_cast<T>(boost::get<int8>(value));
			break;
		case eValidType::tUInt8:
			return static_cast<T>(boost::get<uint8>(value));
			break;
		case eValidType::tInt16:
			return static_cast<T>(boost::get<int16>(value));
			break;
		case eValidType::tUInt16:
			return static_cast<T>(boost::get<uint16>(value));
			break;
		case eValidType::tInt32:
			return static_cast<T>(boost::get<int32>(value));
			break;
		case eValidType::tUInt32:
			return static_cast<T>(boost::get<uint32>(value));
			break;
		case eValidType::tInt64:
			return static_cast<T>(boost::get<int64>(value));
			break;
		case eValidType::tUInt64:
			return static_cast<T>(boost::get<uint64>(value)); 
			break;
		default:
			return static_cast<T>(0); 
			break;
		}

		return static_cast<T>(0);
	}

	template <typename T>
	static const T get(const NumberVariant &dest)
	{
		if (dest.m_empty)
		{
			const auto &typeInfo(typeid(T));

			auto it(TypeName2Type.left.find(std::string(typeInfo.name())));
			if (it != TypeName2Type.left.end())
			{
				eValidType targetType(it->second);

				if (targetType == dest.m_type)
				{
					return boost::get<T>(dest.m_value);
				}
				else
				{
					const std::set<eValidType> validTypeSet(ValidTypeTransformation.find(dest.m_type)->second);
					if (validTypeSet.find(targetType) != validTypeSet.cend())
					{
						return getValue<T>(dest.m_type, dest.m_value);
					}
					else
					{
						static const std::string ErrorMessagePrefix("Invalid Type Transformation: ");
						static const std::string TypeTransformationSeparator(" --> ");
						throw std::logic_error(ErrorMessagePrefix + typeInfo.name() + TypeTransformationSeparator + TypeName2Type.right.find(dest.m_type)->second);
					}
				}
			}
			else
			{
				try
				{
					return getValue<T>(dest.m_type, dest.m_value);
				}
				catch (std::exception &e)
				{
					static const std::string ErrorMessage("Invalid Type And Cannot transform to: ");
					throw std::logic_error(ErrorMessage + typeInfo.name() + "\n" + e.what());
				}
			}
		}
		else
		{
			static const std::string ErrorMessage("Cannot get value from a empty variant.");
			throw std::logic_error(ErrorMessage);
		}
	}

public:
	NumberVariant(void);
	NumberVariant(const bool value);
	NumberVariant(const float value);
	NumberVariant(const double value);
	NumberVariant(const int8 value);
	NumberVariant(const uint8 value);
	NumberVariant(const int16 value);
	NumberVariant(const uint16 value);
	NumberVariant(const int32 value);
	NumberVariant(const uint32 value);
	NumberVariant(const int64 value);
	NumberVariant(const uint64 value);
	NumberVariant(const std::string &str);
	NumberVariant(const DataUtils::Data &data);
	NumberVariant(const NumberVariant &ano);
	NumberVariant(const NumberVariant &&ano);
	NumberVariant &operator=(const NumberVariant &rhs);
	NumberVariant &operator=(const NumberVariant &&rhs);
	~NumberVariant(void);

	inline const bool set(const bool value) { return set(*this, value); }
	inline const bool set(const float value) { return set(*this, value); }
	inline const bool set(const double value) { return set(*this, value); }
	inline const bool set(const int8 value) { return set(*this, value); }
	inline const bool set(const uint8 value) { return set(*this, value); }
	inline const bool set(const int16 value) { return set(*this, value); }
	inline const bool set(const uint16 value) { return set(*this, value); }
	inline const bool set(const int32 value) { return set(*this, value); }
	inline const bool set(const uint32 value) { return set(*this, value); }
	inline const bool set(const int64 value) { return set(*this, value); }
	inline const bool set(const uint64 value) { return set(*this, value); }

	template<typename T>
	inline const T get(void) const { return get<T>(*this); }

	const bool getBool(void) const;
	const float getFloat(void) const;
	const double getDouble(void) const;
	const int8 getInt8(void) const;
	const uint8 getUInt8(void) const;
	const int16 getInt16(void) const;
	const uint16 getUInt16(void) const;
	const int32 getInt32(void) const;
	const uint32 getUInt32(void) const;
	const int64 getInt64(void) const;
	const uint64 getUInt64(void) const;

	inline void assign(const NumberVariant &ano) { *this = ano; }
	inline void assign(const NumberVariant &&ano) { *this = std::move(ano); }
	void swap(NumberVariant &ano);

	inline const bool empty(void) const { return m_empty; }
	void clear(void);
	inline const eValidType type(void) const { return m_type; }

	const std::string toString(const int digit = -1, const int precision = -1) const;
	static const NumberVariant fromString(const std::string &str);

	const DataUtils::Data toData(void) const;
	static const NumberVariant fromData(const DataUtils::Data &data);

private:
	void refreshClassfication(void);

private:
	bool m_empty;
	eClassfication m_classfication;
	eValidType m_type;

	ValueType m_value;
	static const uint8 ValueDataOffset = 8;
	static const uint8 ValueDataLength = 8;
};

#include "NumberVariant.h"
#include "EndianUtils.h"

#include <boost/algorithm/string.hpp>

#include <iostream>
#include <sstream>
#include <iomanip>

const boost::bimap<std::string, NumberVariant::eValidType> NumberVariant::TypeName2Type = NumberVariant::_initTypeName2Type();

const std::map<NumberVariant::eValidType, std::set<NumberVariant::eValidType>> NumberVariant::ValidTypeTransformation =
{
	std::make_pair(eValidType::tBool, std::set<eValidType>(
		{eValidType::tInt8, eValidType::tUInt8, eValidType::tInt16, eValidType::tUInt16, eValidType::tInt32, eValidType::tUInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tFloat, std::set<eValidType>(
		{ eValidType::tDouble })),
	std::make_pair(eValidType::tDouble, std::set<eValidType>({})),
	std::make_pair(eValidType::tInt8, std::set<eValidType>(
		{ eValidType::tUInt8, eValidType::tInt16, eValidType::tUInt16, eValidType::tInt32, eValidType::tUInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tUInt8, std::set<eValidType>(
		{ eValidType::tInt8, eValidType::tInt16, eValidType::tUInt16, eValidType::tInt32, eValidType::tUInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tInt16, std::set<eValidType>(
		{ eValidType::tUInt16, eValidType::tInt32, eValidType::tUInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tUInt16, std::set<eValidType>(
		{ eValidType::tInt16, eValidType::tInt32, eValidType::tUInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tInt32, std::set<eValidType>(
		{ eValidType::tUInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tUInt32, std::set<eValidType>(
		{ eValidType::tInt32, eValidType::tInt64, eValidType::tUInt64 })),
	std::make_pair(eValidType::tInt64, std::set<eValidType>(
		{ eValidType::tUInt64 })),
	std::make_pair(eValidType::tUInt64, std::set<eValidType>(
		{ eValidType::tInt64 }))
};

const std::map<NumberVariant::eValidType, NumberVariant::eClassfication> NumberVariant::Type2Classfication =
{
	std::make_pair(eValidType::tBool, eClassfication::tBoolean),
	std::make_pair(eValidType::tFloat, eClassfication::tFloat),
	std::make_pair(eValidType::tDouble, eClassfication::tFloat),
	std::make_pair(eValidType::tInt8, eClassfication::tInteger),
	std::make_pair(eValidType::tUInt8, eClassfication::tUnsigned),
	std::make_pair(eValidType::tInt16, eClassfication::tInteger),
	std::make_pair(eValidType::tUInt16, eClassfication::tUnsigned),
	std::make_pair(eValidType::tInt32, eClassfication::tInteger),
	std::make_pair(eValidType::tUInt32, eClassfication::tUnsigned),
	std::make_pair(eValidType::tInt64, eClassfication::tInteger),
	std::make_pair(eValidType::tUInt64, eClassfication::tUnsigned)
};

const boost::bimap<std::string, NumberVariant::eValidType> NumberVariant::_initTypeName2Type(void)
{
	using PairType = boost::bimap<std::string, eValidType>::value_type;

	boost::bimap<std::string, NumberVariant::eValidType> ret;
	ret.insert(PairType(typeid(bool).name(), eValidType::tBool));
	ret.insert(PairType(typeid(float).name(), eValidType::tFloat));
	ret.insert(PairType(typeid(double).name(), eValidType::tDouble));
	ret.insert(PairType(typeid(int8).name(), eValidType::tInt8));
	ret.insert(PairType(typeid(uint8).name(), eValidType::tUInt8));
	ret.insert(PairType(typeid(int16).name(), eValidType::tInt16));
	ret.insert(PairType(typeid(uint16).name(), eValidType::tUInt16));
	ret.insert(PairType(typeid(int32).name(), eValidType::tInt32));
	ret.insert(PairType(typeid(uint32).name(), eValidType::tUInt32));
	ret.insert(PairType(typeid(int64).name(), eValidType::tInt64));
	ret.insert(PairType(typeid(uint64).name(), eValidType::tUInt64));

	return ret;
}

NumberVariant::NumberVariant(void)
	: m_empty(true), m_classfication(eClassfication::tBoolean), m_type(eValidType::tBool), m_value()
{
	m_value = false;
}

NumberVariant::NumberVariant(const bool value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const float value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const double value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const int8 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const uint8 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const int16 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const uint16 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const int32 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const uint32 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const int64 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const uint64 value)
	: NumberVariant()
{
	set(value);
}

NumberVariant::NumberVariant(const std::string & str)
	: NumberVariant(fromString(str))
{
}

NumberVariant::NumberVariant(const DataUtils::Data & data)
	: NumberVariant(fromData(data))
{
}

const bool NumberVariant::getBool(void) const
{
	try
	{
		return get<bool>();
	}
	catch (...)
	{
		return false;
	}
}

const float NumberVariant::getFloat(void) const
{
	try
	{
		return get<float>();
	}
	catch (...)
	{
		return 0.0f;
	}
}

const double NumberVariant::getDouble(void) const
{
	try
	{
		return get<double>();
	}
	catch (...)
	{
		return 0.0f;
	}
}

const NumberVariant::int8 NumberVariant::getInt8(void) const
{
	try
	{
		return get<int8>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::uint8 NumberVariant::getUInt8(void) const
{
	try
	{
		return get<uint8>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::int16 NumberVariant::getInt16(void) const
{
	try
	{
		return get<int16>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::uint16 NumberVariant::getUInt16(void) const
{
	try
	{
		return get<uint16>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::int32 NumberVariant::getInt32(void) const
{
	try
	{
		return get<int32>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::uint32 NumberVariant::getUInt32(void) const
{
	try
	{
		return get<uint32>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::int64 NumberVariant::getInt64(void) const
{
	try
	{
		return get<int64>();
	}
	catch (...)
	{
		return 0;
	}
}

const NumberVariant::uint64 NumberVariant::getUInt64(void) const
{
	try
	{
		return get<uint64>();
	}
	catch (...)
	{
		return 0;
	}
}

void NumberVariant::swap(NumberVariant & ano)
{
	std::swap(m_classfication, ano.m_classfication);
	std::swap(m_type, ano.m_type);
	std::swap(m_value, ano.m_value);
}

void NumberVariant::clear(void)
{
	set(false);
	m_empty = true;
}

std::string NumberVariant::getValueString(const int digit, const int precision) const
{
	if (m_empty)
	{
		static const std::string EmptyString("empty");
		return EmptyString;
	}
	else
	{
		static const std::string EmptyString("");

		std::string ret(EmptyString);
		switch (m_classfication)
		{
		case NumberVariant::eClassfication::tBoolean:
		{
			static const std::string TrueString("true");
			static const std::string FalseString("false");

			ret = get<bool>() ? TrueString : FalseString;
		}
		break;
		case NumberVariant::eClassfication::tFloat:
		{
			std::ostringstream floatSout(EmptyString);

			if (precision != -1)
			{
				floatSout << std::setprecision(precision) << get<double>();
			}
			else
			{
				floatSout << get<double>();
			}
			std::string numberStr(floatSout.str());

			if (digit != -1)
			{
				size_t pointPos(numberStr.find("."));
				if (numberStr.size() > (digit + 1) && pointPos != std::string::npos && pointPos < (digit + 1))
				{
					numberStr.erase(numberStr.begin() + digit + 1, numberStr.end());
				}
				else if (numberStr.size() > digit)
				{
					numberStr.erase(numberStr.begin() + digit, numberStr.end());
				}
			}

			ret = std::move(numberStr);
		}
		break;
		case NumberVariant::eClassfication::tInteger:
		{
			int64 number(get<int64>());
			std::string numberStr(std::to_string(number));

			if (digit != -1)
			{
				if (number < 0 && numberStr.size() >(digit + 1))
				{
					numberStr.erase(numberStr.begin() + digit + 1, numberStr.end());
				}
				else if (numberStr.size() > digit)
				{
					numberStr.erase(numberStr.begin() + digit, numberStr.end());
				}
			}

			ret = std::move(numberStr);
		}
		break;
		case NumberVariant::eClassfication::tUnsigned:
		{
			uint64 number(get<uint64>());
			std::string numberStr(std::to_string(number));

			if (digit != -1 && numberStr.size() > digit)
			{
				numberStr.erase(numberStr.begin() + digit, numberStr.end());
			}

			ret = std::move(numberStr);
		}
		break;
		default:
			ret = EmptyString;
			break;
		}

		return ret;
	}
}

std::string NumberVariant::toString(const int digit, const int precision) const
{
	if (m_empty)
	{
		static const std::string EmptyString("empty");
		return EmptyString;
	}
	else
	{
		static const std::string EmptyString("");
		static const std::string Seperator("_");

		std::string ret(EmptyString);
		switch (m_classfication)
		{
		case NumberVariant::eClassfication::tBoolean:
		{
			static const std::string TrueString("true");
			static const std::string FalseString("false");

			ret = get<bool>() ? TrueString : FalseString;
		}
			break;
		case NumberVariant::eClassfication::tFloat:
		{
			std::ostringstream floatSout(EmptyString);

			if (precision != -1)
			{
				floatSout << std::setprecision(precision) << get<double>();
			}
			else
			{
				floatSout << get<double>();
			}
			std::string numberStr(floatSout.str());

			if (digit != -1)
			{
				size_t pointPos(numberStr.find("."));
				if (numberStr.size() > (digit + 1) && pointPos != std::string::npos && pointPos < (digit + 1))
				{
					numberStr.erase(numberStr.begin() + digit + 1, numberStr.end());
				}
				else if (numberStr.size() > digit)
				{
					numberStr.erase(numberStr.begin() + digit, numberStr.end());
				}
			}

			ret = std::to_string(static_cast<int>(m_type)) + Seperator + numberStr;
		}
			break;
		case NumberVariant::eClassfication::tInteger:
		{
			int64 number(get<int64>());
			std::string numberStr(std::to_string(number));

			if (digit != -1)
			{
				if (number < 0 && numberStr.size() >(digit + 1))
				{
					numberStr.erase(numberStr.begin() + digit + 1, numberStr.end());
				}
				else if (numberStr.size() > digit)
				{
					numberStr.erase(numberStr.begin() + digit, numberStr.end());
				}
			}

			ret = std::to_string(static_cast<int>(m_type)) + Seperator + numberStr;
		}
			break;
		case NumberVariant::eClassfication::tUnsigned:
		{
			uint64 number(get<uint64>());
			std::string numberStr(std::to_string(number));

			if (digit != -1 && numberStr.size() > digit)
			{
				numberStr.erase(numberStr.begin() + digit, numberStr.end());
			}
			ret = std::to_string(static_cast<int>(m_type)) + Seperator + numberStr;
		}
			break;
		default:
			ret = EmptyString;
			break;
		}

		return ret;
	}
}

NumberVariant NumberVariant::fromString(const std::string & str)
{
	static const std::string Seperator("_");
	static const std::string EmptyString("empty");
	static const std::string TrueString("true");
	static const std::string FalseString("false");

	if (str == EmptyString)
	{
		return NumberVariant();
	}
	else if (str == TrueString)
	{
		return NumberVariant(true);
	}
	else if (str == FalseString)
	{
		return NumberVariant(false);
	}
	else
	{
		std::vector<std::string> parts;
		boost::split(parts, str, boost::is_any_of(Seperator));

		if (parts.size() == 2)
		{
			eValidType type(static_cast<eValidType>(std::stoi(parts.front())));

			NumberVariant number;
			switch (type)
			{
			case NumberVariant::eValidType::tFloat:
				number.set(std::stof(parts.back()));
				break;
			case NumberVariant::eValidType::tDouble:
				number.set(std::stod(parts.back()));
				break;
			case NumberVariant::eValidType::tInt8:
				number.set(static_cast<int8>(std::stoi(parts.back())));
				break;
			case NumberVariant::eValidType::tUInt8:
				number.set(static_cast<uint8>(std::stoul(parts.back())));
				break;
			case NumberVariant::eValidType::tInt16:
				number.set(static_cast<int16>(std::stoi(parts.back())));
				break;
			case NumberVariant::eValidType::tUInt16:
				number.set(static_cast<uint16>(std::stoul(parts.back())));
				break;
			case NumberVariant::eValidType::tInt32:
				number.set(static_cast<int32>(std::stoi(parts.back())));
				break;
			case NumberVariant::eValidType::tUInt32:
				number.set(static_cast<uint32>(std::stoul(parts.back())));
				break;
			case NumberVariant::eValidType::tInt64:
				number.set(std::stoll(parts.back()));
				break;
			case NumberVariant::eValidType::tUInt64:
				number.set(std::stoull(parts.back()));
				break;
			case NumberVariant::eValidType::tBool:
			default:
				return NumberVariant();
				break;
			}
		}
		else
		{
			return NumberVariant();
		}
	}

	return NumberVariant();
}

DataUtils::Data NumberVariant::toData(void) const
{	
	if (m_empty)
	{
		static const DataUtils::Data EmptyData({ 0x00 });
		return EmptyData;
	}
	else
	{
		static const DataUtils::Data EmptyData;

		DataUtils::byte flag(static_cast<DataUtils::byte>(m_type));
		flag <<= 1;
		flag |= static_cast<DataUtils::byte>(EndianUtils::Endian::BigEndian);

		DataUtils::Data ret;
		switch (m_type)
		{
		case NumberVariant::eValidType::tBool:
			ret = DataUtils::fromBool(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getBool()));
			break;
		case NumberVariant::eValidType::tFloat:
			ret = DataUtils::fromFloat(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getFloat()));
			break;
		case NumberVariant::eValidType::tDouble:
			ret = DataUtils::fromDouble(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getDouble()));
			break;
		case NumberVariant::eValidType::tInt8:
			ret = DataUtils::fromInt8(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getInt8()));
			break;
		case NumberVariant::eValidType::tUInt8:
			ret = DataUtils::fromUInt8(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getUInt8()));
			break;
		case NumberVariant::eValidType::tInt16:
			ret = DataUtils::fromInt16(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getUInt16()));
			break;
		case NumberVariant::eValidType::tUInt16:
			ret = DataUtils::fromUInt16(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getUInt16()));
			break;
		case NumberVariant::eValidType::tInt32:
			ret = DataUtils::fromInt32(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getInt32()));
			break;
		case NumberVariant::eValidType::tUInt32:
			ret = DataUtils::fromUInt32(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getUInt32()));
			break;
		case NumberVariant::eValidType::tInt64:
			ret = DataUtils::fromInt64(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getInt64()));
			break;
		case NumberVariant::eValidType::tUInt64:
			ret = DataUtils::fromUInt64(EndianUtils::fromLocalEndian(EndianUtils::Endian::BigEndian, getUInt64()));
			break;
		default:
			ret = EmptyData;
			break;
		}

		if (!ret.empty())
		{
			ret.insert(ret.begin(), flag);
		}
		return ret;
	}
}

NumberVariant NumberVariant::fromData(const DataUtils::Data & data)
{
	if (data.size() == 1 && data.front() == 0x00)
	{
		return NumberVariant();
	}
	else if (!data.empty())
	{
		DataUtils::byte flag(data.front());
		NumberVariant::eValidType type = static_cast<NumberVariant::eValidType>(flag >> 1);
		EndianUtils::Endian srcEndian = static_cast<EndianUtils::Endian>(flag & 0x01);

		NumberVariant number;
		DataUtils::Data valuePart(std::next(data.cbegin()), data.cend());
		switch (type)
		{
		case NumberVariant::eValidType::tBool:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toBool(valuePart)));
			break;
		case NumberVariant::eValidType::tFloat:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toFloat(valuePart)));
			break;
		case NumberVariant::eValidType::tDouble:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toDouble(valuePart)));
			break;
		case NumberVariant::eValidType::tInt8:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toInt8(valuePart)));
			break;
		case NumberVariant::eValidType::tUInt8:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toUInt8(valuePart)));
			break;
		case NumberVariant::eValidType::tInt16:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toInt16(valuePart)));
			break;
		case NumberVariant::eValidType::tUInt16:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toUInt16(valuePart)));
			break;
		case NumberVariant::eValidType::tInt32:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toInt32(valuePart)));
			break;
		case NumberVariant::eValidType::tUInt32:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toUInt32(valuePart)));
			break;
		case NumberVariant::eValidType::tInt64:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toInt64(valuePart)));
			break;
		case NumberVariant::eValidType::tUInt64:
			number.set(EndianUtils::toLocalEndian(srcEndian, DataUtils::toUInt64(valuePart)));
			break;
		default:
			break;
		}

		return number;
	}
	else
	{
		return NumberVariant();
	}
}

void NumberVariant::refreshClassfication(void)
{
	m_classfication = Type2Classfication.find(m_type)->second;
}

const bool operator<(const NumberVariant & lhs, const NumberVariant & rhs)
{
	return lhs.m_value < rhs.m_value;
}

const bool operator<=(const NumberVariant & lhs, const NumberVariant & rhs)
{
	return lhs.m_value <= rhs.m_value;
}

const bool operator>(const NumberVariant & lhs, const NumberVariant & rhs)
{
	return lhs.m_value > rhs.m_value;
}

const bool operator>=(const NumberVariant & lhs, const NumberVariant & rhs)
{
	return lhs.m_value >= rhs.m_value;
}

const bool operator==(const NumberVariant & lhs, const NumberVariant & rhs)
{
	return lhs.m_value == rhs.m_value;
}

const int operator!=(const NumberVariant & lhs, const NumberVariant & rhs)
{
	return lhs.m_value != rhs.m_value;
}

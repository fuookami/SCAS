#pragma once

#include <vector>
#include <array>
#include <string>
#include <memory>
#include <algorithm>

namespace DataUtils
{
	using byte = unsigned char;
	using Data = std::vector<byte>;

	using int8 = std::int8_t;
	using uint8 = std::uint8_t;
	using int16 = std::int16_t;
	using uint16 = std::uint16_t;
	using int32 = std::int32_t;
	using uint32 = std::uint32_t;
	using int64 = std::int64_t;
	using uint64 = std::uint64_t;

	template <typename T>
	inline const byte *getDataCBegin(const T &src)
	{
		return reinterpret_cast<const byte *>(std::addressof(src));
	}

	template <typename T>
	inline byte *getDataBegin(T &src)
	{
		return reinterpret_cast<byte *>(std::addressof(src));
	}

	template <typename T>
	inline const byte *getDataCEnd(const T &src)
	{
		static const uint8 DataLength(sizeof(T));

		return reinterpret_cast<const byte *>(std::addressof(src)) + DataLength;
	}

	template <typename T>
	inline byte *getDataEnd(T &src)
	{
		static const uint8 DataLength(sizeof(T));

		return reinterpret_cast<byte *>(std::addressof(src)) + DataLength;
	}

	class _DataUtils
	{
	public:
		~_DataUtils() {};

		inline static Data toData(const bool value) { return _toData(value); }
		inline static const bool fromData2Bool(const Data &data) { return _fromData<bool>(data); }

		inline static Data toData(const float value) { return _toData(value); }
		inline static const float fromData2Float(const Data &data) { return _fromData<float>(data); }

		inline static Data toData(const double value) { return _toData(value); }
		inline static const double fromData2Double(const Data &data) { return _fromData<double>(data); }

		inline static Data toData(const uint8 value) { return _toData(value); }
		inline static const int8 fromData2Int8(const Data &data) { return _fromData<int8>(data); }
		inline static const uint8 fromData2UInt8(const Data &data) { return _fromData<uint8>(data); }

		inline static Data toData(const uint16 value) { return _toData(value); }
		inline static const int16 fromData2Int16(const Data &data) { return _fromData<int16>(data); }
		inline static const uint16 fromData2UInt16(const Data &data) { return _fromData<uint16>(data); }

		inline static Data toData(const uint32 value) { return _toData(value); }
		inline static const int32 fromData2Int32(const Data &data) { return _fromData<int32>(data); }
		inline static const uint32 fromData2UInt32(const Data &data) { return _fromData<uint32>(data); }

		inline static Data toData(const uint64 value) { return _toData(value); }
		inline static const int64 fromData2Int64(const Data &data) { return _fromData<int64>(data); }
		inline static const uint64 fromData2UInt64(const Data &data) { return _fromData<uint64>(data); }

	private:
		template <typename T>
		static Data _toData(const T src)
		{
			Data buff;
			std::copy(getDataCBegin(src), getDataCEnd(src), std::back_inserter(buff));

			return buff;
		}

		template <typename T>
		static T _fromData(const Data &data)
		{
			static const uint8 DataLength(sizeof(T));

			if (data.size() != DataLength)
			{
				return T();
			}
			else
			{
				T ret;
				std::copy(data.cbegin(), data.cend(), getDataBegin(ret));

				return ret;
			}
		}

	private:
		_DataUtils() {};
	};

	inline const bool toBool(const Data &data) { return DataUtils::_DataUtils::fromData2Bool(data); }
	inline Data fromBool(const bool value) { return DataUtils::_DataUtils::toData(value); }

	inline const float toFloat(const Data &data) { return DataUtils::_DataUtils::fromData2Float(data); }
	inline Data fromFloat(const float value) { return DataUtils::_DataUtils::toData(value); }

	inline const double toDouble(const Data &data) { return DataUtils::_DataUtils::fromData2Double(data); }
	inline Data fromDouble(const double value) { return DataUtils::_DataUtils::toData(value); }

	inline const int8 toInt8(const Data &data) { return DataUtils::_DataUtils::fromData2Int8(data); }
	inline Data fromInt8(const int8 value) { return DataUtils::_DataUtils::toData(static_cast<uint8>(value)); }
	inline const uint8 toUInt8(const Data &data) { return DataUtils::_DataUtils::fromData2UInt8(data); }
	inline Data fromUInt8(const uint8 value) { return DataUtils::_DataUtils::toData(value); }

	inline const int16 toInt16(const Data &data) { return DataUtils::_DataUtils::fromData2Int16(data); }
	inline Data fromInt16(const int16 value) { return DataUtils::_DataUtils::toData(static_cast<uint16>(value)); }
	inline const uint16 toUInt16(const Data &data) { return DataUtils::_DataUtils::fromData2UInt16(data); }
	inline Data fromUInt16(const uint16 value) { return DataUtils::_DataUtils::toData(value); }

	inline const int32 toInt32(const Data &data) { return DataUtils::_DataUtils::fromData2Int32(data); }
	inline Data fromInt32(const int32 value) { return DataUtils::_DataUtils::toData(static_cast<uint32>(value)); }
	inline const uint32 toUInt32(const Data &data) { return DataUtils::_DataUtils::fromData2UInt32(data); }
	inline Data fromUInt32(const uint32 value) { return DataUtils::_DataUtils::toData(value); }

	inline const int64 toInt64(const Data &data) { return DataUtils::_DataUtils::fromData2Int64(data); }
	inline Data fromInt64(const int64 value) { return DataUtils::_DataUtils::toData(static_cast<uint64>(value)); }
	inline const uint64 toUInt64(const Data &data) { return DataUtils::_DataUtils::fromData2UInt64(data); }
	inline Data fromUInt64(const uint64 value) { return DataUtils::_DataUtils::toData(value); }

	template<int size>
	inline Data fromArray(const std::array<byte, size> &data) { return Data(data.cbegin(), data.cend()); }

	std::string toHexString(const Data &data, const std::string seperator = "");
	Data fromHexString(const std::string &str, const std::string seperator = "");

	std::string toBase64String(const Data &data, const char fillCharacter = '=');
	Data fromBase64String(const std::string &str);

	std::string toString(const Data &data);
	Data fromString(const std::string &str);
};

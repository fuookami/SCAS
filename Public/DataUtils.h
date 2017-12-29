#pragma once

#include <vector>
#include <string>

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

	class __DataUtils
	{
	public:
		~__DataUtils() {};

		inline static Data toData(const bool value) { return __toData(value); }
		inline static bool fromData2Bool(const Data &data) { return __fromData<bool>(data); }

		inline static Data toData(const float value) { return __toData(value); }
		inline static float fromData2Float(const Data &data) { return __fromData<float>(data); }

		inline static Data toData(const double value) { return __toData(value); }
		inline static double fromData2Double(const Data &data) { return __fromData<double>(data); }

		inline static Data toData(const uint8 value) { return __toData(value); }
		inline static int8 fromData2Int8(const Data &data) { return __fromData<int8>(data); }
		inline static uint8 fromData2UInt8(const Data &data) { return __fromData<uint8>(data); }

		inline static Data toData(const uint16 value) { return __toData(value); }
		inline static int16 fromData2Int16(const Data &data) { return __fromData<int16>(data); }
		inline static uint16 fromData2UInt16(const Data &data) { return __fromData<uint16>(data); }

		inline static Data toData(const uint32 value) { return __toData(value); }
		inline static int32 fromData2Int32(const Data &data) { return __fromData<int32>(data); }
		inline static uint32 fromData2UInt32(const Data &data) { return __fromData<uint32>(data); }

		inline static Data toData(const uint64 value) { return __toData(value); }
		inline static int64 fromData2Int64(const Data &data) { return __fromData<int64>(data); }
		inline static uint64 fromData2UInt64(const Data &data) { return __fromData<uint64>(data); }

	private:
		template <typename T>
		static const Data __toData(const T src)
		{
			static const uint8 DataLength(sizeof(T));

			Data buff(DataLength);
			memcpy(reinterpret_cast<void *>(buff.data()), reinterpret_cast<const void *>(&src), DataLength);

			return buff;
		}

		template <typename T>
		static const T __fromData(const Data &data)
		{
			static const uint8 DataLength(sizeof(T));

			if (data.size() != DataLength)
			{
				return T();
			}
			else
			{
				T ret;
				memcpy(reinterpret_cast<void *>(&ret), reinterpret_cast<const void *>(data.data()), DataLength);

				return ret;
			}
		}

	private:
		__DataUtils() {};
	};

	inline const bool toBool(const Data &data) { return DataUtils::__DataUtils::fromData2Bool(data); }
	inline const Data fromBool(const bool value) { return DataUtils::__DataUtils::toData(value); }

	inline const float toFloat(const Data &data) { return DataUtils::__DataUtils::fromData2Float(data); }
	inline const Data fromFloat(const float value) { return DataUtils::__DataUtils::toData(value); }

	inline const double toDouble(const Data &data) { return DataUtils::__DataUtils::fromData2Double(data); }
	inline const Data fromDouble(const double value) { return DataUtils::__DataUtils::toData(value); }

	inline const int8 toInt8(const Data &data) { return DataUtils::__DataUtils::fromData2Int8(data); }
	inline const Data fromInt8(const int8 value) { return DataUtils::__DataUtils::toData(static_cast<uint8>(value)); }
	inline const uint8 toUInt8(const Data &data) { return DataUtils::__DataUtils::fromData2UInt8(data); }
	inline const Data fromUInt8(const uint8 value) { return DataUtils::__DataUtils::toData(value); }

	inline const int16 toInt16(const Data &data) { return DataUtils::__DataUtils::fromData2Int16(data); }
	inline const Data fromInt16(const int16 value) { return DataUtils::__DataUtils::toData(static_cast<uint16>(value)); }
	inline const uint16 toUInt16(const Data &data) { return DataUtils::__DataUtils::fromData2UInt16(data); }
	inline const Data fromUInt16(const uint16 value) { return DataUtils::__DataUtils::toData(value); }

	inline const int32 toInt32(const Data &data) { return DataUtils::__DataUtils::fromData2Int32(data); }
	inline const Data fromInt32(const int32 value) { return DataUtils::__DataUtils::toData(static_cast<uint32>(value)); }
	inline const uint32 toUInt32(const Data &data) { return DataUtils::__DataUtils::fromData2UInt32(data); }
	inline const Data fromUInt32(const uint32 value) { return DataUtils::__DataUtils::toData(value); }

	inline const int64 toInt64(const Data &data) { return DataUtils::__DataUtils::fromData2Int64(data); }
	inline const Data fromInt64(const int64 value) { return DataUtils::__DataUtils::toData(static_cast<uint64>(value)); }
	inline const uint64 toUInt64(const Data &data) { return DataUtils::__DataUtils::fromData2UInt64(data); }
	inline const Data fromUInt64(const uint64 value) { return DataUtils::__DataUtils::toData(value); }

	const std::string toHexString(const Data &data, const std::string seperator = "");
	const Data fromHexString(const std::string &str, const std::string seperator = "");

	const std::string toBase64String(const Data &data, const char fillCharacter = '=');
	const Data fromBase64String(const std::string &str);

	const std::string toString(const Data &data);
	const Data fromString(const std::string &str);
};

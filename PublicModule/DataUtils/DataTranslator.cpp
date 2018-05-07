#include "DataTranslator.h"

namespace SSUtils
{
	namespace Data
	{
		const bool toBool(const Block &data, const Endian endian)
		{
			static DataTranslator<bool> translator(endian);
			return translator.toData(data);
		}
		Block fromBool(const bool value, const Endian endian)
		{
			static DataTranslator<bool> translator(endian);
			return translator.fromData(value);
		}

		const float toFloat(const Block &data, const Endian endian)
		{
			static DataTranslator<float> translator(endian);
			return translator.toData(data);
		}
		Block fromFloat(const float value, const Endian endian)
		{
			static DataTranslator<float> translator(endian);
			return translator.fromData(value);
		}

		const double toDouble(const Block &data, const Endian endian)
		{
			static DataTranslator<double> translator(endian);
			return translator.toData(data);
		}
		Block fromDouble(const double value, const Endian endian)
		{
			static DataTranslator<double> translator(endian);
			return translator.fromData(value);
		}

		const int8 toInt8(const Block &data, const Endian endian)
		{
			static DataTranslator<int8> translator(endian);
			return translator.toData(data);
		}
		Block fromInt8(const int8 value, const Endian endian)
		{
			static DataTranslator<int8> translator(endian);
			return translator.fromData(value);
		}
		const uint8 toUInt8(const Block &data, const Endian endian)
		{
			static DataTranslator<uint8> translator(endian);
			return translator.toData(data);
		}
		Block fromUInt8(const uint8 value, const Endian endian)
		{
			static DataTranslator<uint8> translator(endian);
			return translator.fromData(value);
		}

		const int16 toInt16(const Block &data, const Endian endian)
		{
			static DataTranslator<int16> translator(endian);
			return translator.toData(data);
		}
		Block fromInt16(const int16 value, const Endian endian)
		{
			static DataTranslator<int16> translator(endian);
			return translator.fromData(value);
		}
		const uint16 toUInt16(const Block &data, const Endian endian)
		{
			static DataTranslator<uint16> translator(endian);
			return translator.toData(data);
		}
		Block fromUInt16(const uint16 value, const Endian endian)
		{
			static DataTranslator<uint16> translator(endian);
			return translator.fromData(value);
		}

		const int32 toInt32(const Block &data, const Endian endian)
		{
			static DataTranslator<int32> translator(endian);
			return translator.toData(data);
		}
		Block fromInt32(const int32 value, const Endian endian)
		{
			static DataTranslator<int32> translator(endian);
			return translator.fromData(value);
		}
		const uint32 toUInt32(const Block &data, const Endian endian)
		{
			static DataTranslator<uint32> translator(endian);
			return translator.toData(data);
		}
		Block fromUInt32(const uint32 value, const Endian endian)
		{
			static DataTranslator<uint32> translator(endian);
			return translator.fromData(value);
		}

		const int64 toInt64(const Block &data, const Endian endian)
		{
			static DataTranslator<int64> translator(endian);
			return translator.toData(data);
		}
		Block fromInt64(const int64 value, const Endian endian)
		{
			static DataTranslator<int64> translator(endian);
			return translator.fromData(value);
		}
		const uint64 toUInt64(const Block &data, const Endian endian)
		{
			static DataTranslator<uint64> translator(endian);
			return translator.toData(data);
		}
		Block fromUInt64(const uint64 value, const Endian endian)
		{
			static DataTranslator<uint64> translator(endian);
			return translator.fromData(value);
		}
	}
};

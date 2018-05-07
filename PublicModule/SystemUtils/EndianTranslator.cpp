#include "EndianTranslator.h"

namespace SSUtils
{
	namespace System
	{
		const uint16 toLocalEndian(const Endian srcEndian, const uint16 src)
		{
			static EndianTranslator<uint16> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const uint16 fromLocalEndian(const Endian targetEndian, const uint16 src)
		{
			static EndianTranslator<uint16> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}
		const int16 toLocalEndian(const Endian srcEndian, const int16 src)
		{
			static EndianTranslator<int16> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const int16 fromLocalEndian(const Endian targetEndian, const int16 src)
		{
			static EndianTranslator<int16> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}

		const uint32 toLocalEndian(const Endian srcEndian, const uint32 src)
		{
			static EndianTranslator<uint32> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const uint32 fromLocalEndian(const Endian targetEndian, const uint32 src)
		{
			static EndianTranslator<uint32> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}
		const int32 toLocalEndian(const Endian srcEndian, const int32 src)
		{
			static EndianTranslator<int32> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const int32 fromLocalEndian(const Endian targetEndian, const int32 src)
		{
			static EndianTranslator<int32> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}

		const uint64 toLocalEndian(const Endian srcEndian, const uint64 src)
		{
			static EndianTranslator<uint64> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const uint64 fromLocalEndian(const Endian targetEndian, const uint64 src)
		{
			static EndianTranslator<uint64> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}
		const int64 toLocalEndian(const Endian srcEndian, const int64 src)
		{
			static EndianTranslator<int64> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const int64 fromLocalEndian(const Endian targetEndian, const int64 src)
		{
			static EndianTranslator<int64> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}

		const float toLocalEndian(const Endian srcEndian, const float src)
		{
			static EndianTranslator<float> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const float fromLocalEndian(const Endian targetEndian, const float src)
		{
			static EndianTranslator<float> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}

		const double toLocalEndian(const Endian srcEndian, const double src)
		{
			static EndianTranslator<double> translator;
			return translator.toLocalEndian(srcEndian, src);
		}
		const double fromLocalEndian(const Endian targetEndian, const double src)
		{
			static EndianTranslator<double> translator;
			return translator.fromLocalEndian(targetEndian, src);
		}
	};
};

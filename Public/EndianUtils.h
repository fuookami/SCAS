#pragma once

#include <cstdint>
#include <array>
#include <algorithm>

namespace EndianUtils
{
	enum class Endian
	{
		BigEndian,
		LittleEndian,
		Unknown
	};
	
	using int8 = std::int8_t;
	using uint8 = std::uint8_t;
	using int16 = std::int16_t;
	using uint16 = std::uint16_t;
	using int32 = std::int32_t;
	using uint32 = std::uint32_t;
	using int64 = std::int64_t;
	using uint64 = std::uint64_t;

	class _EndianUtils
	{
	public:
		~_EndianUtils() {};

		inline static const uint16 toLocalEndian(const Endian srcEndian, const uint16 src) { return _toLocalEndian(srcEndian, src); }
		inline static const uint16 fromLocalEndian(const Endian targetEndian, const uint16 src) { return _fromLocalEndian(targetEndian, src); }

		inline static const uint32 toLocalEndian(const Endian srcEndian, const uint32 src) { return _toLocalEndian(srcEndian, src); }
		inline static const uint32 fromLocalEndian(const Endian targetEndian, const uint32 src) { return _fromLocalEndian(targetEndian, src); }

		inline static const uint64 toLocalEndian(const Endian srcEndian, const uint64 src) { return _toLocalEndian(srcEndian, src); }
		inline static const uint64 fromLocalEndian(const Endian targetEndian, const uint64 src) { return _fromLocalEndian(targetEndian, src); }

		inline static const float toLocalEndian(const Endian srcEndian, const float src) { return _toLocalEndian(srcEndian, src); }
		inline static const float fromLocalEndian(const Endian targetEndian, const float src) { return _fromLocalEndian(targetEndian, src); }

		inline static const double toLocalEndian(const Endian srcEndian, const double src) { return _toLocalEndian(srcEndian, src); }
		inline static const double fromLocalEndian(const Endian targetEndian, const double src) { return _fromLocalEndian(targetEndian, src); }

	private:
		template <class T>
		inline static const T _toLocalEndian(const Endian srcEndian, const T src)
		{
			if (getLocalEndian() == srcEndian)
			{
				return src;
			}
			else
			{
				return _translateEndian(src);
			}
		}

		template <class T>
		inline static const T _fromLocalEndian(const Endian targetEndian, const T src)
		{
			if (getLocalEndian() == targetEndian)
			{
				return src;
			}
			else
			{
				return _translateEndian(src);
			}
		}

		template <class T>
		static const T _translateEndian(const T src)
		{
			static const uint8 DataLength = sizeof(T);

			std::array<int8, DataLength> buff;
			std::copy(reinterpret_cast<const int8 *>(&src), reinterpret_cast<const int8 *>(&src) + DataLength, buff.begin());
			std::reverse(buff.begin(), buff.end());

			T ret;
			std::copy(buff.cbegin(), buff.cend(), reinterpret_cast<int8 *>(&ret));
			return ret;
		}

	private:
		_EndianUtils() {};
	};

	Endian getLocalEndian(void);

	inline const uint16 toLocalEndian(const Endian srcEndian, const uint16 src) { return _EndianUtils::toLocalEndian(srcEndian, src); }
	inline const uint16 fromLocalEndian(const Endian targetEndian, const uint16 src) { return _EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const int16 toLocalEndian(const Endian srcEndian, const int16 src) { return _EndianUtils::toLocalEndian(srcEndian, static_cast<uint16>(src)); }
	inline const int16 fromLocalEndian(const Endian targetEndian, const int16 src) { return _EndianUtils::fromLocalEndian(targetEndian, static_cast<uint16>(src)); }

	inline const uint32 toLocalEndian(const Endian srcEndian, const uint32 src) { return _EndianUtils::toLocalEndian(srcEndian, src); }
	inline const uint32 fromLocalEndian(const Endian targetEndian, const uint32 src) { return _EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const int32 toLocalEndian(const Endian srcEndian, const int32 src) { return _EndianUtils::toLocalEndian(srcEndian, static_cast<uint32>(src)); }
	inline const int32 fromLocalEndian(const Endian targetEndian, const int32 src) { return _EndianUtils::fromLocalEndian(targetEndian, static_cast<uint32>(src)); }

	inline const uint64 toLocalEndian(const Endian srcEndian, const uint64 src) { return _EndianUtils::toLocalEndian(srcEndian, src); }
	inline const uint64 fromLocalEndian(const Endian targetEndian, const uint64 src) { return _EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const int64 toLocalEndian(const Endian srcEndian, const int64 src) { return _EndianUtils::toLocalEndian(srcEndian, static_cast<uint64>(src)); }
	inline const int64 fromLocalEndian(const Endian targetEndian, const int64 src) { return _EndianUtils::fromLocalEndian(targetEndian, static_cast<uint64>(src)); }

	inline const float toLocalEndian(const Endian srcEndian, const float src) { return _EndianUtils::toLocalEndian(srcEndian, src); }
	inline const float fromLocalEndian(const Endian targetEndian, const float src) { return _EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const double toLocalEndian(const Endian srcEndian, const double src) { return _EndianUtils::toLocalEndian(srcEndian, src); }
	inline const double fromLocalEndian(const Endian targetEndian, const double src) { return _EndianUtils::fromLocalEndian(targetEndian, src); }
};

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

	class __EndianUtils
	{
	public:
		~__EndianUtils() {};

		inline static const uint16 toLocalEndian(const Endian srcEndian, const uint16 src) { return __toLocalEndian(srcEndian, src); }
		inline static const uint16 fromLocalEndian(const Endian targetEndian, const uint16 src) { return __fromLocalEndian(targetEndian, src); }

		inline static const uint32 toLocalEndian(const Endian srcEndian, const uint32 src) { return __toLocalEndian(srcEndian, src); }
		inline static const uint32 fromLocalEndian(const Endian targetEndian, const uint32 src) { return __fromLocalEndian(targetEndian, src); }

		inline static const uint64 toLocalEndian(const Endian srcEndian, const uint64 src) { return __toLocalEndian(srcEndian, src); }
		inline static const uint64 fromLocalEndian(const Endian targetEndian, const uint64 src) { return __fromLocalEndian(targetEndian, src); }

		inline static const float toLocalEndian(const Endian srcEndian, const float src) { return __toLocalEndian(srcEndian, src); }
		inline static const float fromLocalEndian(const Endian targetEndian, const float src) { return __fromLocalEndian(targetEndian, src); }

		inline static const double toLocalEndian(const Endian srcEndian, const double src) { return __toLocalEndian(srcEndian, src); }
		inline static const double fromLocalEndian(const Endian targetEndian, const double src) { return __fromLocalEndian(targetEndian, src); }

	private:
		template <class T>
		inline static const T __toLocalEndian(const Endian srcEndian, const T src)
		{
			if (getLocalEndian() == srcEndian)
			{
				return src;
			}
			else
			{
				return __translateEndian(src);
			}
		}

		template <class T>
		inline static const T __fromLocalEndian(const Endian targetEndian, const T src)
		{
			if (getLocalEndian() == targetEndian)
			{
				return src;
			}
			else
			{
				return __translateEndian(src);
			}
		}

		template <class T>
		static const T __translateEndian(const T src)
		{
			static const uint8 DataLength = sizeof(T);

			std::array<int8, DataLength> buff;
			memcpy(reinterpret_cast<void *>(buff.data()), reinterpret_cast<const void *>(&src), DataLength);
			std::reverse(buff.begin(), buff.end());

			T ret;
			memcpy(reinterpret_cast<void *>(&ret), reinterpret_cast<const void *>(buff.data()), DataLength);
			return ret;
		}

	private:
		__EndianUtils() {};
	};

	Endian getLocalEndian(void);

	inline const uint16 toLocalEndian(const Endian srcEndian, const uint16 src) { return __EndianUtils::toLocalEndian(srcEndian, src); }
	inline const uint16 fromLocalEndian(const Endian targetEndian, const uint16 src) { return __EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const int16 toLocalEndian(const Endian srcEndian, const int16 src) { return __EndianUtils::toLocalEndian(srcEndian, static_cast<uint16>(src)); }
	inline const int16 fromLocalEndian(const Endian targetEndian, const int16 src) { return __EndianUtils::fromLocalEndian(targetEndian, static_cast<uint16>(src)); }

	inline const uint32 toLocalEndian(const Endian srcEndian, const uint32 src) { return __EndianUtils::toLocalEndian(srcEndian, src); }
	inline const uint32 fromLocalEndian(const Endian targetEndian, const uint32 src) { return __EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const int32 toLocalEndian(const Endian srcEndian, const int32 src) { return __EndianUtils::toLocalEndian(srcEndian, static_cast<uint32>(src)); }
	inline const int32 fromLocalEndian(const Endian targetEndian, const int32 src) { return __EndianUtils::fromLocalEndian(targetEndian, static_cast<uint32>(src)); }

	inline const uint64 toLocalEndian(const Endian srcEndian, const uint64 src) { return __EndianUtils::toLocalEndian(srcEndian, src); }
	inline const uint64 fromLocalEndian(const Endian targetEndian, const uint64 src) { return __EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const int64 toLocalEndian(const Endian srcEndian, const int64 src) { return __EndianUtils::toLocalEndian(srcEndian, static_cast<uint64>(src)); }
	inline const int64 fromLocalEndian(const Endian targetEndian, const int64 src) { return __EndianUtils::fromLocalEndian(targetEndian, static_cast<uint64>(src)); }

	inline const float toLocalEndian(const Endian srcEndian, const float src) { return __EndianUtils::toLocalEndian(srcEndian, src); }
	inline const float fromLocalEndian(const Endian targetEndian, const float src) { return __EndianUtils::fromLocalEndian(targetEndian, src); }
	inline const double toLocalEndian(const Endian srcEndian, const double src) { return __EndianUtils::toLocalEndian(srcEndian, src); }
	inline const double fromLocalEndian(const Endian targetEndian, const double src) { return __EndianUtils::fromLocalEndian(targetEndian, src); }
};

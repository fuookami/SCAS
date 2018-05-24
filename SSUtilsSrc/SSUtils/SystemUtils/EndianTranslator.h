#pragma once

#include "_pri_system_global.h"
#include "..\DataUtils\_pri_data.h"

#include <array>
#include <algorithm>

namespace SSUtils
{
	namespace System
	{
		template<typename T, typename = std::enable_if_t<sizeof(T) != 1>>
		class EndianTranslator
		{
		public:
			static const uint8 DataLength = sizeof(T);

			EndianTranslator(void) = default;
			EndianTranslator(const EndianTranslator &ano) = delete;
			EndianTranslator(EndianTranslator &&ano) = delete;
			EndianTranslator &operator=(const EndianTranslator &rhs) = delete;
			EndianTranslator &operator=(EndianTranslator &&rhs) = delete;
			~EndianTranslator(void) = default;

			static T toLocalEndian(const Endian srcEndian, const T src)
			{
				return LocalEndian() == srcEndian ? src : translateEndian(src);
			}

			static T fromLocalEndian(const Endian targetEndian, const T src)
			{
				return LocalEndian() == targetEndian ? src : translateEndian(src);
			}

			static T toLocalEndian_ref(const Endian srcEndian, const T &src)
			{
				return LocalEndian() == srcEndian ? src : translateEndian_ref(src);
			}

			static T fromLocalEndian_ref(const Endian targetEndian, const T &src)
			{
				return LocalEndian() == targetEndian ? src : translateEndian_ref(src);
			}

		private:
			static T translateEndian(const T src)
			{
				if (DataLength <= 1)
				{
					return src;
				}
				else
				{
					std::array<int8, DataLength> buff;
					std::copy(Data::getDataCBegin(src), Data::getDataCEnd(src), buff.begin());
					std::reverse(buff.begin(), buff.end());

					T ret;
					std::copy(buff.cbegin(), buff.cend(), Data::getDataBegin(ret));
					return ret;
				}
			}

			static T translateEndian_ref(const T &src)
			{
				if (DataLength <= 1)
				{
					return src;
				}
				else
				{
					std::array<int8, DataLength> buff;
					std::copy(Data::getDataCBegin(src), Data::getDataCEnd(src), buff.begin());
					std::reverse(buff.begin(), buff.end());

					T ret;
					std::copy(buff.cbegin(), buff.cend(), Data::getDataBegin(ret));
					return ret;
				}
			}
		};

		template<typename T>
		const T toLocalEndian(const Endian srcEndian, const T &src)
		{
			static EndianTranslator<T> translator;
			return translator.toLocalEndian_ref(srcEndian, src);
		}
		template<typename T>
		const T fromLocalEndian(const Endian targetEndian, const T &src)
		{
			static EndianTranslator<T> translator;
			return translator.fromLocalEndian_ref(targetEndian, src);
		}

		SSUtils_API_DECLSPEC const uint16 toLocalEndian(const Endian srcEndian, const uint16 src);
		SSUtils_API_DECLSPEC const uint16 fromLocalEndian(const Endian targetEndian, const uint16 src);
		SSUtils_API_DECLSPEC const int16 toLocalEndian(const Endian srcEndian, const int16 src);
		SSUtils_API_DECLSPEC const int16 fromLocalEndian(const Endian targetEndian, const int16 src);

		SSUtils_API_DECLSPEC const uint32 toLocalEndian(const Endian srcEndian, const uint32 src);
		SSUtils_API_DECLSPEC const uint32 fromLocalEndian(const Endian targetEndian, const uint32 src);
		SSUtils_API_DECLSPEC const int32 toLocalEndian(const Endian srcEndian, const int32 src);
		SSUtils_API_DECLSPEC const int32 fromLocalEndian(const Endian targetEndian, const int32 src);

		SSUtils_API_DECLSPEC const uint64 toLocalEndian(const Endian srcEndian, const uint64 src);
		SSUtils_API_DECLSPEC const uint64 fromLocalEndian(const Endian targetEndian, const uint64 src);
		SSUtils_API_DECLSPEC const int64 toLocalEndian(const Endian srcEndian, const int64 src);
		SSUtils_API_DECLSPEC const int64 fromLocalEndian(const Endian targetEndian, const int64 src);

		SSUtils_API_DECLSPEC const float toLocalEndian(const Endian srcEndian, const float src);
		SSUtils_API_DECLSPEC const float fromLocalEndian(const Endian targetEndian, const float src);
		SSUtils_API_DECLSPEC const double toLocalEndian(const Endian srcEndian, const double src);
		SSUtils_API_DECLSPEC const double fromLocalEndian(const Endian targetEndian, const double src);
	};
};

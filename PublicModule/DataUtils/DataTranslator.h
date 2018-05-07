#pragma once

#include "_pri_data.h"
#include "SystemUtils.h"
#include <array>
#include <algorithm>

namespace SSUtils
{
	namespace Data
	{
		template <typename T>
		struct DataTranslator
		{
			static const uint8 DataLength = sizeof(T);
			Endian endian;

			DataTranslator(const Endian _endian = System::LocalEndian)
				: endian(_endian) {}
			DataTranslator(const DataTranslator &ano) = delete;
			DataTranslator(DataTranslator &&ano) = delete;
			DataTranslator &operator=(const DataTranslator &rhs) = delete;
			DataTranslator &operator=(DataTranslator &&rhs) = delete;
			~DataTranslator(void) = default;

			Block fromData(const T &src) const
			{
				Block buff(DataLength, 0);
				if (DataLength == 1 || endian == Endian::BigEndian)
				{
					std::copy(getDataCBegin(src), getDataCEnd(src), buff.begin());
				}
				else
				{
					std::copy(getDataCBegin(src), getDataCEnd(src), buff.begin());
					std::reverse(buff.begin(), buff.end());
				}

				return buff;
			}

			template <typename container, typename = std::enable_if_t<std::is_same_v<typename container::value_type, T>>>
			Block fromDataContainer(const container &datas) const
			{
				Block buff;
				if (DataLength == 1 || endian == Endian::BigEndian)
				{
					for (const auto &data : datas)
					{
						std::copy(getDataCBegin(data), getDataCEnd(data), std::back_inserter(buff));
					}
				}
				else
				{
					for (const auto &data : datas)
					{
						std::copy(getDataCBegin(data), getDataCEnd(data), std::back_inserter(buff));
						std::reverse(buff.end() - DataLength, buff.end());
					}
				}

				return buff;
			}

			template <typename iter, typename = std::enable_if_t<std::is_same_v<typename iter::value_type, T>>>
			Block fromDataIterator(const iter bgIt, const iter edIt) const
			{
				Block buff;
				if (DataLength == 1 || endian == Endian::BigEndian)
				{
					for (iter curr(bgIt); curr != edIt; ++curr)
					{
						const T &data(*curr);
						std::copy(getDataCBegin(data), getDataCEnd(data), std::back_inserter(buff));
					}
				}
				else
				{
					for (iter curr(bgIt); curr != edIt; ++curr)
					{
						const T &data(*curr);
						std::copy(getDataCBegin(data), getDataCEnd(data), std::back_inserter(buff));
						std::reverse(buff.end() - DataLength, buff.end());
					}
				}


				return buff;
			}

			T toData(const Block &data) const
			{
				if (data.size() != DataLength)
				{
					return T();
				}
				else
				{
					T ret;
					if (DataLength == 1 || endian == Endian::BigEndian)
					{
						std::copy(data.cbegin(), data.cend(), getDataBegin(ret));
					}
					else
					{
						std::copy(data.crbegin(), data.crend(), getDataBegin(ret));
					}

					return ret;
				}
			}

			template<uint32 size>
			std::array<T, size> toDataArray(const Block &datas) const
			{
				if (data.size() != (size * DataLength))
				{
					return std::array<T, size>();
				}
				else
				{
					std::array<T, size> ret;
					if (DataLength == 1 || endian == Endian::BigEndian)
					{
						for (uint32 i(0), j(datas.size()); i != j; i += DataLength)
						{
							std::copy(datas.cbegin() + i, datas.cbegin() + i + DataLength, getDataBegin(ret[i / DataLength]));
						}
					}
					else
					{
						for (uint32 i(0), j(datas.size()); i != j; i += DataLength)
						{
							Block data(DataLength, 0);
							std::reverse_copy(datas.cbegin() + i, datas.cbegin() + i + DataLength, data.begin());
							std::copy(data.cbegin(), data.cend(), getDataBegin(ret[i / DataLength]));
						}
					}

					return ret;
				}
			}

			template<typename container, typename = std::enable_if_t<std::is_same_v<typename container::value_type, T>>>
			container toDataContainer(const Block &datas) const
			{
				if (datas.size() % DataLength != 0)
				{
					return container();
				}
				else
				{
					container ret;
					if (DataLength == 1 || endian == Endian::BigEndian)
					{
						for (uint32 i(0), j(static_cast<uint32>(datas.size())); i != j; i += DataLength)
						{
							T temp;
							std::copy(datas.cbegin() + i, datas.cbegin() + i + DataLength, getDataBegin(temp));

							ret.insert(ret.end(), std::move(temp));
						}
					}
					else
					{
						for (uint32 i(0), j(static_cast<uint32>(datas.size())); i != j; i += DataLength)
						{
							T temp;
							Block data(DataLength, 0);
							std::reverse_copy(datas.cbegin() + i, datas.cbegin() + i + DataLength, data.begin());
							std::copy(data.cbegin(), data.cend(), getDataBegin(temp));

							ret.insert(ret.end(), std::move(temp));
						}
					}

					return ret;
				}
			}

			template<typename outIt, typename = std::enable_if_t<std::is_same_v<typename outIt::value_type, T>>>
			outIt toDataIterator(const Block &data, outIt it) const
			{
				if (data.size() % DataLength != 0)
				{
					return std::vector<T>();
				}
				else
				{
					if (DataLength == 1 || endian == Endian::BigEndian)
					{
						for (uint32 i(0), j(datas.size()); i != j; i += DataLength)
						{
							T temp;
							std::copy(datas.cbegin() + i, datas.cbegin() + i + DataLength, getDataBegin(temp));

							it = std::move(temp);
							++it;
						}
					}
					else
					{
						for (uint32 i(0), j(datas.size()); i != j; i += DataLength)
						{
							T temp;
							Block data(DataLength, 0);
							std::reverse_copy(datas.cbegin() + i, datas.cbegin() + i + DataLength, data.begin());
							std::copy(data.cbegin(), data.end(), getDataBegin(temp));

							it = std::move(temp);
							++it;
						}
					}

					return it;
				}
			}
		};

		const bool toBool(const Block &data, const Endian endian = System::LocalEndian);
		Block fromBool(const bool value, const Endian endian = System::LocalEndian);

		const float toFloat(const Block &data, const Endian endian = System::LocalEndian);
		Block fromFloat(const float value, const Endian endian = System::LocalEndian);
		const double toDouble(const Block &data, const Endian endian = System::LocalEndian);
		Block fromDouble(const double value, const Endian endian = System::LocalEndian);

		const int8 toInt8(const Block &data, const Endian endian = System::LocalEndian);
		Block fromInt8(const int8 value, const Endian endian = System::LocalEndian);
		const uint8 toUInt8(const Block &data, const Endian endian = System::LocalEndian);
		Block fromUInt8(const uint8 value, const Endian endian = System::LocalEndian);

		const int16 toInt16(const Block &data, const Endian endian = System::LocalEndian);
		Block fromInt16(const int16 value, const Endian endian = System::LocalEndian);
		const uint16 toUInt16(const Block &data, const Endian endian = System::LocalEndian);
		Block fromUInt16(const uint16 value, const Endian endian = System::LocalEndian);

		const int32 toInt32(const Block &data, const Endian endian = System::LocalEndian);
		Block fromInt32(const int32 value, const Endian endian = System::LocalEndian);
		const uint32 toUInt32(const Block &data, const Endian endian = System::LocalEndian);
		Block fromUInt32(const uint32 value, const Endian endian = System::LocalEndian);
		
		const int64 toInt64(const Block &data, const Endian endian = System::LocalEndian);
		Block fromInt64(const int64 value, const Endian endian = System::LocalEndian);
		const uint64 toUInt64(const Block &data, const Endian endian = System::LocalEndian);
		Block fromUInt64(const uint64 value, const Endian endian = System::LocalEndian);

		template<typename T>
		Block fromData(const T &data, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<T> translator(endian);
			return translator.fromData(data);
		}
		template<typename T>
		T toData(const Block &data, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<T> translator(endian);
			return translator.toData(data);
		}

		template<typename T, uint32 size>
		Block fromArray(const std::array<T, size> &datas, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<T> translator(endian);
			return translator.fromDataContainer(datas);
		}
		template<typename T, uint32 size>
		std::array<T, size> toArray(const Block &datas, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<T> translator(endian);
			return translator.toDataArray<size>(datas);
		}

		template<typename container>
		Block fromContainer(const container &datas, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<container::value_type> translator(endian);
			return translator.fromDataContainer(datas);
		}
		template<typename container>
		container toContainer(const Block &data, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<container::value_type> translaotr(endian);
			return translaotr.toDataContainer<container>(data);
		}

		template<typename iter>
		Block fromIterator(const iter bgIt, const iter edIt, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<iter::value_type> translator(endian);
			return translator.fromDataIterator(bgIt, edIt);
		}
		template<typename outIt>
		outIt toIterator(const Block &data, outIt it, const Endian endian = System::LocalEndian)
		{
			static DataTranslator<iter::value_type> translator(endian);
			return translator.toDataIterator(data, it);
		}
	};
};

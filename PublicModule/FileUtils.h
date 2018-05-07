#pragma once

#include "Global.h"
#include "DataUtils/DataTranslator.h"
#include "DataUtils.h"
#include <string>
#include <iterator>
#include <fstream>
#include <algorithm>
#include <type_traits>

namespace SSUtils
{
	namespace File
	{
		extern const std::string PathSeperator;
		extern const std::string ExtensionSeperator;
		extern const std::string InitailPath;

		const bool checkFileExist(const std::string &targetUrl);
		void createFile(const std::string &targetUrl);
		const bool removeFile(const std::string &targetUrl);

		std::string getPathOfUrl(const std::string &targetUrl);
		std::string getFileNameOfUrl(const std::string &targetUrl);
		std::string getFileMainNameOfUrl(const std::string &targetUrl);
		std::string getFileExtensionOfUrl(const std::string &targetUrl);
		std::vector<std::string> getAllFilesUrlsOfPath(const std::string &targetPath);
		std::vector<std::string> getAllDirectoryPathsOfPath(const std::string &targetPath);

		template<typename T, uint32 DataLength = sizeof(T)>
		struct FileLoader
		{
			FileLoader(void) = default;
			FileLoader(const FileLoader &ano) = delete;
			FileLoader(FileLoader &&ano) = delete;
			FileLoader &operator=(const FileLoader &rhs) = delete;
			FileLoader &operator=(FileLoader &&rhs) = delete;
			~FileLoader(void) = default;

			template<typename iter, typename = std::enable_if_t<std::is_same_v<typename iter::value_type, T>>>
			iter toIter(const std::string &targetUrl, iter it) const
			{
				static const FileLoader<byte> loader;
				static const Data::DataTranslator<T> translator;
				Block data(loader.load<Block>(targetUrl));
				return translator.toDataIterator<iter>(data, it);
			}
			template<typename container, typename = std::enable_if_t<std::is_same_v<typename container::value_type, T>>>
			container toContainer(const std::string &targetUrl) const
			{
				static const FileLoader<byte> loader;
				static const Data::DataTranslator<T> translator;
				Block data(loader.load<Block>(targetUrl));
				return translator.toDataContainer<container>(data);
			}
		};
		template<typename T>
		struct FileLoader<T, 1>
		{
			FileLoader(void) = default;
			FileLoader(const FileLoader &ano) = delete;
			FileLoader(FileLoader &&ano) = delete;
			FileLoader &operator=(const FileLoader &rhs) = delete;
			FileLoader &operator=(FileLoader &&rhs) = delete;
			~FileLoader(void) = default;

			template<typename iter, typename = std::enable_if_t<std::is_same_v<typename iter::value_type, T>>>
			iter toIter(const std::string &targetUrl, iter it) const
			{
				if (checkFileExist(targetUrl))
				{
					std::ifstream fin(targetUrl, std::ifstream::binary);
					std::istreambuf_iterator<T> bgIt(fin), edIt;
					std::copy(bgIt, edIt, it);
					fin.close();
					return it;
				}
				else
				{
					return it;
				}
			}
			template<typename container, typename = std::enable_if_t<std::is_same_v<typename container::value_type, T>>>
			container toContainer(const std::string &targetUrl) const
			{
				if (checkFileExist(targetUrl))
				{
					std::ifstream fin(targetUrl, std::ifstream::binary);
					std::istreambuf_iterator<char> bgIt(fin), edIt;
					container data(bgIt, edIt);
					fin.close();
					return data;
				}
				else
				{
					return container();
				}
			}
		};
		Block loadFile(const std::string & targetUrl);
		template<typename container>
		container loadFile(const std::string &targetUrl)
		{
			static const FileLoader<container::value_type> loader;
			return loader.toContainer<container>(targetUrl);
		}
		template<typename iter>
		iter loadFile(const std::string &targetUrl, iter it)
		{
			static const FileLoader<iter::value_type> loader;
			return loader.toIter(targetUrl, it);
		}

		std::vector<std::string> loadFileByLine(const std::string &targetUrl);

		template<typename T, uint32 DataLength = sizeof(T)>
		struct FileSaver
		{
			FileSaver(void) = default;
			FileSaver(const FileSaver &ano) = delete;
			FileSaver(FileSaver &&ano) = delete;
			FileSaver &operator=(const FileSaver &rhs) = delete;
			FileSaver &operator=(FileSaver &&rhs) = delete;
			~FileSaver(void) = default;

			template<typename iter, typename = std::enable_if_t<std::is_same_v<typename iter::value_type, T>>>
			void operator()(const std::string &targetUrl, const iter bgIt, const iter edIt) const
			{
				static const Data::DataTranslator<T> translator;
				static const FileSaver<byte> saver;
				Block buff(translator.fromDataIterator(bgIt, edIt));
				saver(targetUrl, buff.cbegin(), buff.cend());
			}
			template<typename container, typename = std::enable_if_t<std::is_same_v<typename container::value_type, T>>>
			void operator()(const std::string &targetUrl, const container &datas) const
			{
				operator()(targetUrl, datas.cbegin(), datas.cend());
			}
		};
		template<typename T>
		struct FileSaver<T, 1>
		{
			static_assert(std::is_convertible_v<T, byte>, "can not translate to byte");

			FileSaver(void) = default;
			FileSaver(const FileSaver &ano) = delete;
			FileSaver(FileSaver &&ano) = delete;
			FileSaver &operator=(const FileSaver &rhs) = delete;
			FileSaver &operator=(FileSaver &&rhs) = delete;
			~FileSaver(void) = default;

			template<typename iter, typename = std::enable_if_t<std::is_same_v<typename iter::value_type, T>>>
			void operator()(const std::string &targetUrl, const iter bgIt, const iter edIt) const
			{
				std::ofstream fout(targetUrl);
				std::ostream_iterator<byte> outIt(fout);
				std::copy(bgIt, edIt, outIt);
				fout.close();
			}
			template<typename container, typename = std::enable_if_t<std::is_same_v<typename container::value_type, T>>>
			void operator()(const std::string &targetUrl, const container &datas) const
			{
				std::ofstream fout(targetUrl);
				std::ostream_iterator<byte> outIt(fout);
				std::copy(datas.cbegin(), datas.cend(), outIt);
				fout.close();
			}
		};
		void saveFile(const std::string &targetUrl, const Block &fileData);
		void saveFile(const std::string &targetUrl, const std::string &fileData);
		template<typename iter>
		void saveFile(const std::string &targetUrl, const iter bgIt, const iter edIt)
		{
			static const FileSaver<iter::value_type> saver;
			saver(targetUrl, bgIt, edIt);
		}
		template<typename container>
		void saveFile(const std::string &targetUrl, const container &fileData)
		{
			static const FileSaver<container::value_type> saver;
			saver(targetUrl, fileData);
		}

		const bool checkPathExist(const std::string &targetPath);
		const bool insurePathExist(const std::string &targetPath);
		const bool removePath(const std::string &targetPath);
		std::string getSystemNativePath(const std::string &targetPath);
	};
};
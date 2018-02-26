#pragma once

#include <vector>
#include <string>
#include <iterator>
#include <fstream>
#include <algorithm>

#include "DataUtils.h"

namespace FileUtils
{
#ifdef _WIN32
	static const std::string PathSeperator("\\");
#else
	static const std::string PathSeperator("/");
#endif

	const std::string &initailPath();

	const bool checkFileExist(const std::string &targetUrl);
	void createFile(const std::string &targetUrl);
	const bool removeFile(const std::string &targetUrl);

	std::string getPathOfUrl(const std::string &targetUrl);
	std::string getFileNameOfUrl(const std::string &targetUrl);
	std::vector<std::string> getAllFilesNameOfPath(const std::string &targetPath);
	std::vector<std::string> getAllDirectoryPathsOfPath(const std::string &targetPath);
	
	std::vector<unsigned char> loadFile(const std::string &targetUrl);
	void saveFile(const std::string &targetUrl, const DataUtils::Data &fileData);
	void saveFile(const std::string &targetUrl, const std::string &fileData);
	template<typename iter>
	void _saveFile(const std::string &targetUrl, const iter bgIt, const iter edIt);

	const bool checkPathExist(const std::string &targetPath);
	const bool insurePathExist(const std::string &targetPath);
	const bool removePath(const std::string &targetPath);
	std::string getSystemNativePath(const std::string &targetPath);

	template<typename iter>
	void _saveFile(const std::string & targetUrl, const iter bgIt, const iter edIt)
	{
		std::ofstream fout(targetUrl);
		std::ostream_iterator<DataUtils::byte> outIt(fout);
		std::copy(bgIt, edIt, outIt);
		fout.close();
	}
};

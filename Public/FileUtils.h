#pragma once

#include <vector>
#include <string>

namespace FileUtils
{
	const std::string &initailPath();

	bool checkFileExist(const std::string &targetUrl);
	void createFile(const std::string &targetUrl);
	bool removeFile(const std::string &targetUrl);

	const std::string getPathOfUrl(const std::string &targetUrl);
	const std::vector<std::string> getAllFilesNameOfPath(const std::string &targetPath);
	const std::vector<std::string> getAllDirectoryPathsOfPath(const std::string &targetPath);
	
	std::vector<unsigned char> loadFile(const std::string &targetUrl);
	void saveFile(const std::string &targetUrl, const std::vector<unsigned char> &fileData);

	bool checkPathExist(const std::string &targetPath);
	bool insurePathExist(const std::string &targetPath);
	bool removePath(const std::string &targetPath);
	const std::string getSystemNativePath(const std::string &targetPath);
};

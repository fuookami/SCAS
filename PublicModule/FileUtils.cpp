#include "FileUtils.h"
#include "StringUtils.h"

#include <boost/filesystem/path.hpp>
#include <boost/filesystem/operations.hpp>
#include <boost/filesystem/convenience.hpp>
#include <sstream>
#include <fstream>
#include <algorithm>

namespace FileUtils
{
	const std::string & initailPath()
	{
		static const std::string initailPath(boost::filesystem::initial_path().string());
		return initailPath;
	}

	const bool checkFileExist(const std::string & targetUrl)
	{
		std::ifstream fin(targetUrl);
		if (!fin.is_open())
		{
			return false;
		}

		fin.close();
		return true;
	}

	void createFile(const std::string & targetUrl)
	{
		// 确保路径存在
		insurePathExist(getPathOfUrl(targetUrl));

		std::ofstream fout(targetUrl);
		fout.close();
	}

	const bool removeFile(const std::string & targetUrl)
	{
		try
		{
			return boost::filesystem::remove(targetUrl);
		}
		catch (std::exception e)
		{
			return false;
		}
	}

	std::string getPathOfUrl(const std::string & targetUrl)
	{
		if (targetUrl.find_last_of("\\/") == -1)
		{
			return StringUtils::EmptyString;
		}
		else
		{
			return std::string(targetUrl.cbegin(), targetUrl.cbegin() + targetUrl.find_last_of("\\/"));
		}
	}

	std::string getFileNameOfUrl(const std::string & targetUrl)
	{
		return std::string(targetUrl.cbegin() + targetUrl.find_last_of("\\/") + 1, targetUrl.cend());
	}

	std::vector<std::string> getAllFilesNameOfPath(const std::string & targetPath)
	{
		using namespace boost::filesystem;

		path fullPath(system_complete(path(targetPath, native)));
		std::vector<std::string> fileNames;
		if (!exists(fullPath))
		{
			insurePathExist(targetPath);
			return {};
		}
		else
		{
			for (directory_iterator bgIt(fullPath), endIt; bgIt != endIt; ++bgIt)
			{
				if (is_regular_file(bgIt->status()))
				{
					fileNames.push_back(bgIt->path().string());
				}
			}
		}
		return fileNames;
	}

	std::vector<std::string> getAllDirectoryPathsOfPath(const std::string & targetPath)
	{
		using namespace boost::filesystem;

		path fullPath(system_complete(path(targetPath, native)));
		std::vector<std::string> pathNames;
		if (!exists(fullPath))
		{
			return {};
		}
		else
		{
			for (directory_iterator bgIt(fullPath), endIt; bgIt != endIt; ++bgIt)
			{
				if (is_directory(bgIt->status()))
				{
					pathNames.push_back(bgIt->path().string());
				}
			}
		}
		return pathNames;
	}

	std::vector<unsigned char> loadFile(const std::string & targetUrl)
	{
		if (checkFileExist(targetUrl))
		{
			std::ifstream fin(targetUrl, std::ifstream::binary);
			std::istreambuf_iterator<char> bgIt(fin), edIt;
			std::vector<unsigned char> data(bgIt, edIt);
			fin.close();
			return data;
		}
		else
		{
			return {};
		}
	}

	void saveFile(const std::string & targetUrl, const DataUtils::Data& fileData)
	{
		insurePathExist(getPathOfUrl(targetUrl));

		_saveFile(targetUrl, fileData.cbegin(), fileData.cend());
	}

	void saveFile(const std::string & targetUrl, const std::string & fileData)
	{
		insurePathExist(getPathOfUrl(targetUrl));

		_saveFile(targetUrl, fileData.cbegin(), fileData.cend());
	}

	const bool checkPathExist(const std::string & targetPath)
	{
		using namespace boost::filesystem;

		path fullPath(system_complete(path(targetPath, native)));
		return exists(fullPath);
	}

	const bool insurePathExist(const std::string & targetPath)
	{
		using namespace boost::filesystem;

		if (!targetPath.empty())
		{
			path fullPath(system_complete(path(targetPath, native)));

			if (!exists(fullPath))
			{
				// 创建多层子目录
				bool ret = create_directories(fullPath);
				if (ret == false)
				{
					/*
					std::ostringstream sout;
					sout << "无法创建目录: " << fullPath << std::endl;
					throw std::runtime_error(sout.str().c_str());
					*/

					return false;
				}
			}
		}

		return true;
	}

	const bool removePath(const std::string & targetPath)
	{
		try
		{
			return boost::filesystem::remove_all(targetPath);
		}
		catch (std::exception e)
		{
			return false;
		}
	}

	std::string getSystemNativePath(const std::string & targetPath)
	{
		using namespace boost::filesystem;

		return system_complete(path(targetPath, native)).string();
	}
};

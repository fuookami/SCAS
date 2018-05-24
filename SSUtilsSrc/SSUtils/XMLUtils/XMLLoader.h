#pragma once

#include "_pri_xml_global.h"
#include "..\StringUtils.h"
#include <boost/property_tree/ptree.hpp>
#include <memory>

namespace SSUtils
{
	namespace XML
	{
		struct SSUtils_API_DECLSPEC Loader
		{
			boost::property_tree::ptree ori_pt;
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> pt;
			std::string url;
			CharType charType;

			Loader(const CharType _charType = String::LocalCharType());
			Loader(const std::string &_url, const CharType _charType = String::LocalCharType());
			Loader(std::string &&_url, const CharType _charType = String::LocalCharType());
			Loader(const boost::property_tree::ptree &_pt, const CharType _charType = String::LocalCharType());
			Loader(const Loader &ano) = default;
			Loader(Loader &&ano) = default;
			Loader &operator=(const Loader &rhs) = default;
			Loader &operator=(Loader &&rhs) = default;
			~Loader(void) = default;

			const bool isOpened(void) const;
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> open(void);
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> open(const std::string &_url);
			std::vector<std::shared_ptr<Node>> operator()(void);
			std::vector<std::shared_ptr<Node>> operator()(const boost::property_tree::ptree &_pt, const CharType _charType = String::LocalCharType());
		};

		struct SSUtils_API_DECLSPEC Scaner
		{
			boost::property_tree::ptree ori_pt;
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> pt;
			std::string data;
			CharType charType;

			Scaner(const CharType _charType = String::LocalCharType());
			Scaner(const std::string &_data, const CharType _charType = String::LocalCharType());
			Scaner(std::string &&_data, const CharType _charType = String::LocalCharType());
			Scaner(const boost::property_tree::ptree &_pt, const CharType _charType = String::LocalCharType());
			Scaner(const Scaner &ano) = default;
			Scaner(Scaner &&ano) = default;
			Scaner &operator=(const Scaner &rhs) = default;
			Scaner &operator=(Scaner &&ano) = default;
			~Scaner(void) = default;

			const bool isScaned(void) const;
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> scan(void);
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> scan(const std::string &_data);
			std::vector<std::shared_ptr<Node>> operator()();
			std::vector<std::shared_ptr<Node>> operator()(const boost::property_tree::ptree &_pt, const CharType _charType = String::LocalCharType());
		};
	};
};

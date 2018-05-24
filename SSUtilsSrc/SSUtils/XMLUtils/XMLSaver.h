#pragma once

#include "_pri_xml_global.h"
#include "..\StringUtils.h"
#include <boost/property_tree/ptree.hpp>
#include <memory>

namespace SSUtils
{
	namespace XML
	{
		struct SSUtils_API_DECLSPEC Saver
		{
			std::string url;
			std::shared_ptr<std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>> roots;
			std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> pt;
			CharType charType;

			Saver(const CharType _charType = String::LocalCharType());
			Saver(const std::string &_url, const std::vector<std::shared_ptr<Node>> &_roots, const CharType _charType = String::LocalCharType());
			Saver(std::string &&_url, const std::vector<std::shared_ptr<Node>> &_roots, const CharType _charType = String::LocalCharType());
			Saver(const std::string &_url, const boost::property_tree::ptree & _pt, const CharType _charType = String::LocalCharType());
			Saver(std::string &&_url, const boost::property_tree::ptree & _pt, const CharType _charType = String::LocalCharType());
			Saver(const std::vector<std::shared_ptr<Node>> &_roots, const CharType _charType = String::LocalCharType());
			Saver(const boost::property_tree::ptree & _pt, const CharType _charType = String::LocalCharType());
			Saver(const Saver &ano) = default;
			Saver(Saver &&ano) = default;
			Saver &operator=(const Saver &rhs) = default;
			Saver &operator=(Saver &&rhs) = default;
			~Saver(void) = default;

			std::string toString(void);
			std::string toString(const std::vector<std::shared_ptr<Node>> &_roots, const CharType _charType = String::LocalCharType());
			std::string toString(const boost::property_tree::ptree &_pt, const CharType _charType = String::LocalCharType());

			const bool toFile(void);
			const bool toFile(const std::string &_url, const std::vector<std::shared_ptr<Node>> &_roots, const CharType _charType = String::LocalCharType());
			const bool toFile(const std::string &_url, const boost::property_tree::ptree &_pt, const CharType _charType = String::LocalCharType());
		};
	}
};

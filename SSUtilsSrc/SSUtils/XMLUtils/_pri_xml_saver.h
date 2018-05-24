#pragma once

#include "_pri_xml_global.h"
#include "..\StringUtils.h"
#include <boost/property_tree/ptree.hpp>
#include <memory>

namespace SSUtils
{
	namespace XML
	{
		std::string saveToString(const boost::property_tree::ptree &ptree, const CharType charType = String::LocalCharType());
		std::string saveToString(const std::vector<std::shared_ptr<Node>> &roots, const CharType charType = String::LocalCharType());

		const bool saveToFile(const std::string &url, const boost::property_tree::ptree &ptree, const CharType charType = String::LocalCharType());
		const bool saveToFile(const std::string &url, const std::vector<std::shared_ptr<Node>> &roots, const CharType charType = String::LocalCharType());

		boost::property_tree::ptree::value_type saveToPTreeNode(const std::shared_ptr<Node> node);
		boost::property_tree::ptree saveToPTree(const std::shared_ptr<Node> node);
		boost::property_tree::ptree saveToPTree(const std::vector<std::shared_ptr<Node>> &nodes);
	};
};

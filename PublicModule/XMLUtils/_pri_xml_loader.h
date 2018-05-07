#pragma once

#include "_pri_xml_global.h"
#include "StringUtils.h"
#include <boost/property_tree/ptree.hpp>
#include <memory>

namespace SSUtils
{
	namespace XML
	{
		std::shared_ptr<Node> getNode(const boost::property_tree::ptree::value_type &root, const CharType charType = String::LocalCharType);
		std::shared_ptr<Node> getTree(const boost::property_tree::ptree::value_type &root, const CharType charType = String::LocalCharType);
		void getAttrs(std::shared_ptr<Node> node, const boost::property_tree::ptree::value_type &root, const CharType charType = String::LocalCharType);
		void getChildren(std::shared_ptr<Node> node, const boost::property_tree::ptree::value_type &root, const CharType charType = String::LocalCharType);
		void getContent(std::shared_ptr<Node> node, const boost::property_tree::ptree::value_type &root, const CharType charType = String::LocalCharType);
		
		const bool openXMLFile(boost::property_tree::ptree &pt, const std::string &url);
		const bool scanXMLString(boost::property_tree::ptree &pt, const std::string &data);
		std::vector<std::shared_ptr<Node>> loadXML(const std::string &url, const CharType charType = String::LocalCharType);
		std::vector<std::shared_ptr<Node>> loadXML(const boost::property_tree::ptree &pt, const CharType charType = String::LocalCharType);
	};
};

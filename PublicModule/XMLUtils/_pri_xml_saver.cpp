#include "_pri_xml_saver.h"
#include "XMLNode.h"
#include "FileUtils.h"
#include <boost/property_tree/xml_parser.hpp>

namespace SSUtils
{
	namespace XML
	{
		std::string saveToString(const boost::property_tree::ptree & pt, const CharType charType)
		{
			std::ostringstream sout;
			boost::property_tree::xml_parser::write_xml(sout, pt,
				boost::property_tree::xml_writer_settings<std::string>('\t', 1, String::CharTypeCode.find(charType)->second));
			return sout.str();
		}

		std::string saveToString(const std::vector<std::shared_ptr<Node>>& roots, const CharType charType)
		{
			static const boost::property_tree::ptree EmptyPTree;

			auto pt(saveToPTree(roots));
			if (pt == EmptyPTree)
			{
				return false;
			}
			else
			{
				return saveToString(pt, charType);
			}
		}

		const bool saveToFile(const std::string & url, const boost::property_tree::ptree & pt, const CharType charType)
		{
			std::string str(saveToString(pt, charType));
			if (!str.empty() && File::insurePathExist(File::getPathOfUrl(url)))
			{
				File::saveFile(url, str);
				return true;
			}
			return false;
		}

		const bool saveToFile(const std::string & url, const std::vector<std::shared_ptr<Node>>& roots, const CharType charType)
		{
			std::string str(saveToString(roots, charType));
			if (!str.empty() && File::insurePathExist(File::getPathOfUrl(url)))
			{
				File::saveFile(url, str);
				return true;
			}
			return false;
		}

		boost::property_tree::ptree::value_type saveToPTreeNode(const std::shared_ptr<Node> node)
		{
			boost::property_tree::ptree::value_type ret;

			for (const auto &attr : node->getAttrs())
			{
				ret.second.put(AttrTag + AttrSeperator + attr.first, attr.second);
			}

			if (node->hasAnyChild())
			{
				for (const auto &childNode : node->getChildren())
				{
					auto child(childNode.lock());
					if (child != nullptr)
					{
						ret.second.push_back(saveToPTreeNode(child));
					}
				}
			}
			else
			{
				ret.second.put(String::EmptyString, node->getContent());
			}

			return std::make_pair(node->getTag(), ret.second);
		}

		boost::property_tree::ptree saveToPTree(const std::shared_ptr<Node> node)
		{
			boost::property_tree::ptree ret;
			ret.insert(ret.end(), saveToPTreeNode(node));
			return ret;
		}

		boost::property_tree::ptree saveToPTree(const std::vector<std::shared_ptr<Node>>& nodes)
		{
			boost::property_tree::ptree ret;
			for (const auto &node : nodes)
			{
				ret.insert(ret.end(), saveToPTreeNode(node));
			}
			return ret;
		}
	};
};

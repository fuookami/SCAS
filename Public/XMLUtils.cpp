#include "XMLUtils.h"
#include "FileUtils.h"

#include <boost/property_tree/xml_parser.hpp>

#include <algorithm>

namespace XMLUtils
{
	const std::string XMLNode::DefaultAttrValue("");

	XMLNode::XMLNode(const std::string & tag)
		: m_tag(tag), m_path(tag), m_content(), m_attrs(), m_children(), m_parent(nullptr)
	{
	}

	XMLNode::XMLNode(const std::string && tag)
		: m_tag(std::move(tag)), m_path(m_tag), m_content(), m_attrs(), m_children(), m_parent(nullptr)
	{
	}

	XMLNode::XMLNode(const XMLNode & ano)
		: m_tag(ano.m_tag), m_path(m_tag), m_content(ano.m_content), m_attrs(ano.m_attrs), m_children(ano.m_children), m_parent(nullptr)
	{
	}

	XMLNode::XMLNode(const XMLNode && ano)
		: m_tag(std::move(ano.m_tag)), m_path(m_tag), m_content(std::move(ano.m_content)), m_attrs(std::move(ano.m_attrs)), m_children(std::move(ano.m_children)), m_parent(nullptr)
	{
	}

	XMLNode & XMLNode::operator=(const XMLNode & rhs)
	{
		m_tag.assign(rhs.m_tag);
		m_path.assign(m_tag);
		m_content.assign(rhs.m_content);
		m_children = rhs.m_children;
		m_attrs = rhs.m_attrs;

		return *this;
	}

	XMLNode & XMLNode::operator=(const XMLNode && rhs)
	{
		m_tag.assign(std::move(rhs.m_tag));
		m_path.assign(m_tag);
		m_content.assign(std::move(rhs.m_content));
		m_attrs = std::move(rhs.m_attrs);
		m_children = std::move(rhs.m_children);

		return *this;
	}

	XMLNode::~XMLNode(void)
	{
	}

	const std::string XMLNode::getAttr(const std::string & key, const std::string & defaultValue) const
	{
		auto it(m_attrs.find(key));
		return it != m_attrs.cend() ? it->second : defaultValue;
	}

	void XMLNode::tidyStruct(void)
	{
		m_path = m_parent != nullptr ? (m_parent->m_path + PathSeperator + m_tag) : m_tag;
		for (auto &child : m_children)
		{
			child.m_parent = this;
			child.tidyStruct();
		}
	}

	XMLNode XMLNode::shallowCopyFrom(const XMLNode & ano)
	{
		XMLNode ret(ano.m_tag);
		ret.m_content = ano.m_content;
		ret.m_attrs = ano.m_attrs;

		return ret;
	}

	XMLNode XMLNode::shallowCopyFrom(const XMLNode && ano)
	{
		XMLNode ret(std::move(ano.m_tag));
		ret.m_content.assign(ano.m_content);
		ret.m_attrs = std::move(ano.m_attrs);

		return ret;
	}

	XMLNode getNode(const boost::property_tree::ptree::value_type & root)
	{
		if (root.first == AttrTag)
		{
			return XMLNode("");
		}

		XMLNode ret(root.first);
		getAttrs(ret, root);

		return ret;
	}

	XMLNode getTree(const boost::property_tree::ptree::value_type & root)
	{
		if (root.first == AttrTag)
		{
			return XMLNode("");
		}

		XMLNode ret(root.first);
		getAttrs(ret, root);
		getChildren(ret, root);

		return ret;
	}

	void getAttrs(XMLNode & node, const boost::property_tree::ptree::value_type & root)
	{
		static const boost::property_tree::ptree EmptyPTree;

		if (root.first == node.tag())
		{
			const auto &attrs(root.second.get_child(AttrTag, EmptyPTree));
			for (const auto &attr : attrs)
			{
				node.addAttr(std::make_pair(attr.first, attr.second.data()));
			}
		}
	}

	void getChildren(XMLNode & node, const boost::property_tree::ptree::value_type & root)
	{
		if (root.first == node.tag())
		{
			for (const auto &childRoot : root.second)
			{
				if (childRoot.first != AttrTag)
				{
					node.addChild(getTree(childRoot));
				}
			}
		}
	}

	const bool openXMLFile(boost::property_tree::ptree & pt, const std::string & fileUrl)
	{
		if (FileUtils::checkFileExist(fileUrl))
		{
			boost::property_tree::xml_parser::read_xml(fileUrl, pt);
			return true;
		}
		else
		{
			return false;
		}
	}

	std::vector<XMLNode> scanXMLFile(const std::string & fileUrl)
	{
		boost::property_tree::ptree root;
		if (openXMLFile(root, fileUrl))
		{
			return scanXMLFile(root);
		}
		else
		{
			return std::vector<XMLNode>();
		}
	}

	std::vector<XMLNode> scanXMLFile(const boost::property_tree::ptree & root)
	{
		std::vector<XMLNode> nodes;
		for (const auto &nodeRoot : root)
		{
			nodes.push_back(XMLUtils::getTree(nodeRoot));
			
			auto &node(nodes.back());
			node.tidyStruct();
			node.setPath(PathSeperator + node.path());
		}

		return nodes;
	}
};

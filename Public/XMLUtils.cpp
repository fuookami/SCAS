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

	XMLNode::XMLNode(std::string && tag)
		: m_tag(std::move(tag)), m_path(m_tag), m_content(), m_attrs(), m_children(), m_parent(nullptr)
	{
	}

	const std::string &XMLNode::getAttr(const std::string & key, const std::string & defaultValue) const
	{
		auto it(m_attrs.find(key));
		return it != m_attrs.cend() ? it->second : defaultValue;
	}

	void XMLNode::tidyStruct(void)
	{
		m_path = m_parent != nullptr ? (m_parent->m_path + PathSeperator + m_tag) : (PathSeperator + m_tag);
		for (auto &child : m_children)
		{
			child.m_parent = this;
			child.tidyStruct();
		}
	}

	int XMLNode::findChild(const std::string & tag, const int pos) const
	{
		auto it(std::find_if(m_children.cbegin() + pos, m_children.cend(), [&tag](const XMLNode &node)->bool
		{
			return node.m_tag == tag;
		}));

		return it != m_children.cend() ? (it - m_children.cbegin()) : NoChild;
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
};

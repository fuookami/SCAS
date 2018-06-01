#include "stdafx.h"
#include "XMLNode.h"
#include "..\StringUtils.h"

namespace SSUtils
{
	namespace XML
	{
		const int Node::npos = -1;
		const std::string Node::DefaultAttrValue = String::EmptyString();

		std::shared_ptr<Node> Node::generate(const std::string & tag)
		{
			std::shared_ptr<Node> ret(new Node(tag));
			ret->m_self = ret;
			return ret;
		}

		std::shared_ptr<Node> Node::generate(std::string && tag)
		{
			std::shared_ptr<Node> ret(new Node(std::move(tag)));
			ret->m_self = ret;
			return ret;
		}

		std::shared_ptr<Node> Node::deepCopy(const Node & ano)
		{
			std::shared_ptr<Node> ret(new Node(ano));
			ret->deepCopyFrom(ano);
			ret->m_self = ret;
			return ret;
		}

		std::shared_ptr<Node> Node::moveCopy(Node && ano)
		{
			std::shared_ptr<Node> ret(new Node(std::move(ano)));
			ret->moveCopyFrom(std::move(ano));
			ret->m_self = ret;
			return ret;
		}

		Node::Node(const std::string & tag)
			: m_tag(tag)
		{
		}

		Node::Node(std::string && tag)
			: m_tag(std::move(tag))
		{
		}

		Node::Node(const Node & ano)
			: Node(ano.m_tag)
		{
			deepCopyFrom(ano);
		}

		Node::Node(Node && ano)
			: Node(std::move(ano.m_tag))
		{
			moveCopyFrom(std::move(ano));
		}

		Node & Node::operator=(const Node & rhs)
		{
			deepCopyFrom(rhs);
			return *this;
		}

		Node & Node::operator=(Node && rhs)
		{
			moveCopyFrom(std::move(rhs));
			return *this;
		}

		void Node::setTag(const std::string & tag)
		{
			m_tag.assign(tag);
		}

		void Node::setTag(std::string && tag)
		{
			m_tag.assign(std::move(tag));
		}

		const std::string & Node::getTag(void) const
		{
			return m_tag;
		}

		void Node::setPath(const std::string & path)
		{
			m_path.assign(path);
		}

		void Node::setPath(std::string && path)
		{
			m_path.assign(std::move(path));
		}

		const std::string & Node::getPath(void) const
		{
			return m_path;
		}

		void Node::setContent(const std::string & content)
		{
			m_content.assign(content);
		}

		void Node::setContent(std::string && content)
		{
			m_content.assign(std::move(content));
		}

		const std::string & Node::getContent(void) const
		{
			return m_content;
		}

		void Node::addAttr(const std::pair<std::string, std::string>& pair)
		{
			m_attrs.insert(pair);
		}

		void Node::addAttr(std::pair<std::string, std::string>&& pair)
		{
			m_attrs.insert(std::move(pair));
		}

		void Node::addAttr(const std::string & key, const std::string & value)
		{
			m_attrs.insert(std::make_pair(key, value));
		}

		void Node::addAttr(const std::string & key, std::string && value)
		{
			m_attrs.insert(std::make_pair(key, std::move(value)));
		}

		void Node::eraseAttr(const std::string & key)
		{
			m_attrs.erase(key);
		}

		void Node::clearAttrs(void)
		{
			m_attrs.clear();
		}

		void Node::setAttr(const std::string & key, const std::string & value)
		{
			m_attrs[key].assign(value);
		}

		void Node::setAttr(const std::string & key, std::string && value)
		{
			m_attrs[key].assign(std::move(value));
		}

		void Node::setAttr(const std::pair<std::string, std::string>& pair)
		{
			m_attrs[pair.first].assign(pair.second);
		}

		void Node::setAttr(std::pair<std::string, std::string>&& pair)
		{
			m_attrs[pair.first].assign(std::move(pair.second));
		}

		const bool Node::hasAttr(const std::string & key) const
		{
			return m_attrs.find(key) != m_attrs.cend();
		}

		const std::string & Node::getAttr(const std::string & key, const std::string & defaultValue) const
		{
			auto it(m_attrs.find(key));
			return it != m_attrs.cend() ? it->second : defaultValue;
		}

		const std::map<std::string, std::string>& Node::getAttrs(void) const
		{
			return m_attrs;
		}

		std::map<std::string, std::string>& Node::getAttrs(void)
		{
			return m_attrs;
		}

		void Node::setParent(const std::weak_ptr<Node> parent)
		{
			auto ori_parent(m_parent.lock());
			if (ori_parent != nullptr)
			{
				ori_parent->removeChild(m_self.lock());
			}
			m_parent = parent;
		}

		void Node::removeParent(void)
		{
			auto parent(m_parent.lock());
			if (parent != nullptr)
			{
				parent->removeChild(m_self.lock());
			}
			m_parent.reset();
		}

		const bool Node::hasParent(void) const
		{
			return m_parent.lock() != nullptr;
		}

		const std::weak_ptr<Node> Node::getParent(void) const
		{
			return m_parent;
		}

		void Node::addChild(const std::shared_ptr<Node> child, const int32 pos)
		{
			if (pos <= npos || pos >= m_children.size())
			{
				m_children.push_back(child);
			}
			else
			{
				m_children.insert(m_children.begin() + pos, child);
			}
		}

		void Node::removeChild(const std::shared_ptr<Node> child)
		{
			if (child != nullptr)
			{
				auto it(std::find_if(m_children.cbegin(), m_children.cend(), [child](const std::shared_ptr<Node> childNode)
				{
					return child == childNode;
				}));
				if (it != m_children.cend())
				{
					m_children.erase(it);
					child->removeParent();
				}
			}
		}

		void Node::clearChild(void)
		{
			m_children.clear();
		}

		const bool Node::hasChild(const std::string & tag) const
		{
			auto it(std::find_if(m_children.cbegin(), m_children.cend(), [&tag](const std::shared_ptr<Node> child)
			{
				return child->getTag() == tag;
			}));
			return it != m_children.cend();
		}

		const uint32 Node::countChild(const std::string &tag) const
		{
			return std::count_if(m_children.cbegin(), m_children.cend(), [&tag](const std::shared_ptr<Node> child)
			{
				return child->getTag() == tag;
			});
		}

		const bool Node::hasAnyChild(void) const
		{
			return !m_children.empty();
		}

		const int Node::findChild(const std::string & tag, const int pos) const
		{
			for (uint32 i(pos), j(static_cast<uint32>(m_children.size())); i != j; ++i)
			{
				if (m_children[i]->getTag() == tag)
				{
					return i;
				}
			}
			return npos;
		}

		const std::vector<std::shared_ptr<Node>>& Node::getChildren(void) const
		{
			return m_children;
		}

		std::vector<std::shared_ptr<Node>>& Node::getChildren(void)
		{
			return m_children;
		}

		void Node::deepCopyFrom(const Node & ano)
		{
			m_path.assign(m_tag);
			m_content.assign(ano.m_content);
			m_attrs = ano.m_attrs;
			for (auto child : ano.m_children)
			{
				m_children.push_back(deepCopy(*child));
				
			}
		}

		void Node::moveCopyFrom(Node && ano)
		{
			m_path.assign(m_tag);
			m_content.assign(std::move(ano.m_content));
			m_attrs = std::move(ano.m_attrs);
			m_children = std::move(ano.m_children);
		}
	};
};

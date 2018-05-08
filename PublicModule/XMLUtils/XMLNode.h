#pragma once

#include "_pri_xml_global.h"
#include <map>
#include <memory>

namespace SSUtils
{
	namespace XML
	{
		class Node
		{
		public:
			static const int npos;

		private:
			static const std::string DefaultAttrValue;

		public:
			static std::shared_ptr<Node> generate(const std::string &tag);
			static std::shared_ptr<Node> generate(std::string &&tag);
			static std::shared_ptr<Node> deepCopy(const Node &ano);
			static std::shared_ptr<Node> moveCopy(Node &&ano);

		private:
			Node(const std::string &tag);
			Node(std::string &&tag);
			Node(const Node &ano);
			Node(Node &&ano);
		public:
			Node &operator=(const Node &rhs);
			Node &operator=(Node &&rhs);
			~Node(void) = default;

		public:
			void setTag(const std::string &tag);
			void setTag(std::string &&tag);
			const std::string &getTag(void) const;

			void setPath(const std::string &path);
			void setPath(std::string &&path);
			const std::string &getPath(void) const;

			void setContent(const std::string &content);
			void setContent(std::string &&content);
			const std::string &getContent(void) const;

		public:
			void addAttr(const std::pair<std::string, std::string> &pair);
			void addAttr(std::pair<std::string, std::string> &&pair);
			void addAttr(const std::string &key, const std::string &value);
			void addAttr(const std::string &key, std::string &&value);
			void eraseAttr(const std::string &key);
			void clearAttrs(void);
			void setAttr(const std::string &key, const std::string &value);
			void setAttr(const std::string &key, std::string &&value);
			void setAttr(const std::pair<std::string, std::string> &pair);
			void setAttr(std::pair<std::string, std::string> &&pair);
			const bool hasAttr(const std::string &key) const;
			const std::string &getAttr(const std::string &key, const std::string &defaultValue = DefaultAttrValue) const;
			const std::map<std::string, std::string> &getAttrs(void) const;
			std::map<std::string, std::string> &getAttrs(void);

		public:
			void setParent(const std::weak_ptr<Node> parent);
			void removeParent(void);
			const bool hasParent(void) const;
			const std::weak_ptr<Node> getParent(void) const;

		public:
			void addChild(const std::shared_ptr<Node> child);

			void removeChild(const std::shared_ptr<Node> child);
			template<typename fun_t, typename U = std::enable_if_t<std::is_function_v<fun_t>>>
			void removeChildren(const fun_t fun)
			{
				std::remove_if(m_children.cbegin(), m_children.cend(), [fun](const std::shared_ptr<Node> child)
				{
					return child == nullptr ? false : fun(*child);
				});
			}

			void clearChild(void);

			const bool hasChild(const std::string &tag) const;
			template<typename fun_t, typename U = std::enable_if_t<std::is_function_v<fun_t>>>
			const bool hasChild(const fun_t fun) const
			{
				auto it(std::find_if(m_children.cbegin(), m_children.cend(), [fun](const std::shared_ptr<Node> child)
				{
					return child == nullptr ? false : fun(*child);
				}));
				return it != m_children.cend();
			}
			const bool hasAnyChild(void) const;

			const int findChild(const std::string &tag, const int pos = 0) const;
			template<typename fun_t, typename U = std::enable_if_t<std::is_function_v<fun_t>>>
			const int findChild(const fun_t fun, const int pos = 0) const
			{
				for (int i(pos), j(m_children.size()); i != j; ++i)
				{
					auto child(m_children[i].lock());
					if (child != nullptr && fun(*child))
					{
						return i;
					}
				}
				return npos;
			}

			const std::vector<std::shared_ptr<Node>> &getChildren(void) const;
			std::vector<std::shared_ptr<Node>> &getChildren(void);

		private:
			void deepCopyFrom(const Node &ano);
			void moveCopyFrom(Node &&ano);

		private:
			std::string m_tag;
			std::string m_path;
			std::string m_content;
			std::map<std::string, std::string> m_attrs;
			
			std::weak_ptr<Node> m_self;
			std::weak_ptr<Node> m_parent;
			std::vector<std::shared_ptr<Node>> m_children;
		};
	};
};

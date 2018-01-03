#pragma once

#include <boost/property_tree/ptree.hpp>

#include <vector>
#include <map>
#include <string>

namespace XMLUtils
{
	static const std::string AttrTag("<xmlattr>");
	static const std::string PathSeperator("/");

	class XMLNode
	{
	private:
		static const std::string DefaultAttrValue;

	public:
		XMLNode(const std::string &tag);
		XMLNode(const std::string &&tag);
		XMLNode(const XMLNode &rhs);
		XMLNode(const XMLNode &&rhs);
		XMLNode &operator=(const XMLNode &rhs);
		XMLNode &operator=(const XMLNode &&rhs);
		~XMLNode(void);

		inline void setTag(const std::string &tag) { m_tag.assign(tag); };
		inline void setTag(const std::string &&tag) { m_tag.assign(std::move(tag)); }
		inline const std::string &getTag(void) const { return m_tag; }

		inline void setPath(const std::string &path) { m_path.assign(path); }
		inline void setPath(const std::string &&path) { m_path.assign(std::move(path)); }
		inline const std::string &getPath(void) const { return m_path; }

		inline void setContent(const std::string &content) { m_content.assign(content); }
		inline void setContent(const std::string &&content) { m_content.assign(std::move(content)); }
		inline const std::string &getContent(void) const { return m_content; }

		inline void addAttr(const std::pair<std::string, std::string> &pair) { m_attrs.insert(pair); }
		inline void addAttr(const std::pair<std::string, std::string> &&pair) { m_attrs.insert(std::move(pair)); }
		inline void eraseAttr(const std::string &key) { m_attrs.erase(key); }
		inline const bool hasAttr(const std::string &key) const { return m_attrs.find(key) != m_attrs.cend(); }
		const std::string getAttr(const std::string &key, const std::string &defaultValue = DefaultAttrValue) const;
		inline void setAttr(const std::string &key, const std::string &value) { m_attrs[key].assign(value); }
		inline void setAttr(const std::string &key, const std::string &&value) { m_attrs[key].assign(std::move(value)); }
		inline const std::map<std::string, std::string> &getAttrs(void) const { return m_attrs; }

	public:
		void tidyStruct(void);

	public:
		inline void addChild(const XMLNode &child) { m_children.push_back(child); }
		inline void addChild(const XMLNode &&child) { m_children.push_back(std::move(child)); }
		inline const std::vector<XMLNode> &getChildren(void) const { return m_children; }
		inline std::vector<XMLNode> &getChildren(void) { return m_children; }

		inline void setParent(XMLNode &parent) { m_parent = &parent; }
		inline void removeParent(void) { m_parent = nullptr; }
		inline const bool hasParent(void) const { return m_parent == nullptr; }
		inline XMLNode *getParent(void) { return m_parent; }

	public:
		static XMLNode shallowCopyFrom(const XMLNode &ano);
		static XMLNode shallowCopyFrom(const XMLNode &&ano);

	private:
		std::string m_tag;
		std::string m_path;
		std::string m_content;
		std::map<std::string, std::string> m_attrs;

	private:
		XMLNode *m_parent;
		std::vector<XMLNode> m_children;
	};

	XMLNode getNode(const boost::property_tree::ptree::value_type & root);
	XMLNode getTree(const boost::property_tree::ptree::value_type & root);
	void getAttrs(XMLNode &node, const boost::property_tree::ptree::value_type & root);
	void getChildren(XMLNode &node, const boost::property_tree::ptree::value_type & root);

	const bool openXMLFile(boost::property_tree::ptree &pt, const std::string &fileUrl);
	std::vector<XMLNode> scanXMLFile(const std::string &fileUrl);
	std::vector<XMLNode> scanXMLFile(const boost::property_tree::ptree &root);
};

#pragma once

#include "_pri_xml_global.h"
#include "StringUtils.h"
#include <memory>

namespace SSUtils
{
	namespace XML
	{
		class Document
		{
		public:
			Document(void) = default;
			Document(const Document &ano);
			Document(Document &&ano) = default;
			Document &operator=(const Document &rhs);
			Document &operator=(Document &&rhs) = default;
			~Document(void) = default;

		public:
			static Document fromFile(const std::string &url, const CharType charType = String::LocalCharType);
			static Document fromFile(std::string &&url, const CharType charType = String::LocalCharType);
			static Document fromString(const std::string &data, const CharType charType = String::LocalCharType);
			static Document fromString(std::string &&data, const CharType charType = String::LocalCharType);

			const bool toFile(const std::string &url, const CharType charType = String::LocalCharType);
			std::string toString(const CharType charType = String::LocalCharType);

		public:
			const std::vector<std::shared_ptr<Node>> &getRoots(void) const;
			std::vector<std::shared_ptr<Node>> &getRoots(void);
			void setRoots(const std::vector<std::shared_ptr<Node>> &roots);
			void setRoots(std::vector<std::shared_ptr<Node>> &&roots);

		private:
			std::vector<std::shared_ptr<Node>> m_roots;
		};
	};
};

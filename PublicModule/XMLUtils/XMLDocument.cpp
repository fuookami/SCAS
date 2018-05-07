#include "XMLDocument.h"
#include "XMLLoader.h"
#include "XMLSaver.h"
#include "XMLNode.h"

namespace SSUtils
{
	namespace XML
	{
		Document::Document(const Document &ano)
		{
			setRoots(ano.m_roots);
		}

		Document & Document::operator=(const Document & rhs)
		{
			setRoots(rhs.m_roots);
			return *this;
		}

		Document Document::fromFile(const std::string & url, const CharType charType)
		{
			Document doc;
			Loader loader(url, charType);
			doc.m_roots = loader();
			return doc;
		}

		Document Document::fromFile(std::string && url, const CharType charType)
		{
			Document doc;
			Loader loader(std::move(url), charType);
			doc.m_roots = loader();
			return doc;
		}

		Document Document::fromString(const std::string & data, const CharType charType)
		{
			Document doc;
			Scaner scaner(data, charType);
			doc.m_roots = scaner();
			return doc;
		}

		Document Document::fromString(std::string && data, const CharType charType)
		{
			Document doc;
			Scaner scaner(std::move(data), charType);
			doc.m_roots = scaner();
			return doc;
		}

		const bool Document::toFile(const std::string & url, const CharType charType)
		{
			Saver saver(url, m_roots, charType);
			return saver.toFile();
		}

			std::string Document::toString(const CharType charType)
		{
			Saver saver(m_roots, charType);
			return saver.toString();
		}

		const std::vector<std::shared_ptr<Node>> Document::getRoots(void) const
		{
			return m_roots;
		}

		std::vector<std::shared_ptr<Node>> Document::getRoots(void)
		{
			return m_roots;
		}

		void Document::setRoots(const std::vector<std::shared_ptr<Node>>& roots)
		{
			m_roots.clear();
			for (const auto &root : roots)
			{
				if (root != nullptr)
				{
					m_roots.push_back(Node::deepCopy(*root));
				}
			}
		}

		void Document::setRoots(std::vector<std::shared_ptr<Node>>&& roots)
		{
			m_roots = std::move(roots);
		}
	};
};

#include "XMLSaver.h"
#include "_pri_xml_saver.h"
#include "FileUtils.h"

namespace SSUtils
{
	namespace XML
	{
		Saver::Saver(const CharType _charType)
			: charType(_charType)
		{
		}

		Saver::Saver(const std::string & _url, const std::vector<std::shared_ptr<Node>>& _roots, const CharType _charType)
			: url(_url), roots(std::shared_ptr<std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>>(new std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>(_roots))), charType(_charType)
		{
		}

		Saver::Saver(std::string && _url, const std::vector<std::shared_ptr<Node>>& _roots, const CharType _charType)
			: url(std::move(_url)), roots(std::shared_ptr<std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>>(new std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>(_roots))), charType(_charType)
		{
		}

		Saver::Saver(const std::string & _url, const boost::property_tree::ptree & _pt, const CharType _charType)
			: url(_url), pt(std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>>(new std::reference_wrapper<const boost::property_tree::ptree>(_pt))), charType(_charType)
		{
		}

		Saver::Saver(std::string && _url, const boost::property_tree::ptree & _pt, const CharType _charType)
			: url(std::move(_url)), pt(std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>>(new std::reference_wrapper<const boost::property_tree::ptree>(_pt))), charType(_charType)
		{
		}

		Saver::Saver(const std::vector<std::shared_ptr<Node>>& _roots, const CharType _charType)
			: roots(std::shared_ptr<std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>>(new std::reference_wrapper<const std::vector<std::shared_ptr<Node>>>(_roots))), charType(_charType)
		{
		}

		Saver::Saver(const boost::property_tree::ptree & _pt, const CharType _charType)
			: pt(std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>>(new std::reference_wrapper<const boost::property_tree::ptree>(_pt))), charType(_charType)
		{
		}

		std::string Saver::toString(void)
		{
			return pt != nullptr ? toString(pt->get(), charType)
				: roots != nullptr ? toString(roots->get(), charType) 
				: String::EmptyString;
		}

		std::string Saver::toString(const std::vector<std::shared_ptr<Node>>& roots, const CharType charType)
		{
			return saveToString(roots, charType);
		}

		std::string Saver::toString(const boost::property_tree::ptree & _pt, const CharType _charType)
		{
			return saveToString(_pt, charType);
		}

		const bool Saver::toFile(void)
		{
			if (File::insurePathExist(File::getPathOfUrl(url)))
			{
				return pt != nullptr ? toFile(url, pt->get(), charType)
					: roots != nullptr ? toFile(url, roots->get(), charType)
					: false;
			}
			return false;
		}

		const bool Saver::toFile(const std::string & _url, const std::vector<std::shared_ptr<Node>>& _roots, const CharType _charType)
		{
			return saveToFile(_url, _roots, _charType);
		}

		const bool Saver::toFile(const std::string & _url, const boost::property_tree::ptree & _pt, const CharType _charType)
		{
			return saveToFile(_url, _pt, _charType);
		}
	};
};

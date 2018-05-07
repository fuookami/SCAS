#include "_pri_xml_loader.h"
#include "XMLLoader.h"

namespace SSUtils
{
	namespace XML
	{
		Loader::Loader(const CharType _charType)
			: charType(_charType)
		{
		}

		Loader::Loader(const std::string & _url, const CharType _charType)
			: url(_url), charType(_charType)
		{
		}

		Loader::Loader(std::string && _url, const CharType _charType)
			: url(std::move(_url)), charType(_charType)
		{
		}

		Loader::Loader(const boost::property_tree::ptree & _pt, const CharType _charType)
			: pt(std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>>(new std::reference_wrapper<const boost::property_tree::ptree>(_pt))), charType(_charType)
		{
		}

		const bool Loader::isOpened(void) const
		{
			return pt != nullptr;
		}

		std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> Loader::open(void)
		{
			if (!url.empty())
			{
				return open(url);
			}
			else
			{
				return pt;
			}
		}

		std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> Loader::open(const std::string & _url)
		{
			openXMLFile(ori_pt, _url);
			pt.reset(new std::reference_wrapper<const boost::property_tree::ptree>(ori_pt));
			return pt;
		}

		std::vector<std::shared_ptr<Node>> Loader::operator()()
		{
			if (!isOpened())
			{
				open();
			}
			return isOpened() ? operator()(pt->get(), charType) : std::vector<std::shared_ptr<Node>>();
		}

		std::vector<std::shared_ptr<Node>> Loader::operator()(const boost::property_tree::ptree & _pt, const CharType _charType)
		{
			return loadXML(_pt, charType);
		}

		Scaner::Scaner(const CharType _charType)
			: charType(_charType)
		{
		}

		Scaner::Scaner(const std::string & _data, const CharType _charType)
			: data(_data), charType(_charType)
		{
		}

		Scaner::Scaner(std::string && _data, const CharType _charType)
			: data(std::move(_data)), charType(_charType)
		{
		}

		Scaner::Scaner(const boost::property_tree::ptree & _pt, const CharType _charType)
			: pt(std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>>(new std::reference_wrapper<const boost::property_tree::ptree>(_pt))), charType(_charType)
		{
		}

		const bool Scaner::isScaned(void) const
		{
			return pt != nullptr;
		}

		std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> Scaner::scan(void)
		{
			if (!data.empty())
			{
				return scan(data);
			}
			else
			{
				return pt;
			}
		}

		std::shared_ptr<std::reference_wrapper<const boost::property_tree::ptree>> Scaner::scan(const std::string & _data)
		{
			scanXMLString(ori_pt, _data);
			pt.reset(new std::reference_wrapper<const boost::property_tree::ptree>(ori_pt));
			return pt;
		}

		std::vector<std::shared_ptr<Node>> Scaner::operator()()
		{
			if (!isScaned())
			{
				scan();
			}
			return isScaned() ? operator()(pt->get(), charType) : std::vector<std::shared_ptr<Node>>();
		}

		std::vector<std::shared_ptr<Node>> Scaner::operator()(const boost::property_tree::ptree & _pt, const CharType _charType)
		{
			return loadXML(_pt, charType);
		}
	};
};

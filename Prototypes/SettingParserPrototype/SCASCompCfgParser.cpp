#include "SCASCompCfgParser.h"

namespace SCAS
{
	namespace CompCfg
	{
		const bool saveToXML(const std::shared_ptr<CompetitionInfo> info, const std::string & outUrl)
		{
			return false;
		}

		const std::shared_ptr<CompetitionInfo> readFromXML(const std::string & inUrl)
		{
			return std::shared_ptr<CompetitionInfo>();
		}
	};
};

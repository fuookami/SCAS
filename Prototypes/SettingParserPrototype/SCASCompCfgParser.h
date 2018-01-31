#pragma once

#include "CompetitionInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		const bool saveToXML(const std::shared_ptr<CompetitionInfo> info, const std::string &outUrl);
		const std::shared_ptr<CompetitionInfo> readFromXML(const std::string &inUrl);
	};
};

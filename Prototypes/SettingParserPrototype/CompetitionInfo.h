#pragma once

#include "GameInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		class RankInfo
		{
		private:
			bool m_enabled;
			bool m_forced;
			
			std::vector<std::pair<std::string, std::string>> m_ranks;
			std::pair<std::string, std::string> m_defaultRank;
		};

		class CompetitionInfo
		{
		public:
		private:
			ScoreInfo m_publicScoreInfo;
		};
	};
};
